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

    bool weapon_choice;

    public GameObject test;

    bool forward_pressed = false;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        weapon_choice = false;

        trail = body.GetComponent<ParticleSystem>();
        trail_em = trail.emission;
        trail_em.rateOverTime = 0.0f;
        direction = Vector3.zero;
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

        transform.Translate(direction.normalized * speed * Time.deltaTime);
        rb.velocity = direction.normalized * speed;
    }

    void UserInput()
    {
        //Vector3 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //mouse -= new Vector3(0.5f, 0.5f, 0.0f);

        //mouse.z = mouse.y;
        //mouse.y = 0.0f;

        //mouse = mouse.normalized;

        Vector3 mouse = Input.mousePosition;
        mouse.z = 20.0f;
        mouse = Camera.main.ScreenToWorldPoint(mouse);

        //mouse = mouse.normalized;

        body.transform.LookAt(mouse);

        trail_em.rateOverTime = 0.0f;

        if(Input.GetKeyUp(KeyCode.W))
        {
            direction = rb.velocity.normalized;
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
        StartCoroutine(FireHeavy());
        }
      
        if (Input.GetMouseButtonDown(0))
        {
            FireGun(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            FireGun(false);
        }

    }

    IEnumerator FireHeavy()
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

            temp_rocket.GetComponent<Rocket>().Activate(rb.velocity, test);

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

       
    }
}
