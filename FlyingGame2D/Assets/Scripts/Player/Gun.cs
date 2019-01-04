using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fire_speed;
    public float damage;
    public float range;

    LineRenderer line;

    float timer;

    bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Activate()
    {
        active = true;
    }

    void Fire()
    {
        timer += Time.deltaTime;

        line.SetPosition(0, transform.position);

        if (timer > fire_speed)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward * range, out hit))
            {
                line.SetPosition(1, hit.point);
            }
            else
            {
                line.SetPosition(1, transform.position + transform.forward * range);
            }

            timer = 0.0f;
        }
        else
        {
            line.SetPosition(1, transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            Fire();
        }
        else
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position);
        }

        active = false;
    }
}
