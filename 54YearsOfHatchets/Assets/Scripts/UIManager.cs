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

    public Camera cam;

    public List<_Selectable> selected = new List<_Selectable>();

    Texture2D t;
    GUIStyle s;
    public Color selection;

    Vector2 start, end;
    Vector2 worldStart, worldEnd;
    bool dragging = false;
    public LayerMask selectableLayers;

    public GameObject selectedView;
    public GameObject content;
    public GameObject selectedPrefab;




    void Awake()
    {
        thisUI = this;
    }

    // Use this for initialization
    void Start()
    {
        OnHover.SetActive(false);
        t = new Texture2D(1, 1);
        s = new GUIStyle();
        t.wrapMode = TextureWrapMode.Repeat;
        t.SetPixel(0, 0, selection);
        t.Apply();
        s.normal.background = t;

    }

    void OnGUI()
    {


        if (Input.GetMouseButtonDown(0))
        {
            start = Event.current.mousePosition;
            worldStart = Input.mousePosition;

            dragging = true;
        }


        if (dragging)
        {
            end = Event.current.mousePosition;

            float height = end.y - start.y;
            float width = end.x - start.x;

            GUI.Label(new Rect(start.x, start.y, width, height), "", s);

            worldEnd = Input.mousePosition;

        }

        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(worldStart);
            Vector3 cubeStart = Vector3.zero, cubeEnd = Vector3.zero, center = Vector3.zero;
            if (Physics.Raycast(ray, out hit))
                cubeStart = hit.point;
            else
                return;

            ray = cam.ScreenPointToRay(worldEnd);
            if (Physics.Raycast(ray, out hit))
                cubeEnd = hit.point;
            else
                return;

            center = Vector3.Lerp(cubeStart, cubeEnd, 0.5f);

            foreach (Collider c in Physics.OverlapSphere(center, Vector3.Distance(center, cubeStart), selectableLayers))
            {
                _Selectable s = c.GetComponent<_Selectable>();
                if (s != null)
                {
                    if (!s.Selected)
                        AddSelected(s);
                    else
                        RemoveSelected(s);
                }
            }
        }

    }

    public void AddSelected(_Selectable s)
    {

        if (selected.Count <= 0)
            selectedView.SetActive(true);

        selected.Add(s);

        s.OnSelected();

        GameObject g = (GameObject)Instantiate(selectedPrefab);
        g.name = s.Name;
        g.transform.parent = content.transform;
        g.transform.FindChild("Name").GetComponent<Text>().text = s.Name;
        g.transform.FindChild("Description").GetComponent<Text>().text = s.Description;

        Person p = s.IGameObject.GetComponent<Person>();
        if (p != null)
        {
            g.transform.FindChild("Job").GetComponent<Text>().text = p.Job;
        }
        else
        {
            g.transform.FindChild("Job").GetComponent<Text>().text = s.Tooltip;
        }


    }

    public void RemoveSelected(_Selectable s)
    {
        selected.Remove(s);
        s.OnDeselected();

        Destroy(content.transform.FindChild(s.Name).gameObject);

        if (selected.Count <= 0)
            selectedView.SetActive(false);


    }

    public void ShowHoverOver(string name, string description, string toolTip)
    {
        OnHover.SetActive(true);

        Name.text = name;
        Description.text = description;
        Tooltip.text = toolTip;

    }

    public void UpdateSelectedView(_Selectable s)
    {
        Transform g = content.transform.FindChild(s.Name);
        g.FindChild("Description").GetComponent<Text>().text = s.Description;
        Person p = s.IGameObject.GetComponent<Person>();
        if (p != null)
        {
            g.transform.FindChild("Job").GetComponent<Text>().text = p.Job;
        }
        else
        {
            g.transform.FindChild("Job").GetComponent<Text>().text = s.Tooltip;
        }
    }


    public void HideHoverOver()
    {
        OnHover.SetActive(false);
    }


}
