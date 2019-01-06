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

        if (weapon_choice)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireHeavy();
            }
            weapon_choise_text.text = "Heavy";
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                FireGun();
            }
            weapon_choise_text.text = "Machine Gun";
        }
    }

    void FireHeavy()
    {
        GameObject temp_rocket;
        temp_rocket = Instantiate(heavy_weapon, transform.position + body.transform.forward * 2.0f, body.transform.rotation, null);
        temp_rocket.SendMessage("Activate", rb.velocity);
    }

    void FireGun()
    {
        foreach(GameObject g in guns)
        {
            g.SendMessage("Activate");
        }
    }

    // Update is called once per frame
    void Update()
    {
        UserInput();
    }
}
