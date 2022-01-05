using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCloud : EnvironmentInteraction {
    public float speed;
    private float initialX;
    public float maxX;
    private bool raised = false;
    private int movementDirection = 1;
    public int lightningDuration;
    public int lightningDelay;
    private int lightningCountdown;
    private int previousDirection;
    public GameObject lightning;
   
    // Use this for initialization
    void Start () {
        initialX = transform.position.x;
        lightningCountdown = lightningDuration;
    }

    void FixedUpdate()
    {
        if (movementDirection == 1)
        {
            transform.Translate(new Vector3(speed, 0));
            if (transform.position.x >= maxX)
            {
                movementDirection = 2;
                
            }
        }

        if (movementDirection == 2)
        {
            transform.Translate(new Vector3(-speed, 0));
            if (transform.position.x <= initialX)
            {
                movementDirection = 1;
                raised = false;
            }
        }

        if (movementDirection == 0)
        {
            lightningCountdown--;
            if(lightningCountdown + lightningDelay == lightningDuration)
            {
                GameObject lightningStrike = Instantiate(lightning, transform.position, transform.rotation);
                //set lightning lifeSpan to lightning countdown;
            }
            if(lightningCountdown <= 0)
            {
                lightningCountdown = lightningDuration;
                movementDirection = previousDirection;
            }
        }

        

    }

    public override void Activate()
    {
        previousDirection = movementDirection;
        movementDirection = 0;
        
    }
}
