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
    public float home_in_dist;
    float life_time;
    public Vector3 direction;

    public bool homing;

    Rigidbody rb;
    ParticleSystem ps;

    public GameObject target;

    bool dead;

    public List<Transform> obstacles;

    float speed = 10.0f;
    float acc = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Activate(Vector3 vel, GameObject tar, bool home)
    {
        life_time = 0.0f;
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        
        dead = false;
        rb.velocity = vel;
        target = tar;
        Physics.IgnoreLayerCollision(9, 9);
        Physics.IgnoreLayerCollision(9, 10);
        obstacles = new List<Transform>();
        homing = home;
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

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Obstacle")
        {
            obstacles.Add(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            obstacles.Remove(other.transform);
        }
    }

    // Jancky move that uses mainly physics
    void OldMove()
    {
        if (life_time < time_until_targetting)
        {
            if (rb.velocity.magnitude < max_speed)
            {
                rb.AddForce(transform.forward * acceleration / 4.0f);
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

            if (dist < home_in_dist)
            {
                //rb.AddForce((home_in_dist - dist) / home_in_dist * -1.0f * rb.velocity);
                rotate_speed = 500.0f;
            }

            float pos2 = Vector3.Angle(transform.forward, rb.velocity.normalized);

            if (pos2 > 90.0f)
            {
                pos2 = 90.0f;
            }

            if (pos > 0.0f)
            {
                transform.Rotate(transform.up, rotate_speed * Time.deltaTime);
                rb.AddForce(transform.right * (pos2 / 90.0f) * -acceleration);
            }
            else if(pos < 0.0f)
            {
                transform.Rotate(transform.up, -rotate_speed * Time.deltaTime);
                rb.AddForce(transform.right * (pos2 / 90.0f) * acceleration );
            }

            foreach(Transform tr in obstacles)
            {
                float num = 90.0f - Vector3.Angle(transform.right, tr.position - transform.position);

                Vector3 offset = transform.position - tr.position;
                offset = offset.normalized;
                rb.AddForce(offset * acceleration / 2.0f);
                if (num > 0.0f)
                {
                    transform.Rotate(transform.up, -rotate_speed * Time.deltaTime);
                    
                }
                else 
                {
                    transform.Rotate(transform.up, rotate_speed * Time.deltaTime);
                }
            }
        }
    }


    void TakeDamage(float damage)
    {
        Dead();
    }

    // Much better movement
    void HomingMove()
    {
        if (rb.velocity.magnitude < max_speed)
        {
            rb.AddForce(transform.forward * 2.0f);
        }

        if (life_time < time_until_targetting)
        {
            transform.Translate(Vector3.forward * speed / 2.0f * Time.deltaTime);
        }
        else
        {
            if (ps.isPlaying == false)
            {
                ps.Play();
            }
            Vector3 direction = target.transform.position - transform.position + Random.insideUnitSphere;
            direction.Normalize();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 180.0f * Time.deltaTime);

            if (obstacles.Count > 0)
            {
                float nearest_obstacle = 10000000.0f;

                RaycastHit hit;

                Vector3 obs = Vector3.zero;

                foreach (Transform tr in obstacles)
                {
                    Vector3 dir = tr.position - transform.position;
                    if (Physics.Raycast(transform.position, dir.normalized, out hit))
                    {
                        float d = Vector3.Distance(transform.position, hit.point);
                        if (d < nearest_obstacle)
                        {
                            nearest_obstacle = d;
                            obs = tr.position;
                        }
                    }
                }
                

                float pos = 90.0f - Vector3.Angle(transform.right, obs - transform.position);

                float dist = Vector3.Distance(obs, transform.position);

                dist = (18.0f - dist) / 18.0f;

                dist = 1.0f;
                

                if (pos > 0.0f)
                {
                    transform.Rotate(transform.up, -rotate_speed * Time.deltaTime * dist);

                }
                else if (pos < 0.0f)
                {
                    transform.Rotate(transform.up, rotate_speed * Time.deltaTime * dist);

                }

                
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void Move()
    {
        if (ps.isPlaying == false)
        {
            ps.Play();
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (homing)
            {
                HomingMove();
            }
            else
            {
                Move();
            }

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
