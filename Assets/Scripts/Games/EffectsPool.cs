using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolAmount = 100;
    private List<GameObject> pool = new List<GameObject>();
    public float effectTime = 2f;

    private void Awake()
    {
        GameObject tmp;
        for (int i = 0; i < poolAmount; ++i)
        {
            tmp = Instantiate(prefab, transform);
            pool.Add(tmp);
            tmp.SetActive(false);
        }
    }

    public void SpawnObject(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < pool.Count; ++i)
        {
            if (!pool[i].activeSelf)
            {
                pool[i].transform.position = position;
                pool[i].transform.rotation = rotation;
                pool[i].SetActive(true);
                StartCoroutine(DisableObject(pool[i]));
                break;
            }
        }
    }

    IEnumerator DisableObject(GameObject go)
    {
        yield return new WaitForSeconds(effectTime);
        go.SetActive(false);
    }
}
