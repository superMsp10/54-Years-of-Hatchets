using UnityEngine;
using System.Collections;

public class Nairb : Person
{

    void Start()
    {
        _description = "A warriors form the Nairb tribe coming to attack you";
        _tooltip = "Right click with people selected to attack";
        Job = "Attack";
        lastDamage = Time.time;
    }

    void Update()
    {
        if (aggrovated == null)
        {
            FindTarget();
        }
        else
        {
            Attack();
        }
    }


    void FindTarget()
    {
        foreach (Person p in FindObjectsOfType<Person>())
        {
            if (p.targetted < 7 && p.tribe != this.tribe)
            {
                aggrovated = p;
                p.targetted++;
                return;
            }
        }

    }


}
