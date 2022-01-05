using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour {
    private bool broken = false;
	public bool Respawns = true;
	public float respawnTimer = 200;
	private float respawnTimerReset;
    public float breakTimer = 5;
    private float breakCountdown;
	Animator anim;
    
	void Start (){
		anim = GetComponent<Animator>();
		respawnTimerReset = respawnTimer;
        breakCountdown = breakTimer;
	}


    // Use this for initialization
    private void Update()
    {
		if (broken == true) {
            breakCountdown -= Time.deltaTime;
            if (breakCountdown <= 0)
            {
                //break animation
				SetAllCollisionsStatus (false);
                anim.SetBool("break", true);
				respawnTimer-= Time.deltaTime;
            }
          
		}

		if (respawnTimer < 0) {
			if (Respawns == true) {
				broken = false;
                breakCountdown = breakTimer;
				respawnTimer = respawnTimerReset;
				anim.SetBool ("break", false);
				SetAllCollisionsStatus (true);
			} else {
				Destroy(gameObject);
			}
		}
			
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("platformCollision");
		if (collision.CompareTag("Cat") || collision.CompareTag("Fish") || collision.CompareTag("Bird"))
        {
			//Debug.Log("PlatformBomber");
			if (!broken)
				anim.SetTrigger ("start");
            broken = true;
        }

      
    }

    
	private void SetAllCollisionsStatus (bool active) 
	{
		foreach (Collider2D c in GetComponents<Collider2D>()) 
		{
			c.enabled = active;
		}
	}
    
}
