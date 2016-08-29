using UnityEngine;
using System.Collections;

public class House : MonoBehaviour
{

    public Transform spawnArea;
    public int amount;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 10f;
            GameObject g = (GameObject)Instantiate(GameManager.thisGameManager.person, new Vector3(spawnArea.position.x + randomDirection.x, 0.5f, spawnArea.position.z + randomDirection.z), Quaternion.identity);
            Person p = g.GetComponent<Person>();
            GameManager.thisGameManager.people.Add(p);
            p.tribe = "Arcadio";
            p.Description = "A person from the Arcadio tribe";
        }
    }
}
