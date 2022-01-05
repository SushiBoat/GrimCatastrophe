using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum FishState { Jumping, Spitting, Swimming,Hurt,Dead }
public class FishBoss : MonoBehaviour {
    
   //tracks player position
    public Transform player;
    
    //handle to spit prefab
    public GameObject spit;

    private FishState state = FishState.Swimming;

    private Vector3[] pools = new Vector3[3];
   // public Transform[] poolLocations = new Transform[3];

	public Slider healthSlider;

    private Animator anim;
    private bool cutscene = true;
    private bool pause = false;

    private int targetPool;

    private int jumpCounter = 0;

    private int phase = 1;
    public float speed = 0.15f;
    private bool submerged = true;
    public float stunTime;
    private float stunCountdown;

    public float jumpInterval;
    private float jumpCountdown;


    private float struggleRotationCooldown = 5;
    private int struggleRotationDirection = 1;

    private float denom;
    private float a;
    private float b;
    private float c;
    private float targetY;
    private float speedModifier;

    public float spitCooldown;
    public float spitAnimDuration;
    private float spitCooldownTimer;
    private float spitAnimTimer;
    private bool spitToggle = false;

   

   


	// Use this for initialization
	void Start () {
        //define pool locations
       pools[0] = GameObject.FindGameObjectWithTag("Pool1").transform.position;
        pools[0] = new Vector3(pools[0].x - 14.6f, pools[0].y - 7.3f, 0);
       pools[1] = GameObject.FindGameObjectWithTag("Pool2").transform.position;
        pools[1] = new Vector3(pools[1].x - 14.6f, pools[1].y - 7.3f, 0);
        pools[2] = GameObject.FindGameObjectWithTag("Pool3").transform.position;
        pools[2] = new Vector3(pools[2].x - 14.6f, pools[2].y - 7.3f, 0);
       
        //set boss location to second pool
        transform.position = pools[2];
      
        targetPool = 0;
        spitCooldownTimer = spitCooldown;
        spitAnimTimer = spitAnimDuration;
        jumpCountdown = 8;
        anim = gameObject.GetComponent<Animator>();
		healthSlider.value = 3;
		cutscene = true;

    }
	
	void FixedUpdate () {

        //Debug.Log(player.position.x);
        if (player && !pause)
        {
            //Only update when player is nearby
            if (player.position.x > pools[0].x - 8.0f && player.position.x < pools[2].x + 8.0f)
            {
              
                switch (state)
                {
                    //default fish state 
                    case FishState.Swimming:
                        jumpCountdown-= Time.deltaTime;
                        if (jumpCountdown <= 0)
                        {
                            jumpCountdown = jumpInterval;
                            setTarget();
                        }
                        break;
                    case FishState.Jumping:
                        incrementJump();
                        break;
                    case FishState.Spitting:
                        Spit();
                        break;
                    case FishState.Hurt:
                        Struggle();
                        break;
                    case FishState.Dead:
                        death();
                        break;



                }
            }
        }
	}

    void Spit()
    {
        resetOrientation();
        jumpCounter = 0;

        if (phase > 1)
        {
            if (spitAnimTimer <= 0)
            {
				spitAnimTimer = spitAnimDuration;
                if (transform.localScale.x < 0)
                {
                    GameObject tempSpit = Instantiate(spit, transform.position, Quaternion.Inverse(transform.rotation));
                    tempSpit.GetComponent<Projectile>().setTarget(new Vector3(player.position.x, player.position.y + 2f, player.position.z), 0.5f, 0.005f); // speed was 0.75f, gravity was 0.015
                   // tempSpit.GetComponent<Projectile>().setTarget(new Vector3(player.position.x, player.position.y + 2f, player.position.z), Vector3.Distance(transform.position,player.position)/10, 0.015f);
                }
                else
                {
                    GameObject tempSpit = Instantiate(spit, transform.position, transform.rotation);
					tempSpit.GetComponent<Projectile>().setTarget(new Vector3(player.position.x, player.position.y + 2f, player.position.z), 0.5f, 0.005f); // speed was 0.75f
                   // tempSpit.GetComponent<Projectile>().setTarget(new Vector3(player.position.x, player.position.y + 2f, player.position.z), Vector3.Distance(transform.position, player.position) / 10, 0.015f);
                }
                //Debug.Log(player.position);
                spitToggle = true;
				anim.SetTrigger ("spit");
            }
            if (spitToggle == false)
            {
                spitAnimTimer-= Time.deltaTime;
            }

            if(spitToggle == true)
            {
                spitCooldownTimer-= Time.deltaTime;
                if (spitCooldownTimer <=0)
                {
                    state = FishState.Swimming;

                    spitCooldownTimer = spitCooldown;
                    spitToggle = false;
                }
            }
        }

        if(phase <= 1)
        {
            state = FishState.Swimming;
         
        }
        

        
        

    }

    void setTarget() {


        resetOrientation();
        if (cutscene == true)
        {
            if (targetPool != 1)
            {
                targetPool = 1;
                calculateJump((pools[1].x + pools[2].x) / 2, pools[1].y + 8);
            }
            else
            {
                targetPool = 0;
                calculateJump((pools[0].x + pools[1].x) / 2, pools[0].y + 8.0f);
				jumpCountdown = 6;
				cutscene = false;
            }
        }else {

            //Determine next target pool based on previous target pool and player position
            switch (targetPool)
            {
                case 0:
                    if (transform.position.x - player.position.x < -2 && player.position.x < 68)
                    {
                        if (transform.position.x - player.position.x > -10)
                        {
                            targetPool = 1;
                            calculateJump((pools[0].x + pools[1].x) / 2, pools[0].y + 8.0f);

                        }
                        else
                        {
                            targetPool = 2;
                            calculateJump(pools[1].x, pools[1].y + 10);

                        }

                    }
                    else
                    {
                        //Debug.Log("0 to 1");
                        targetPool = 1;
                        calculateJump((pools[0].x + pools[1].x) / 2, pools[0].y + 8.0f);
                    }



                    break;

                case 1:
                    if (transform.position.x - player.position.x < -2 && player.position.x < 68)
                    {
                        targetPool = 2;
                        calculateJump((pools[1].x + pools[2].x) / 2, pools[1].y + 8);


                    }
                    else if (transform.position.x - player.position.x > 2 && player.position.x > 38)
                    {
                        targetPool = 0;
                        calculateJump((pools[0].x + pools[1].x) / 2, pools[0].y + 8.0f);

                    }
                    else
                    {
                        if (Random.Range(0.0f, 2.0f) > 1.0f)
                        {
                            // Debug.Log("1 to 2");
                            targetPool = 2;
                            calculateJump((pools[1].x + pools[2].x) / 2, pools[1].y + 8.0f);
                        }
                        else
                        {
                            // Debug.Log("1 to 0");
                            targetPool = 0;
                            calculateJump((pools[0].x + pools[1].x) / 2, pools[0].y + 8.0f);
                        }
                    }



                    break;

                case 2:
                    if (transform.position.x - player.position.x > 2 && player.position.x > 38)
                    {
                        if (transform.position.x - player.position.x < 10)
                        {
                            targetPool = 1;
                            calculateJump((pools[1].x + pools[2].x) / 2, pools[1].y + 8.0f);

                        }
                        else
                        {
                            targetPool = 0;
                            calculateJump(pools[1].x, pools[1].y + 10);

                        }

                    }
                    else
                    {
                        // Debug.Log("2 to 1");
                        targetPool = 1;
                        calculateJump((pools[1].x + pools[2].x) / 2, pools[1].y + 8);
                    }

                    break;
            }
        }



    }

    void calculateJump(float x2,float y2)
    {
       // Debug.Log(x2 + " " +  y2 + " " + targetPool);
       
        float x1 = transform.position.x;
        float y1 = transform.position.y;

        

        if (y2 < -2)
        {
            y2 = -2;
        }

        float x3 = pools[targetPool].x;
        float y3 = pools[targetPool].y;

        denom = (x1 - x2) * (x1 - x3) * (x2 - x3);
        a = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom;
        b = ((x3 * x3) * (y1 - y2) + (x2 * x2) * (y3 - y1) + (x1 * x1) * (y2 - y3)) / denom;
        c = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;

        float maxHeightX = (-b)/(2*a);
        float maxHeight = (a * maxHeightX * maxHeightX) + b * maxHeightX + c;

         if (phase == 3)
            {
                splash();
            }


            state = FishState.Jumping;
            



    }
        /*Lagrange Polynomial Interpolation
   - Calculate quadratic parabola between player, fish and target pool for each jump,


      Y = ax^2 + bx + c


      denom = (x1 - x2)(x1 - x3)(x2 - x3)
      A = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom
      B = (x3^2 * (y1 - y2) + x2^2 * (y3 - y1) + x1^2 * (y2 - y3)) / denom
      C = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom


  source:
  https://stackoverflow.com/questions/717762/how-to-calculate-the-vertex-of-a-parabola-given-three-points
  https://en.wikipedia.org/wiki/Lagrange_polynomial



*/


    

    void incrementJump()
    {
        float targetX;
        int direction = 0;

        if (pools[targetPool].x > transform.position.x)
        {
             targetX = transform.position.x + speed;

        }
        else {targetX = transform.position.x - speed;
            direction = 1;
        }
      
        // Y = ax^2 + bx + c
        targetY = (a * targetX * targetX) + (b * targetX) + c;

        Vector3 target = new Vector3(targetX, targetY);
        Quaternion rotation = Quaternion.LookRotation
       (target - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);



        transform.position = new Vector3(targetX, targetY, 0.0f);
        //y = a*jumpIncrement^2 + b* jump increment + c

        if (transform.position.y < pools[targetPool].y && submerged)
        {
            jumpCounter++;
            transform.position = new Vector2(pools[targetPool].x, pools[targetPool].y);
            state = FishState.Swimming;


            //Cutscene Code

            if (cutscene == true && targetPool == 0)
            {
                cutscene = false;
                rotation = Quaternion.AngleAxis(-60.0f, Vector3.forward);
                transform.rotation = rotation;
                transform.localScale = new Vector3(3, 3, 1);
               	pause = true;
            }
            else
            {
                resetOrientation();
            }
                
            
        }

        if (transform.position.y < pools[targetPool].y + 0.1f && submerged && jumpCounter > 2)
        {
           
            
            transform.position = new Vector2(pools[targetPool].x, pools[targetPool].y);

           

            state = FishState.Spitting;
            
        }
        else if(transform.position.y < pools[targetPool].y + 0.1f && !submerged)
        {
            stunCountdown = stunTime;
			healthSlider.value -= 1;
            state = FishState.Hurt;
           

        }
        //Debug.Log(submerged);
        
    }

    void Struggle()
    {
		anim.SetBool ("hurt", true);
		tag = "Untagged";
        if (struggleRotationCooldown < 0)
        {
            struggleRotationDirection *= -1;
            struggleRotationCooldown = 5;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180*struggleRotationDirection),0.2f);

        stunCountdown -= Time.deltaTime;
		if (stunCountdown < 0 && healthSlider.value == 0)
		{
			Debug.Log("dead");
			jumpCountdown = 2;
			state = FishState.Dead;
		}
        if(stunCountdown < 0 && submerged)
        {
            phase++;
            transform.position = new Vector2(pools[targetPool].x, pools[targetPool].y);
			tag = "Enemy";
            state = FishState.Spitting;
           
        }
        //Jumping before being submerged will cause bugs
        if(phase <= 3 && stunCountdown <0 && submerged)
        {
            //Debug.Log("reset");
            resetOrientation();
			anim.SetBool ("hurt", false);
        }
    }

    void splash()
    {
        Vector2 aim = new Vector3(transform.position.x - 6.0f, transform.position.y + 5f,0.0f);
        
        for (int i = 0;i < 7; i++)
        {
            if (transform.localScale.x < 0)
            {
                GameObject tempSpit = Instantiate(spit, transform.position, Quaternion.Inverse(transform.rotation));
                tempSpit.GetComponent<Projectile>().setTarget(aim, 0.25f, 0.01f);
            }
            else
            {
                GameObject tempSpit = Instantiate(spit, transform.position, transform.rotation);
                tempSpit.GetComponent<Projectile>().setTarget(aim, 0.25f, 0.01f);
            }
            
            

            aim.x += 2.0f;
        }
       
    }

    private void resetOrientation()
    {
        //set fish orientation
        if (transform.position.x - player.position.x < 0)
        {
            Quaternion rotation = Quaternion.AngleAxis(60.0f, Vector3.forward);
            transform.rotation = rotation;
            transform.localScale = new Vector3(-3, 3, 1);
        }
        else
        {
            Quaternion rotation = Quaternion.AngleAxis(-60.0f, Vector3.forward);
            transform.rotation = rotation;

            transform.localScale = new Vector3(3, 3, 1);
        }
    }

    public void setSubmerged(bool tempSub)
    {
        submerged = tempSub;
    }
   
    private void death()
    {
		jumpCountdown-= Time.deltaTime;
		if (jumpCountdown <= 1 && jumpCountdown > 0) 
		{
			anim.SetBool ("dead", true);
		} else if (jumpCountdown <= 0) 
		{
			Destroy (gameObject);
		}
        
    }

    public void setPlayerTarget(Transform tempPlayer)
    {
        player = tempPlayer;
    }

    public void pauseToggle()
    {
        pause = !pause;
    }
  

}
