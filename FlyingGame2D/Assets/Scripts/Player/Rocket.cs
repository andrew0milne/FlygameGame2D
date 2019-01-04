using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float acceleration;
    public float max_speed;
    public Vector3 direction;

    Rigidbody rb;
    ParticleSystem ps;

    bool dead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Activate(Vector3 vel)
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        ps.Play();
        dead = false;
        rb.velocity = vel;
    }

    void OnCollisionEnter(Collision collision)
    {
        //Destroy(this.gameObject);
        dead = true;
        ParticleSystem.EmissionModule em;
        em = ps.emission;
        em.rateOverTime = 0.0f;

        rb.isKinematic = true;
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            //if (rb.velocity.magnitude < max_speed)
            //{
                rb.AddForce(transform.forward * acceleration);
            //}
        }
        else
        {
            if(ps.particleCount <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
