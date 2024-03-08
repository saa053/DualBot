using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] GameObject rewardPrefab;
    [SerializeField] TextMeshProUGUI ui;
    [SerializeField] GameObject createFx;
    int rewardCount;

    static public RewardManager instance;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rewardCount = 0;
    }

    void Update()
    {
        ui.text = rewardCount.ToString();
    }

    public void IncrementRewardCount()
    {
        rewardCount++;
    }

    public void CreateReward(Vector3 pos)
    {

        pos += rewardPrefab.transform.position;
        Instantiate(rewardPrefab, pos, Quaternion.identity);
        PlayFx(createFx, pos);
    }

    void PlayFx(GameObject fx, Vector3 pos)
    {
        GameObject particleObject = Instantiate(fx, pos, fx.transform.rotation);
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }


}