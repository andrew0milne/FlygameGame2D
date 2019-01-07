using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fire_speed;
    public float beam_active_time;
    public float damage;
    public float range;

    public float line_width;

    public Transform fire_pos;

    LineRenderer line;
    AudioSource sound;

    float timer;

    bool active = false;

   
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = line_width;
        line.endWidth = line_width;
        sound = GetComponent<AudioSource>();
    }

    void Activate(bool b)
    {
        active = b;    
    }

    IEnumerator Fire()
    {

        Vector3 mouse = Input.mousePosition;
        mouse.z = 20.0f;
        mouse = Camera.main.ScreenToWorldPoint(mouse);

        RaycastHit hit;
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position - new Vector3(0.0f, 20.0f, 0.0f);
        
        Vector3 end_point;

        if (Physics.Raycast(fire_pos.position, dir, out hit, range))
        {
            end_point = hit.point;
        }
        else
        {
            end_point = mouse - fire_pos.transform.position;
            end_point = end_point.normalized * range;

            end_point = fire_pos.transform.position + end_point;
        }


        float t = 0.0f;

        while (t < beam_active_time)
        {
            line.SetPosition(1, end_point);

            line.startWidth = Mathf.Lerp(line_width, 0.0f, t / beam_active_time);
            line.endWidth = Mathf.Lerp(line_width, 0.0f, t / beam_active_time);

            t += Time.deltaTime;

            yield return null;
        }

        line.startWidth = line_width;
        line.endWidth = line_width;
        line.SetPosition(1, fire_pos.position);

        //active = false;

        yield return null;
    }

    void FireMain()
    {
        line.enabled = true;
        timer += Time.deltaTime;

        line.SetPosition(0, fire_pos.position);

        if (timer > fire_speed)
        {
            sound.Play();
            StartCoroutine(Fire());
            timer = 0.0f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            FireMain();
        }
        else
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position);
            line.enabled = false;
        }

        
    }
}
