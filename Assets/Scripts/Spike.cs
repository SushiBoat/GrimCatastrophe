using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Old Class/Not in use
public class Spike : EnvironmentInteraction {
    public float speed;
    private float initialHeight;
    public float maxHeight;
    private bool raised = false;
    private int movementDirection;
    public int activeDuration;
    private int activeCountdown;
	// Use this for initialization
	void Start () {
        initialHeight = transform.position.y;
        activeCountdown = activeDuration;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (movementDirection == 1)
        {
            transform.Translate(new Vector3(0, speed));
            if(transform.position.y >= maxHeight)
            {
                movementDirection = 0;
                raised = true;
            }
        }

        if(movementDirection == 2)
        {
            transform.Translate(new Vector3(0, -speed));
            if(transform.position.y <= initialHeight)
            {
                movementDirection = 0;
                raised = false;
            }
        }

        if (raised && movementDirection != 2)
        {
            activeCountdown--;
            if(activeCountdown <= 0)
            {
                movementDirection = 2;
                activeCountdown = activeDuration;
            }
        }
       
	}

    public override void Activate()
    {

        if(raised == false)
        {
            movementDirection = 1;
        }
    }
}
