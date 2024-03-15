using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PortableTrigger : MonoBehaviour
{
    [SerializeField] Color outlineColor;
    List<GameObject> portablesInside;
    void Start()
    {
        portablesInside = new List<GameObject>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Portable")
        {
            portablesInside.Add(other.gameObject);
            other.transform.GetComponentInChildren<Outline>().OutlineWidth = 5;
            other.transform.GetComponentInChildren<Outline>().OutlineColor = outlineColor;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Portable")
        {
            portablesInside.Remove(other.gameObject);
            other.transform.GetComponentInChildren<Outline>().OutlineWidth = 0;
        }
    }

    public int GetCount()
    {
        return portablesInside.Count;
    }

    public List<GameObject> GetList()
    {
        return portablesInside;
    }
}
