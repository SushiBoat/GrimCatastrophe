using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {
//    SwapPlayer swapperRef;
//    CatPlayerController catRef;
//    FishPlayerController fishRef;
//    BirdPlayerController birdRef;
	Animator anim;

    //TODO:
    [Header("Type of Obstacle")]
    public bool electricPlatform;
    public bool laser;
	public float startDelayAmount; // initial delay before it starts jumping, in order to offset the fish timing with others
	bool startDelay; // flag so that the startDelay is only added once


    //TODO:make a choice for object dissappearing after collission with player or if it stays, let designer choose.
    bool stayAliveOnCollision;
    bool destroyOnCollision;


    [Header("Electric Platform Stats")]
    public float timeActive;
	public float timeOff;
    float eCooldown;
    bool currentFlowing;

    [Header("Laser Platform Stats")]
    public float laserBeamCooldownTime;
    float lCooldown;
    bool laserFlowing;


    //TODO: Make initial obstacle variables

    // Use this for initialization
    void Start () {
//        swapperRef = GameObject.FindGameObjectWithTag("Swapper").GetComponent<SwapPlayer>();
//        catRef = GameObject.FindGameObjectWithTag("Cat").GetComponent<CatPlayerController>();
		anim = GetComponent<Animator>();
		eCooldown = timeOff;
        currentFlowing = false;
        lCooldown = laserBeamCooldownTime;
        laserFlowing = false;
		startDelay = true;
	}
	
	// Update is called once per frame
	void Update () {
        //TODO: Logic of each obstacle

        //Electric Platform logic
        if (electricPlatform)
        {
			if (startDelay) 
			{
				eCooldown = Time.fixedTime + timeOff + startDelayAmount;
				startDelay = false;
			}
            //eCooldown -= Time.deltaTime;

			if (Time.fixedTime > eCooldown) {
				if (!currentFlowing) 
				{
					eCooldown = Time.fixedTime + timeActive;
				} else if (currentFlowing) 
				{
					eCooldown = Time.fixedTime + timeOff;
				}
				swapCurrent ();
			}
        }

        //Laser beams logic
        if (laser)
        {
            lCooldown -= Time.deltaTime;
            if (lCooldown <= 0.00001f)
            {
                swapLaser();
                lCooldown = laserBeamCooldownTime;
            }
        }

    }


    void swapCurrent()
    {
        currentFlowing = !currentFlowing;
		anim.SetBool ("platformOn", currentFlowing);
    }

    public bool getCurrent()
    {
        return currentFlowing;
    }

    void swapLaser()
    {
        laserFlowing = !laserFlowing;
    }

    public bool getLaserStatus()
    {
        return laserFlowing;
    }
}
