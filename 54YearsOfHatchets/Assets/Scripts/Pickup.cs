using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour, ISelectable
{

    string _description = "Sample pickup resource";
    string _tooltip = "Left Click and Drag to Select";
    bool _selected = false;
    public Shader normal, highlighted;
    public Renderer r;
    public bool pickedUP = false;
    public string pickUpType;

    public string Description
    {
        get
        {
            return _description;
        }

        set
        {
            _description = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public string Tooltip
    {
        get
        {
            return _tooltip;
        }

        set
        {
            _tooltip = value;
        }
    }

    public Vector3 location
    {
        get
        {
            return transform.position;
        }
    }

    public GameObject IGameObject
    {
        get
        {
            return gameObject;
        }
    }

    public bool Selected
    {
        get
        {
            return _selected;
        }

        set
        {
            _selected = value;
        }
    }

    public void OnSelected()
    {
        Selected = true;
        r.material.shader = highlighted;
    }

    public void OnDeselected()
    {
        Selected = false;
        r.material.shader = normal;

    }

    void OnMouseEnter()
    {
        UIManager.thisUI.ShowHoverOver(Name, Description, Tooltip);

    }

    void OnMouseExit()
    {
        UIManager.thisUI.HideHoverOver();

    }
}
