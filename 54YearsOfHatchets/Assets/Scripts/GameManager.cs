using UnityEngine;
using System.Collections;
using UnityStandardAssets.Cameras;

public class GameManager : MonoBehaviour
{
    public static GameManager thisGameManager;

    public AutoCam camController;
    public Camera cam;
    bool freeMove = true;
    public GameObject collection;


    UIManager thisUI;


    void Awake()
    {
        thisGameManager = this;
    }

    // Use this for initialization
    void Start()
    {
        thisUI = UIManager.thisUI;
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

        if (!freeMove) ResetCamera();

        if (freeMove)
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
                pos = hit.point;
            else
                return;

         
            foreach (ISelectable s in thisUI.selected)
            {
                Person p = s.IGameObject.GetComponent<Person>();
                if (p != null)
                {
                    p.MoveToTarget(pos);
                    thisUI.UpdateSelectedView(s);
                }

            }
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
