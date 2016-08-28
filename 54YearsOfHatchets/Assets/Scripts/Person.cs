using UnityEngine;
using System.Collections;
using System;

public class Person : MonoBehaviour, ISelectable
{

    string _description = "A person from the Arcadio tribe";
    string _tooltip = "Left Click to Select";
    public string Job = "";
    bool _selected = false;

    public Vector3 target; // target to aim for
    public NavMeshAgent agent; // the navmesh agent required for the path finding

    public Shader normal, highlighted;
    public Renderer r;

    public float maxWanderDistance = 10f;
    bool moving = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moving = true;
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    if (Job == "Moving")
                        Job = "";
                    if (Selected)
                        UIManager.thisUI.UpdateSelectedView(this);
                    moving = false;
                }
            }
        }

        if (target != null)
        {
            if (Job == "Moving")
                agent.SetDestination(target);
            else if (Job == "" && !moving)
            {
                Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * maxWanderDistance;
                target = randomDirection;
                agent.SetDestination(target);

            }
        }



    }



    public void MoveToTarget(Vector3 pos)
    {
        Job = "Moving";
        target = pos;
    }

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

    void OnMouseOver()
    {

        //if (Input.GetMouseButtonDown(1))
        //{
        //    if (!Selected)
        //        UIManager.thisUI.AddSelected(this);
        //    else
        //        UIManager.thisUI.RemoveSelected(this);
        //}
    }


}
