using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public class PortableItem
{
    public string info;
    public string explanation;
    public bool safe;

    public PortableItem(string info, string explanation, bool safe)
    {
        this.info = info;
        this.explanation = explanation;
        this.safe = safe;
    }
}

public class PortableManager : MonoBehaviour
{
    [Header("Game bools")]
    Room room;
    [SerializeField] bool startGame;
    bool portablesSpawned = false;
    bool isSpawning = false;
    bool evaluate = false;
    bool resultsReady = false;
    bool isShowingResults = false;

    [Header("Portables Settings")]
    [SerializeField] GameObject implodeFxPrefab;
    [SerializeField] GameObject fxParent;
    [SerializeField] int numPortables;
    [SerializeField] List<PortableItem> portableSpawnList;
    [SerializeField] GameObject portablePrefab;
    [SerializeField] GameObject portableParent;
    [SerializeField] Vector3 startSpawnPos;
    [SerializeField] Vector3 spaceBetweenPortables;
    [SerializeField] float timeBetweenSpawns;

    [Header("Sorting Squares Settings")]
    [SerializeField] PortableTrigger safeTrigger;
    [SerializeField] PortableTrigger unsafeTrigger;
    List<GameObject> safeList;
    List<GameObject> unsafeList;
    List<GameObject> resultList;

    bool allPortablesInside;

    [Header("Red button")]
    [SerializeField] Trigger buttonTrigger;

    [Header("Result Settings")]
    [SerializeField] Color correctColor;
    [SerializeField] Color wrongColor;
    [SerializeField] AudioSource correctSound;
    [SerializeField] AudioSource wrongSound;
    [SerializeField] float resultPauseBetweenPortables;
    [SerializeField] float removePause;


    void Start()
    {
       room = GetComponentInParent<Room>();

       safeList = new List<GameObject>();
       unsafeList = new List<GameObject>();
       resultList = new List<GameObject>();
    }

    void Update()
    {
        HandleRedButton();

        if (!startGame)
            return;

        if (!portablesSpawned && !isSpawning)
        {
            StartCoroutine(SpawnPortables());
            return;
        }
        
        HandleSortingSquares();

        if (resultsReady && !isShowingResults)
            StartCoroutine(ShowResults());
    }

    IEnumerator SpawnPortables()
    {
        isSpawning = true;

        for (int i = 0; i < numPortables; i++)
        {
            Vector3 spawnPos = (startSpawnPos + spaceBetweenPortables * i) + room.GetRoomCenter();
            GameObject portable = Instantiate(portablePrefab, spawnPos, Quaternion.identity);

            portable.GetComponentInChildren<TextMeshProUGUI>().text = portableSpawnList[i].info;
            portable.GetComponent<Portable>().SetSafe(portableSpawnList[i].safe);
            portable.GetComponent<Portable>().SetExplanation(portableSpawnList[i].explanation);

            // Assign color?

            portable.transform.SetParent(portableParent.transform);

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        portablesSpawned = true;
        isSpawning = false;
    }

    void HandleSortingSquares()
    {
        int portablesSortedCount = safeTrigger.GetCount() + unsafeTrigger.GetCount();
        if (portablesSortedCount >= numPortables)
            allPortablesInside = true;
        else
            allPortablesInside = false;


        if (allPortablesInside)
        {
            if (safeList.Count == 0)
                safeList = safeTrigger.GetList();
            if (unsafeList.Count == 0)
                unsafeList = unsafeTrigger.GetList();
        }
        
    }

    void HandleRedButton()
    {
        if (buttonTrigger.Player1Trigger() || buttonTrigger.Player2Trigger())
        {
            if (!startGame)
                Debug.Log("Go to NPC to start game");
            else if (!portablesSpawned)
                Debug.Log("Portables not spawned yet");
            else if (!allPortablesInside)
                Debug.Log("All portables not sorted");
            else
                evaluate = true;
        }

        if (evaluate)
            EvaluateResult();
    }

    void EvaluateResult()
    {
        evaluate = false;

        int correct = 0;

        LockPortables(safeList);
        LockPortables(unsafeList);

        foreach (GameObject item in safeList)
        {
            Portable portable = item.GetComponent<Portable>();
            if (portable.GetSafe())
            {
                portable.SetResult(1);
                correct++;
            }
            else
                portable.SetResult(0);
        }

        foreach (GameObject item in unsafeList)
        {
            Portable portable = item.GetComponent<Portable>();
            if (!portable.GetSafe())
            {
                portable.SetResult(1);
                correct++;
            }
            else
                portable.SetResult(0);
        }

        resultList = safeList.Concat(unsafeList).ToList();
        resultList = resultList.OrderBy(obj => obj.transform.position.x).ToList();
        resultsReady = true;
    }

    void LockPortables(List<GameObject> list)
    {
        foreach (GameObject item in list)
        {
            Portable portable = item.GetComponent<Portable>();
            portable.Lock();
        }
    }

    IEnumerator ShowResults()
    {
        isShowingResults = true;
        Portable lastPortable = null;

        foreach (GameObject item in resultList)
        {
            if (lastPortable != null)
                lastPortable.ToggleCanvas(false);

            Portable portable = item.GetComponent<Portable>();
            portable.ToggleCanvas(true);

            yield return new WaitForSeconds(resultPauseBetweenPortables / 2);

            int res = portable.GetResult();

            if (res == 0)
            {
                portable.GetComponentInChildren<Outline>().OutlineWidth = 10;
                portable.GetComponentInChildren<Outline>().OutlineColor = wrongColor;
                wrongSound.Play();
            }
            else if (res == 1)
            {
                portable.GetComponentInChildren<Outline>().OutlineWidth = 10;
                portable.GetComponentInChildren<Outline>().OutlineColor = correctColor;
                correctSound.Play();
            }
            else
                Debug.Log("Result is not set!");

            lastPortable = portable;

            yield return new WaitForSeconds(resultPauseBetweenPortables / 2);
        }

        lastPortable.ToggleCanvas(false);

        StartCoroutine(RemovePortables());
    }

    IEnumerator RemovePortables()
    {
        foreach (GameObject item in resultList)
        {
            Portable portable = item.GetComponent<Portable>();
            GameObject fx = Instantiate(implodeFxPrefab, portable.transform.position, implodeFxPrefab.transform.rotation);
            fx.transform.SetParent(fxParent.transform);
            StartCoroutine(portable.Implode(fx.GetComponent<ParticleSystem>()));

            yield return new WaitForSeconds(removePause);
        }
    }
}
