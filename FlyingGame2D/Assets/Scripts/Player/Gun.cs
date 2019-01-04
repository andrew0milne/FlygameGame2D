using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fire_speed;
    public float damage;
    public float range;

    public Transform fire_pos;

    LineRenderer line;
    AudioSource sound;

    float timer;

    bool active = false;

   
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        sound = GetComponent<AudioSource>();
    }

    void Activate()
    {
        active = true;
        
    }
    // TO DO:
    // MAKE GUN LOOK AND SHOOT AT MOUSE POS
    void Fire()
    {
        line.enabled = true;
        timer += Time.deltaTime;

        line.SetPosition(0, fire_pos.position);

        if (timer > fire_speed)
        {
            sound.Play();

            Vector3 mouse = Input.mousePosition;
            mouse.z = 20.0f;
            mouse = Camera.main.ScreenToWorldPoint(mouse);

           // Debug.Log(mouse);

            RaycastHit hit;
            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position - new Vector3(0.0f, 20.0f, 0.0f);
            //Debug.DrawRay(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position - new Vector3(0.0f, 20.0f, 0.0f));
            if (Physics.Raycast(fire_pos.position, dir, out hit))
            {
                line.SetPosition(1, hit.point);
            }
            else
            {
                line.SetPosition(1, mouse);            
            }

            timer = 0.0f;
        }
        else
        {
            line.SetPosition(1, fire_pos.position);
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
            line.enabled = false;
        }

        active = false;
    }
}
