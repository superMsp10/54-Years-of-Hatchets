using UnityEngine;
using System.Collections;
using System;

public class Person : MonoBehaviour, ISelectable
{

    protected string _description = "";
    protected string _tooltip = "Left Click and Drag to Select";
    public string Job = "";
    bool _selected = false;

    public Vector3 target; // target to aim for
    public NavMeshAgent agent; // the navmesh agent required for the path finding

    public Shader normal, highlighted;
    public Renderer r;

    public float maxWanderDistance = 10f;

    public Pickup pickedUP;
    public float PickupDistance = 2f;
    public LayerMask pickUps;

    public string tribe;
    public int health = 20;
    public int damage = 1;
    public float damageTime;
    protected float lastDamage;

    public int targetted = 0;
    public Person aggrovated;
    public Color Attacking;



    // Update is called once per frame
    void Update()
    {
        if (aggrovated != null)
        {
            Attack();
        }
        else
        {
            if (target != null) agent.SetDestination(target);
            this.r.material.color = Color.white;
            if (Job == "Moving")
            {
                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (agent.velocity.sqrMagnitude == 0f)
                        {
                            Job = "";
                            if (Selected)
                                UIManager.thisUI.UpdateSelectedView(this);
                        }
                    }
                }

            }

            if (Job == "" && UnityEngine.Random.Range(0, 240) == 1)
            {
                UnityEngine.Random r = new UnityEngine.Random();
                Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * maxWanderDistance;
                target = transform.position + randomDirection;
            }

        }

    }

    protected void Attack()
    {

        agent.SetDestination(aggrovated.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            r.material.color = Attacking;
            if (Time.time - lastDamage > damageTime)
            {
                if (aggrovated.TakeDamage(damage, this))
                    r.material.color = Color.white;

                lastDamage = Time.time;


            }
        }
        else
        {
            r.material.color = Color.white;
        }

        Job = "Attack";
    }

    public bool TakeDamage(int dmg, Person attacker)
    {
        aggrovated = attacker;
        health -= dmg;
        if (Selected)
            UIManager.thisUI.UpdateSelectedView(this);

        if (health <= 0)
        {
            if (Selected)
                UIManager.thisUI.RemoveSelected(this);
            if (tribe == "Arcadio")
            {
                GameManager.thisGameManager.people.Remove(this);
            }
            Destroy(this.gameObject);
            return true;
        }
        return false;
    }

    public void PickupItem(string pickupType)
    {
        if (pickedUP == null)
        {
            Job = "Harvesting";
            foreach (Collider c in Physics.OverlapSphere(transform.position, PickupDistance, pickUps))
            {
                if (c.GetComponent(pickupType) != null)
                {
                    Pickup s = c.GetComponent<Pickup>();
                    if (s != null)
                    {
                        if (!s.pickedUP)
                        {
                            s.transform.parent = transform;
                            s.transform.localPosition = new Vector3(0, 1f, 0);
                            pickedUP = s;
                            s.pickedUP = true;
                            Job = "Harvesting";

                            return;
                        }

                    }
                }
            }
        }
    }

    public void DropItem()
    {
        if (pickedUP != null)
        {
            pickedUP.transform.parent = null;
            pickedUP.pickedUP = false;
            pickedUP = null;
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
        UIManager.thisUI.ShowHoverOver(Name, Description, Tooltip, health.ToString());

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
