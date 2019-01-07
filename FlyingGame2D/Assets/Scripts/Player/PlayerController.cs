using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float acceleration = 5.0f;
    public float max_speed = 3.0f;
    public GameObject body;
    public GameObject[] guns;

    public GameObject heavy_weapon;

    public Text weapon_choise_text;

    ParticleSystem trail;
    ParticleSystem.EmissionModule trail_em;

    bool weapon_choice;

    public GameObject test;

   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        weapon_choice = false;

        trail = body.GetComponent<ParticleSystem>();
        trail_em = trail.emission;
        trail_em.rateOverTime = 0.0f;
    }

    void Move()
    {

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
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(body.transform.forward * acceleration);
            trail_em.rateOverTime = 100.0f;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            weapon_choice = !weapon_choice;

        }

        //if (weapon_choice)
        //{
            if (Input.GetKeyDown(KeyCode.Space))
            {
            StartCoroutine(FireHeavy());
            }
            //weapon_choise_text.text = "Heavy";
        //}
        //else
        //{
            if (Input.GetMouseButtonDown(0))
            {
                FireGun(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                FireGun(false);
            }

            //weapon_choise_text.text = "Machine Gun";
        //}
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

        float pos = 90.0f - Vector3.Angle(transform.right, test.transform.position - transform.position);

        //Debug.Log(pos);
        
        // if object is behind or infornt
        //float pos2 = 90.0f - Vector3.Angle(transform.forward, test.transform.position - transform.position);

       
    }
}
