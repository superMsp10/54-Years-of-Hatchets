using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Cameras;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager thisUI;

    public Text Name;
    public Text Description;
    public Text Tooltip;
    public GameObject OnHover;

    public AutoCam camController;

    public GameObject collection;

    public List<_Selectable> selected = new List<_Selectable>();

    bool freeMove = true;

    void Awake()
    {
        thisUI = this;
    }

    // Use this for initialization
    void Start()
    {
        OnHover.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
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

    }

    public void AddSelected(_Selectable s)
    {
        selected.Add(s);
        ResetCamera();

        s.OnSelected();
    }

    public void RemoveSelected(_Selectable s)
    {
        selected.Remove(s);
        s.OnDeselected();
        ResetCamera();

    }

    public void ShowHoverOver(string name, string description, string toolTip)
    {
        OnHover.SetActive(true);

        Name.text = name;
        Description.text = description;
        Tooltip.text = toolTip;

    }

    public void ResetCamera()
    {
        if (selected.Count > 0)
        {
            camController.enabled = true;
            freeMove = false;

            Vector3 average = Vector3.zero;
            for (int i = 0; i < selected.Count; i++)
            {
                average += selected[i].location;
            }

            collection.transform.position = average / selected.Count;
            camController.SetTarget(collection.transform);
        }
        else
        {
            camController.enabled = false;
            freeMove = true;
        }
    }

    public void HideHoverOver()
    {
        OnHover.SetActive(false);
    }


}
