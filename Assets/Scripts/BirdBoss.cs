using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum BirdState { Standby,Repositioning,Struggle,Dead,CreateWind,Patroll,Land,Rest }
public class BirdBoss : MonoBehaviour {

    public Transform player;
    public GameObject wind;
	public Slider healthSlider;

	Animator anim;
    

    private BirdState state = BirdState.Standby;

    private int phase = 1;
   
   
    private Vector3 target;
   
    public float windCooldown = 200;
    public float restDuration = 10;
    public float deathDuration = 5;
    public float windAnimTime = 4;
    public Vector3[] Positions;
    public Vector3[] Posts;
    public float patrollX1 = -90;
    public float patrollX2 = - 50;
    public float speed = 1.0f;

    private int direction = 1;
    private int positionI = 0;
    private Vector3 landing;
    private Vector3 velocity;
    private int velocityReset = 1;
    private float[] windHeight;
    private int windHeightI =0;

    public float stunTime;
    private float stunCountdown;

    private float windTimer;
    private float animTimer;
    private float restTimer;

	// Used for cutscene
	private bool cutsceneDelay = true; 
	private float cutsceneDelayAmount = 6;

    private bool shocked = false;


    // Use this for initialization
    void Start () {
		healthSlider.value = 3;
       stunCountdown = stunTime;
       if(positionI == 1 || positionI == 3)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
		anim = GetComponent<Animator>();

       if(phase == 2)
        {
            state = BirdState.Patroll;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        windHeight = new float[3];
        windHeight[0] = transform.position.y + 2;
        windHeight[1] = transform.position.y - 6;
        windHeight[2] = transform.position.y - 12;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (player)
        {
            if (player.position.x < transform.position.x + 76 && player.position.x > transform.position.x - 76)
            {
                if (phase == 1)
                {
                    switch (state)
                    {
						case BirdState.Standby:
                            standby();
                           // Debug.Log("standby " + windTimer);
                            break;

                        case BirdState.CreateWind:
                            createWind();
                            //Debug.Log("wind");
                            break;

                        case BirdState.Repositioning:
                            reposition();
                           // Debug.Log("reposition");
                            break;
                        case BirdState.Struggle:
                            struggle();
                            break;
						case BirdState.Dead:
							death();
							break;

                    }
                } else if (phase == 2)
                {
                    switch (state)
                    {
                        case BirdState.Repositioning:
                            reposition();
                            break;
                        case BirdState.Patroll:
                            patroll();
                            break;
                        case BirdState.CreateWind:
                            createWind();
                            break;
                        case BirdState.Struggle:
                            struggle();
                            break;
                        case BirdState.Land:
                            land();
                            break;
                        case BirdState.Rest:
                            rest();
                            break;
                        case BirdState.Dead:
                            death();
                            break;
                    }
                }

                // Debug.Log(state);
               // Debug.Log(positionI);
                //Debug.Log(Positions.Length);
                //Debug.Log(height);
                //Debug.Log(phase);
            }
        }
    }

    //TODO: Make wind target player

    void standby()
    {
		anim.SetBool ("sit", true);
		// Used to add a delay for the cutscene, will only happen once the first time its called
		if (cutsceneDelay) 
		{
			cutsceneDelay = false;
			windTimer += cutsceneDelayAmount;
		}
        //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        windTimer-= 1*Time.deltaTime;
        //Debug.Log(windTimer);
        if(windTimer <= 0)
        {
            windTimer = windCooldown;
            animTimer = windAnimTime;
            state = BirdState.CreateWind;
        }

      //  if(shocked == true)
       // {
           // state = BirdState.Repositioning;
          //  shocked = false;
       // }
        /*if(player.transform.position.y > transform.position.y - 5)
        {
            state = BirdState.Repositioning;
        }*/
    }

    void createWind()
    {
        //Play animation
		anim.SetBool ("sit", false);
		anim.SetTrigger("preSwoop");
        animTimer -= 1 * Time.deltaTime;
        if (animTimer <= 0)
        {
            windHeightI++;
            
            if(windHeightI > 2)
            {
                windHeightI = 0;
            }

          float  tempWindHeight = windHeight[windHeightI];

            for (int i = 0; i < 4; i++)
            {
                GameObject tempWind = Instantiate(wind, new Vector3(transform.position.x,tempWindHeight,transform.position.z), transform.rotation);
               if(transform.localScale.x > 0)
                {
                    tempWind.GetComponent<Wind>().setDirection(1);
                }
                else
                {
                    tempWind.GetComponent<Wind>().setDirection(-1);
                }
                
                tempWindHeight +=2;
            }
           
             
            windTimer = windCooldown;
            if (phase == 1)
            {
                state = BirdState.Standby;
            }
            else
            {
                state = BirdState.Patroll;
            }
        }
    }


    void reposition()
    {
		anim.SetBool ("sit", false);
		anim.SetTrigger ("fly");
        Vector3 moveTarget;
        
        
        if (velocityReset == 1)
        {
            
            if (phase == 1 && positionI <= (Positions.Length-1))
            {
                //Debug.Log(phase);
                //Debug.Log(positionI);
                moveTarget = new Vector3(Positions[positionI].x, Positions[positionI].y, 0);
            }
            else
            {
                moveTarget = Posts[0];
                

            }
            velocity = moveTarget - transform.position;
            velocity.Normalize();
            velocityReset = 0;
            //Quaternion rotation = Quaternion.LookRotation
            //(Positions[positionI] - transform.position, transform.TransformDirection(Vector3.up));
            //transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            if(velocity.x < 0)
            {
                transform.localScale = new Vector3(-2f, 2f, 2f);
            }
            else
            {
                transform.localScale = new Vector3(2f, 2f, 2f);
            }
            positionI++;
            if(positionI > Positions.Length)
            {
                phase = 2;    
            }
        }

        transform.position += new Vector3((velocity.x * speed), velocity.y * Mathf.Abs(speed), 0);
       

        if(phase == 1) {

            if (Vector3.Distance(transform.position, Positions[positionI - 1]) < 0.5)
            {

                transform.rotation = Quaternion.identity;
                
                transform.localScale = new Vector3(transform.localScale.x * -1f, 2f, 2f);
                
                velocityReset = 1;

                windHeight[0] = transform.position.y + 2;
                windHeight[1] = transform.position.y - 6;
                windHeight[2] = transform.position.y - 12;

                state = BirdState.Standby;
                
                }
        }
        if (phase == 2)
        {
            if (Vector3.Distance(transform.position, Posts[0]) < 0.2)
            {
                state = BirdState.Patroll;
            }
        }



    }



    void patroll ()
    {
		
		transform.Translate(new Vector3(speed, 0, 0));
       
		//anim.SetTrigger ("standby");

        //change to center of arena 
        if(transform.position.x < patrollX1)
        {
           transform.position = new Vector3(patrollX1, Posts[0].y);
           speed *= -1;
           transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            
        }
        if(transform.position.x > patrollX2)
        {
            transform.position = new Vector3(patrollX2, Posts[0].y);
            speed *= -1;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }


        windTimer -= 1 * Time.deltaTime;
        if (windTimer < 0)
        {
            createWind();
        }


    }

    void struggle()
    {
		anim.SetBool ("sit", false);
		anim.SetTrigger ("getHurt");
        stunCountdown-= Time.deltaTime;
        //Debug.Log(stunCountdown);

        if (stunCountdown <= 0)
        {
			if (healthSlider.value == 0) {
				animTimer = deathDuration;
				state = BirdState.Dead;
			} else if (phase == 1)
            {
               // Debug.Log("reposition");
                stunCountdown = stunTime;
                velocityReset = 1;
                state = BirdState.Repositioning;
            }
            else if (phase == 2)
            {
                state = BirdState.Land;
                velocityReset = 1;
                stunCountdown = stunTime;
            }
        } 
    }

    void land()
    {
       
		//landing/wet anim
        landing = Posts[0];
        //Debug.Log(landing);
        for(int i = 1;i < Posts.Length; i++)
        {
            if(Vector3.Distance(transform.position,Posts[i]) < Vector3.Distance(transform.position,landing))
            {
                landing = Posts[i];
            }
        }
        if (velocityReset == 1)
        {
            Debug.Log(landing);
            velocity = landing - transform.position;
            Debug.Log(velocity);
            velocity.Normalize();
            velocityReset = 0;
        }

        // transform.position += new Vector3((velocity.x * speed), velocity.y * Mathf.Abs(speed), 0);
        transform.position += velocity * speed;
        //Debug.Log(velocity);

        if (Vector3.Distance(transform.position,landing) < 0.2f)
        {
            transform.position = landing;
            transform.rotation = Quaternion.identity;
            velocityReset = 1;
            restTimer = restDuration;
            state = BirdState.Rest;
        }
    }

    void rest()
    {
        restTimer -= 1 * Time.deltaTime;
        if(restTimer <= 0)
        {
            
            state = BirdState.Patroll;
        }



    }

    public void setPlayerTarget(Transform newTarget)
    {
        player = newTarget;
    }

    public void setWet()
    {
        //Debug.Log("hurt");
        state = BirdState.Struggle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.GetComponent<WaterSpout>() && state == BirdState.Patroll)
        {
            state = BirdState.Struggle;
        }

        if (collision.gameObject.CompareTag("Lightning") && state == BirdState.Rest)
        {
            animTimer = deathDuration;
            state = BirdState.Dead;
        }

        // if (collision.gameObject.GetComponent<Electricity>() && state == BirdState.Rest)
        // {
        //  state = BirdState.Dead;
        //  animTimer = deathDuration;
        // }

    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Lightning"))
        {
           // Debug.Log("collision");
            //This occurs before the final phase
			if (phase == 1 && state == BirdState.Standby) {
				healthSlider.value -= 1;
				state = BirdState.Struggle;
				//Debug.Log("struggleTrigger");
            }

//            //This occurs in the fina			l phase after the bird is hit by water
//            if (state == BirdState.Rest)
//            {
//                animTimer = deathDuration;
//                state = BirdState.Dead;
//            }
            
        }
        
    }



        private void death()
    {
		anim.SetBool ("dead", true);
        animTimer -= 1 * Time.deltaTime;
        if (animTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int getPhase()
    {
        return phase;
    }

    public int getCheckpoint()
    {
        return positionI;
    }

    public Vector3 getCheckpointPosition(int checkpoint)
    {
        return Positions[checkpoint];
    }

    public Vector3 getBirdPosts(int post)
    {
        return Posts[post];
    }

    public void setCheckpoint(int checkpoint)
    {
        positionI = checkpoint;
    }

    public void setPhase(int tempPhase)
    {
        phase = tempPhase;
    }
     
  


        
}


