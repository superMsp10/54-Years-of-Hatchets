using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour
{
    public int mapSize = 1000;

    public int resourceLimit = 100;
    public GameObject[] resources;

    // Use this for initialization
    void Start()
    {

        for (int i = 0; i < resourceLimit; i++)
        {
            float within = (float)mapSize / 2;
            Vector3 pos = new Vector3(Random.Range(-within, within), 0, Random.Range(-within, within));
            GameObject g = (GameObject)Instantiate(resources[Random.Range(0, resources.Length)], pos, Quaternion.identity);
            g.transform.parent = transform;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
