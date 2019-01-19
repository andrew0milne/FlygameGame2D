using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 5.0f;
    public float acceleration = 2.0f;
    public float max_speed = 5.0f;
    public GameObject body;
    public GameObject[] guns;

    public GameObject heavy_weapon;

    public Text weapon_choise_text;

    ParticleSystem trail;
    ParticleSystem.EmissionModule trail_em;

    LineRenderer laser_target;
    bool laser_target_found = false;

    bool weapon_choice;

    public GameObject test;

    Vector3 previous_position;

    bool forward_pressed = false;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        weapon_choice = false;

        laser_target = GetComponent<LineRenderer>();

        trail = body.GetComponent<ParticleSystem>();
        trail_em = trail.emission;
        trail_em.rateOverTime = 0.0f;
        direction = Vector3.zero;
       
        previous_position = transform.position;
    }

    void Move()
    {
        if (forward_pressed)
        {
            if (speed < max_speed)
            {
                speed += acceleration * Time.deltaTime;
            }

            trail_em.rateOverTime = 100.0f;

            if (rb.velocity.magnitude < max_speed) ;
            {
                rb.AddForce(direction * speed);
            }
        }
        else
        {
            if (speed >= 0.0f)
            {
                speed -= acceleration * Time.deltaTime;
            }
            else if (speed < 0.0f)
            {
                speed = 0.0f;
            }
            
        }
        //rb.velocity = direction.normalized * speed;
        
        transform.Translate(direction.normalized * speed * Time.deltaTime);    
    }

    void UserInput()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 20.0f;
        mouse = Camera.main.ScreenToWorldPoint(mouse);

        body.transform.LookAt(mouse);

        trail_em.rateOverTime = 0.0f;

        if(Input.GetKeyUp(KeyCode.W))
        {
            direction = transform.position - previous_position;//rb.velocity.normalized;
            direction = direction.normalized;
            forward_pressed = false;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = body.transform.forward;

            forward_pressed = true;
        }

        

        if(Input.GetKeyDown(KeyCode.A))
        {
            rb.AddForce(body.transform.right * -500.0f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.AddForce(body.transform.right * 500.0f);
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            weapon_choice = !weapon_choice;

        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FireHeavy(test));
        }
      
        if (Input.GetMouseButtonDown(0))
        {
            FireGun(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            FireGun(false);
        }

        laser_target.SetPosition(0, transform.position);
        laser_target.SetPosition(1, transform.position);

        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            int layer_mask = ~(1 << LayerMask.NameToLayer("Rocket"));

            if(Physics.Raycast(transform.position, body.transform.forward, out hit, 100.0f, layer_mask))
            {
                laser_target.SetPosition(0, hit.point);
                laser_target_found = true;
            }
            else
            {
                laser_target_found = false;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (laser_target_found)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, body.transform.forward, out hit))
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        StartCoroutine(FireHeavy(hit.transform.gameObject));
                    }
                }
            }
        }

    }

    IEnumerator FireHeavy(GameObject target)
    {
        int number_of_rockets = 6;

        for (int i = 0; i < number_of_rockets; i++)
        {
            GameObject temp_rocket;
            Vector3 pos;

            if (i % 2 == 0)
            {
                temp_rocket = Instantiate(heavy_weapon, transform.position, body.transform.rotation, null);
                temp_rocket.transform.Rotate(new Vector3(0.0f, 90.0f));
            }
            else
            {
                temp_rocket = Instantiate(heavy_weapon, transform.position, body.transform.rotation, null);
                temp_rocket.transform.Rotate(new Vector3(0.0f, -90.0f));
            }

            temp_rocket.GetComponent<Rocket>().Activate(rb.velocity, target, true);

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    void FireGun(bool active)
    {
        foreach(GameObject g in guns)
        {
            g.SendMessage("Activate", active);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UserInput();
        Move();

        //float pos = 90.0f - Vector3.Angle(transform.right, test.transform.position - transform.position);

        //Debug.Log(pos);

        // if object is behind or infornt
        //float pos2 = 90.0f - Vector3.Angle(transform.forward, test.transform.position - transform.position);

        previous_position = transform.position;
    }
}
