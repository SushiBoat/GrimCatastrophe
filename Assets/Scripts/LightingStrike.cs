using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingStrike : EnvironmentInteraction {
  
   
    public int lightningDuration;
    public int lightningDelay;
    private int lightningCountdown;
    public GameObject lightning;
    private GameObject lightningStrike;

    private bool active = false;
    // Use this for initialization
    void Start () {
        lightningCountdown = lightningDuration;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active == true)
        {
            lightningCountdown--;
            if (lightningCountdown + lightningDelay == lightningDuration)
            {
                lightningStrike = Instantiate(lightning, new Vector3(transform.position.x,transform.position.y - 22.0f,transform.position.z), transform.rotation);
                //set lightning lifeSpan to lightning countdown;
            }
            if (lightningCountdown <= 0)
            {
                Destroy(lightningStrike);
                lightningCountdown = lightningDuration;
                active = false;
            }
        }
    }

    public override void Activate()
    {
        active = true;
    }
}

