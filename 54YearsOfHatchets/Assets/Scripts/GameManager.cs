using UnityEngine;
using System.Collections;
using UnityStandardAssets.Cameras;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager thisGameManager;

    public AutoCam camController;
    public Camera cam;
    bool freeMove = true;
    public GameObject collection;

    public List<Person> people = new List<Person>();
    public int startPeople = 25;
    public GameObject person;
    public Transform spawnSpot;

    UIManager thisUI;
    void Awake()
    {
        thisGameManager = this;
    }

    // Use this for initialization
    void Start()
    {
        thisUI = UIManager.thisUI;

        for (int i = 0; i < startPeople; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 10f;
            GameObject g = (GameObject)Instantiate(person, new Vector3(spawnSpot.position.x + randomDirection.x, 0.5f, spawnSpot.position.z + randomDirection.z), Quaternion.identity);
            people.Add(g.GetComponent<Person>());
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift))
        {
            if (!freeMove)
            {
                camController.enabled = false;
                freeMove = true;
            }
            else
                ResetCamera();

        }

        if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))
        {
            thisUI.RemoveAll();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (ISelectable s in thisUI.selected)
            {
                Person p = s.IGameObject.GetComponent<Person>();
                if (p != null)
                {
                    p.DropItem();
                }
            }
        }

        if (!freeMove)
            ResetCamera();
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                camController.transform.Translate(Vector3.forward);
            }
            if (Input.GetKey(KeyCode.S))
            {
                camController.transform.Translate(Vector3.back);

            }
            if (Input.GetKey(KeyCode.D))
            {
                camController.transform.Translate(Vector3.right);

            }
            if (Input.GetKey(KeyCode.A))
            {
                camController.transform.Translate(Vector3.left);

            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = Vector3.zero;
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Pickup pu = hit.collider.gameObject.GetComponent<Pickup>();
                if (pu != null)
                {
                    foreach (ISelectable s in thisUI.selected)
                    {
                        Person p = s.IGameObject.GetComponent<Person>();
                        if (p != null)
                        {
                            p.PickupItem(pu.pickUpType);
                            thisUI.UpdateSelectedView(s);
                        }
                    }
                }
                else
                {
                    foreach (ISelectable s in thisUI.selected)
                    {
                        Person p = s.IGameObject.GetComponent<Person>();
                        if (p != null)
                        {
                            p.MoveToTarget(hit.point);
                            thisUI.UpdateSelectedView(s);
                        }
                    }
                }
            }
            else
                return;


        }
    }

    public void ResetCamera()
    {
        if (thisUI.selected.Count > 0)
        {
            camController.enabled = true;
            freeMove = false;

            Vector3 average = Vector3.zero;
            for (int i = 0; i < thisUI.selected.Count; i++)
            {
                average += thisUI.selected[i].location;
            }

            collection.transform.position = average / thisUI.selected.Count;
            camController.SetTarget(collection.transform);
        }
        else
        {
            camController.enabled = false;
            freeMove = true;
        }
    }
}
