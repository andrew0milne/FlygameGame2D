using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float acceleration;
    public float max_speed;
    public float max_life_time;
    public float time_until_targetting;
    public float rotate_speed;
    float life_time;
    public Vector3 direction;

    Rigidbody rb;
    ParticleSystem ps;

    public GameObject target;

    bool dead;

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Activate(Vector3 vel, GameObject tar)
    {
        life_time = 0.0f;
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        ps.Play();
        dead = false;
        rb.velocity = vel;
        target = tar;
        Physics.IgnoreLayerCollision(9, 9);
    }

    void Dead()
    {
        dead = true;
        ParticleSystem.EmissionModule em;
        em = ps.emission;
        em.rateOverTime = 0.0f;

        rb.isKinematic = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        Dead();       
    }

    void Move()
    {
        if (life_time < time_until_targetting)
        {
            if (rb.velocity.magnitude < max_speed)
            {
                rb.AddForce(transform.forward * acceleration);
            }
        }
        else
        {
            if (rb.velocity.magnitude < max_speed)
            {
                rb.AddForce(transform.forward * acceleration);
            }

            // DOESNT HIT THE TARGET PROPERLY, ORBITS AROUND IT A BIT

            float angle = Vector3.Angle(rb.velocity, transform.forward);

            float pos = 90.0f - Vector3.Angle(transform.right, target.transform.position - transform.position);

            float dist = Vector3.Distance(target.transform.position, transform.position);

            if (dist < 10.0f)
            {
                rb.AddForce((10.0f - dist) / 10.0f * -1.0f * rb.velocity);
                
            }


            if (pos > 0.0f)
            {
                transform.Rotate(transform.up, rotate_speed * Time.deltaTime);
            }
            else if(pos < 0.0f)
            {
                transform.Rotate(transform.up, -rotate_speed * Time.deltaTime);
            }    
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            Move();

            life_time += Time.deltaTime;

            if (life_time > max_life_time)
            {
                Dead();
            }
        }
        else
        {
            if(ps.particleCount <= 0)
            {
                Destroy(this.gameObject);
            }
        }

       // Debug.Log(rb.velocity.magnitude);
    }
}
