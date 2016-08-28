using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Cameras;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

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


    public Light sun;
    public float secondsInFullDay = 120f;
    [Range(0, 1)]
    public float currentTimeOfDay = 0;
    [HideInInspector]
    public float timeMultiplier = 1f;

    float sunInitialIntensity;
    public float minimumSunLight;

    int years = 0;
    bool warTime = false;
    public GameObject NriabTribe;

    public GameObject GOPanel;
    public GameObject ViewPanel;
    public GameObject HelpPanel;


    void Awake()
    {
        thisGameManager = this;
    }

    // Use this for initialization
    void Start()
    {
        thisUI = UIManager.thisUI;
        sunInitialIntensity = sun.intensity;
        thisUI.yearNum.text = years.ToString();
        thisUI.yearSpeed.text = "(X" + Time.timeScale.ToString() + ")";

        for (int i = 0; i < startPeople; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 10f;
            GameObject g = (GameObject)Instantiate(person, new Vector3(spawnSpot.position.x + randomDirection.x, 0.5f, spawnSpot.position.z + randomDirection.z), Quaternion.identity);
            Person p = g.GetComponent<Person>();
            people.Add(p);
            p.tribe = "Arcadio";
            p.Description = "A person from the Arcadio tribe";
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (people.Count <= 5)
        {
            GameOver();
        }

        UpdateControls();
        UpdateSun();

        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
            years += 1;
            thisUI.yearNum.text = years.ToString();
        }

        if (years == 54 && warTime == false)
        {
            warTime = true;
            Time.timeScale = 1f;
            thisUI.yearSpeed.text = "(X" + Time.timeScale.ToString() + ")";
            thisUI.yearSpeed.color = Color.grey;
            SpawnEnemies(100);


        }

    }

    public void GameOver()
    {
        thisUI.OnHover.SetActive(false);
        thisUI.selectedView.SetActive(false);
        GOPanel.SetActive(true);
        ViewPanel.SetActive(false);
        HelpPanel.SetActive(false);


    }

    public void Help()
    {
        thisUI.OnHover.SetActive(false);
        thisUI.selectedView.SetActive(false);

        ViewPanel.SetActive(false);
        HelpPanel.SetActive(true);

    }

    public void Back()
    {
        ViewPanel.SetActive(true);
        HelpPanel.SetActive(false);

    }


    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    void SpawnEnemies(int num)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 100f;

        for (int i = 0; i < num; i++)
        {
            GameObject g = (GameObject)Instantiate(NriabTribe, new Vector3(spawnSpot.position.x + randomDirection.x, 0.5f, spawnSpot.position.z + randomDirection.z), Quaternion.identity);
            Person p = g.GetComponent<Person>();
            p.tribe = "Nairb";
            p.Description = "A person from the Nriab tribe";
        }
    }

    void UpdateControls()
    {
        if (!warTime)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && Time.timeScale <= 90f)
            {
                Time.timeScale += 5f;
                thisUI.yearSpeed.text = "(X" + Time.timeScale.ToString() + ")";

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && Time.timeScale >= 5f)
            {
                Time.timeScale -= 5f;
                thisUI.yearSpeed.text = "(X" + Time.timeScale.ToString() + ")";
            }
        }
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

        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (ISelectable s in thisUI.selected)
            {
                Pickup p = s.IGameObject.GetComponent<Pickup>();
                if (p != null)
                {

                }

                Person per = s.IGameObject.GetComponent<Person>();
                if (per != null)
                {

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
                Person person = hit.collider.gameObject.GetComponent<Person>();

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
                else if (person != null)
                {
                    if (person.tribe != "Arcadio")
                    {
                        foreach (ISelectable s in thisUI.selected)
                        {
                            Person p = s.IGameObject.GetComponent<Person>();
                            if (p != null)
                            {
                                p.aggrovated = person;
                                thisUI.UpdateSelectedView(s);
                            }
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

    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler(Mathf.Lerp(15, 165, currentTimeOfDay), 170, 0);

        float intensityMultiplier = 1;
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        {
            intensityMultiplier = 0;
        }
        else if (currentTimeOfDay <= 0.25f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }

        sun.intensity = (sunInitialIntensity * intensityMultiplier) + minimumSunLight;
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
