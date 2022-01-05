using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {
    public GameObject target;
    public float leverCooldown = 5;
    private float cooldown;
    private bool leverLock;
    Animator myAnim;

    /*
    Adding references to leverGroup will change the behaviour of levers
    - Only 1 lever in the group may be activated at any one time
    - If a lever is activated it cannot be activated again until another lever from the group has been activated
    */
    public Lever[] leverGroup;

	// Use this for initialization
	void Start () {
        cooldown = 0;
		myAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0.5f && leverLock == false)
            {
                myAnim.SetBool("active", false);
            }
        }
        

	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("collision");
		if((collision.gameObject.CompareTag("Cat") || collision.gameObject.CompareTag("Bird") || collision.gameObject.CompareTag("Fish")) && cooldown <= 0 && leverLock == false )
        {
            leverToggle();
            if(leverGroup.Length > 0)
            {
                //This prevents repeated activation of the same lever
                leverLock = true;
                for (int i = 0;i< leverGroup.Length; i++)
                {
                    leverGroup[i].leverToggle();
                }
            }
            // Debug.Log("PlayerCollision");
            target.GetComponent<EnvironmentInteraction>().Activate();
			
        }
    }

	void OnTriggerStay2D(Collider2D collision)
	{
		OnTriggerEnter2D (collision);
	}


    public bool checkState()
    {
        return myAnim.GetBool("active");
    }

    private void leverToggle()
    {
        leverLock = false;
        cooldown = leverCooldown;
        myAnim.SetBool("active", true);
    }
}
