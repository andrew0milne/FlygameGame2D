using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;
    public GameObject body;
    public GameObject[] guns;

    public GameObject rocket;

    public Text weapon_choise_text;

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
