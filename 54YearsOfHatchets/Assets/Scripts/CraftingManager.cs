using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{

    UIManager thisUI;
    int sticks = 0, rocks = 0;
    public Image craftAxe;
    public Image craftHouse;

    public GameObject axe;
    public GameObject house;

    public Transform craftArea;
    public Transform houseArea;

    public float wantedDistance;
    public List<ISelectable> resources = new List<ISelectable>();
    public static int houses = 0;


    // Use this for initialization
    void Start()
    {
        thisUI = UIManager.thisUI;

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CraftAxe()
    {
        int wantedRocks = 1, wantedSticks = 2;
        ResourceCount();
        if (rocks >= 1 && sticks >= 2)
        {
            craftAxe.color = Color.blue;
            List<Pickup> destroy = new List<Pickup>();

            foreach (ISelectable s in resources)
            {
                Pickup p = s.IGameObject.GetComponent<Pickup>();
                if (p != null)
                {
                    switch (p.pickUpType)
                    {
                        case "Stick":
                            if (wantedSticks > 0)
                            {
                                wantedSticks--;
                                destroy.Add(p);
                            }
                            break;
                        case "Rock":
                            if (wantedRocks > 0)
                            {
                                wantedRocks--;
                                destroy.Add(p);
                            }
                            break;
                    }


                }
            }
            for (int i = 0; i < destroy.Count; i++)
            {
                thisUI.RemoveSelected(destroy[i]);
                Destroy(destroy[i].IGameObject);
            }
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 2f;
            Instantiate(axe, new Vector3(craftArea.position.x + randomDirection.x, 2f, craftArea.position.z + randomDirection.z), Quaternion.identity);


        }
        else
        {
            craftAxe.color = Color.red;
        }

    }


    public void CraftHouse()
    {
        int wantedRocks = 10, wantedSticks = 25;
        ResourceCount();
        if (rocks >= 10 && sticks >= 25)
        {
            craftHouse.color = Color.blue;
            List<Pickup> destroy = new List<Pickup>();

            foreach (ISelectable s in resources)
            {
                Pickup p = s.IGameObject.GetComponent<Pickup>();
                if (p != null)
                {
                    switch (p.pickUpType)
                    {
                        case "Stick":
                            if (wantedSticks > 0)
                            {
                                wantedSticks--;
                                destroy.Add(p);
                            }
                            break;
                        case "Rock":
                            if (wantedRocks > 0)
                            {
                                wantedRocks--;
                                destroy.Add(p);
                            }
                            break;
                    }


                }
            }
            for (int i = 0; i < destroy.Count; i++)
            {
                thisUI.RemoveSelected(destroy[i]);
                Destroy(destroy[i].IGameObject);
            }
            Instantiate(house, new Vector3(houseArea.position.x + houses * 10f, 0f, houseArea.position.z), Quaternion.identity);
            houses++;

        }
        else
        {
            craftHouse.color = Color.red;
        }

    }

    void ResourceCount()
    {
        resources.Clear();
        sticks = 0;
        rocks = 0;
        foreach (ISelectable s in thisUI.selected)
        {
            if (Vector3.Distance(s.IGameObject.transform.position, craftArea.position) < wantedDistance)
            {
                Pickup p = s.IGameObject.GetComponent<Pickup>();
                if (p != null)
                {
                    switch (p.pickUpType)
                    {
                        case "Stick":
                            sticks++;
                            resources.Add(p);
                            break;
                        case "Rock":
                            rocks++;
                            resources.Add(p);
                            break;
                    }
                }
            }

        }
    }

}
