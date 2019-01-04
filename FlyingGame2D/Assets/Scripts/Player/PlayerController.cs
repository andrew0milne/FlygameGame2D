using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;
    public GameObject body;
    public GameObject rocket;

    bool weapon_choice;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        weapon_choice = false;
    }

    void UserInput()
    {
        Vector3 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouse -= new Vector3(0.5f, 0.5f, 0.0f);

        mouse.z = mouse.y;
        mouse.y = 0.0f;

        body.transform.LookAt(transform.position + mouse);

        if (Input.GetKey(KeyCode.W))
        {
            cc.Move(mouse);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            weapon_choice = !weapon_choice;
        }

        if (weapon_choice)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireRocket();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                FireGun();
            }
        }
    }

    void FireRocket()
    {
        
    }

    void FireGun()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UserInput();
    }
}
