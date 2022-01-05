using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum NarwhalState { Move, Laser, Wait,Hurt,Dead}
public class NarwhalBoss: MonoBehaviour {
    private NarwhalState state = NarwhalState.Wait;
    public float[] laneHeights = new float[3] { 85.0f, 105.0f, 120.0f };
    public float restZone1;
    public float restZone2;

    public Transform player;

    private Vector3 moveTarget;
    private Vector3 velocity;

    private bool laserToggle = false;

    
    private int phase = 1;
    public float speed;

	public float waitTimer;
	private float waitCountdown;

	public float laserChargeTime;
	private float laserChargeCounter;

	public float laserTimer;
	private float laserCountdown;

    private bool laneCheck = false;

	public float patrolLength;
	public float patrolCooldown;
	private float patrolCooldownCounter;
	private float patrolCounter = 0;


	public float stunTimer;
	private float stunCountdown;

    public GameObject laserBeam;

    private Quaternion targetRotation;

	Animator anim;
	public Slider healthSlider;
	NarwhalState tempState;

	//For cutscene
	bool playCutscene;
	float cutsceneTimer;


    // Use this for initialization
    void Start () {
        waitCountdown = waitTimer;
        stunCountdown = stunTimer;
        laserCountdown = laserTimer;
        patrolCooldownCounter = patrolCooldown;
		laserChargeCounter = laserChargeTime;

		anim = GetComponent<Animator>();
		healthSlider.value = 3;

		playCutscene = true;
		cutsceneTimer = 7;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (player)
        {
            //Dont update unless player is close
			if (player.position.x > restZone1 - 4 && player.position.x < restZone2 + 5 && player.position.y > laneHeights[2]) //if (player.position.y > laneHeights[0] - 10 && player.position.x > restZone1 - 10 && player.position.x < restZone2 + 10)
            {
				if (playCutscene) {
					cutsceneTimer -= Time.deltaTime;
					if (cutsceneTimer <= 3 && cutsceneTimer > 0) 
					{
						anim.SetBool ("awake", true);
					} else if (cutsceneTimer <= 0) {
						playCutscene = false;
					}
				} else {
					
					//Debug.Log(moveTarget);
					switch (state) {
					case NarwhalState.Move:
						reposition ();
						break;
					case NarwhalState.Laser:
						laser ();
						break;
					case NarwhalState.Wait:
						wait ();
						break;
					case NarwhalState.Hurt:
						hurt ();
						break;
					}
				}
            }
        }

       // Debug.Log(patrolCooldownCounter);

		
	}

    void wait()
    {
		waitCountdown -= Time.deltaTime;
        if (patrolCounter > 0)
        {
            //Debug.Log("patrol");
			patrolCounter -= Time.deltaTime;

            if (patrolCounter <= 0)
            {
                state = NarwhalState.Laser;
                laserChargeCounter = laserChargeTime;

            }
            else if (patrolCounter <= 1)
            {
                //line up accurate shot
                setPatrolTarget(true);
            }
            else
            {
                setPatrolTarget(false);
            }
        }
        else if(waitCountdown <= 0)
        {
            waitCountdown = waitTimer;
            
            if(laserToggle == true)
            {
				anim.SetBool("shooting", true);
                state = NarwhalState.Laser;
                
            }
            else
            {
				patrolCooldownCounter -= Time.deltaTime;
                if(patrolCooldownCounter <= 0)
                {
                    patrolCooldownCounter = patrolCooldown;
                    patrolCounter = patrolLength;
                }
                laneCheck = false;
                setTarget();
                state = NarwhalState.Move;
            }
        }
    }

    void laser()
    {
		laserChargeCounter -= Time.deltaTime;
        if (laserChargeCounter <= 0)
        {
            if (laserToggle == true)
            {
                if (transform.position.x == restZone1)
                {
                    GameObject tempLaser = Instantiate(laserBeam, new Vector3(transform.position.x + 37, transform.position.y + 1.2f, transform.position.z), transform.rotation);
                    tempLaser.GetComponent<LaserBeam>().setLifeSpan(laserCountdown);
                }
                else
                {
                    GameObject tempLaser = Instantiate(laserBeam, new Vector3(transform.position.x - 37, transform.position.y + 1.2f, transform.position.z), transform.rotation);
                    tempLaser.GetComponent<LaserBeam>().setLifeSpan(laserCountdown);
                    tempLaser.transform.localScale = new Vector3(tempLaser.transform.localScale.x * -1, tempLaser.transform.localScale.y, tempLaser.transform.localScale.z);
                }

                laserToggle = false;
            }

			laserCountdown -= Time.deltaTime;
            if (laserCountdown <= 0)
            {
				anim.SetBool("shooting", false);
				laserCountdown = laserTimer;
                state = NarwhalState.Wait;
                laserChargeCounter = laserChargeTime;
            }
        }
        
    }

    void setTarget()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), 1);

        if (laneCheck == false)
        {
            moveTarget.y = laneHeights[Random.Range(0, 3)];

            if (moveTarget.y != transform.position.y)
            {
                moveTarget.x = transform.position.x;
            }
            else
            {
                laneCheck = true;
            }

        }

        if (laneCheck == true)
        {
            if (gameObject.transform.position.x == restZone1)
            {
                moveTarget.x = restZone2;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, 1);
            }
            else
            {
                moveTarget.x = restZone1;
            }
            
        }

        velocity = moveTarget- transform.position;
        velocity.Normalize();
        velocity *= speed;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }

    void reposition()
    {
        //Debug.Log(moveTarget.x);
       // Debug.Log(moveTarget.y);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,10);



        transform.position += velocity;

        if(transform.position.x >= moveTarget.x - 0.5f && transform.position.x <= moveTarget.x + 0.5f && transform.position.y >= moveTarget.y - 0.5f && transform.position.y <= moveTarget.y + 0.5f)
        {
            transform.position = moveTarget;
            if(laneCheck == false)
            {
                laneCheck = true;
                setTarget();
            }
            else
            {
                laserToggle = true;
                if(transform.position == new Vector3(restZone1,transform.position.y))
                {
                    transform.rotation = Quaternion.identity;
                    transform.Rotate(0, 0, 180);
                    transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 1);
                }
                else
                {
                    transform.rotation = Quaternion.identity;
                    transform.Rotate(0, 0, 180);
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y* -1, 1);
                }
                state = NarwhalState.Wait;
            }
        }
    }

    void hurt()
    {
		anim.SetBool ("getsHurt", true);
		stunCountdown -= Time.deltaTime;
        if(stunCountdown <= 0)
        {
            phase++;
            stunCountdown = stunTimer;
            patrolLength -= 2;
			anim.SetBool ("getsHurt", false);
			state = tempState;  //NarwhalState.Wait;
        }

        if(phase > 3)
        {
            //temp death code
            Destroy(gameObject);
        }
        
    }

    private void patrol()
    {
        laneCheck = false;
        setTarget();
        state = NarwhalState.Move;
    }

    private void setPatrolTarget(bool aiming)
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), 1);
        if (aiming == false)
        {
            moveTarget.x = transform.position.x;
            if(transform.position.y == laneHeights[0])
            {
                moveTarget.y = laneHeights[2];
            }
            else
            {
                moveTarget.y = laneHeights[0];
                
            }
        }
        else
        {
            moveTarget.x = transform.position.x;
            moveTarget.y = player.position.y + Random.Range(-5, 5);
            if(moveTarget.y < laneHeights[0])
            {
                moveTarget.y = laneHeights[0];
            }
            else if(moveTarget.y > laneHeights[2])
            {
                moveTarget.y = laneHeights[2];
            }
        }

        velocity = moveTarget - transform.position;
        velocity.Normalize();
        velocity *= speed;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        state = NarwhalState.Move;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log("NarwhalCollision");
        if (collision.gameObject.CompareTag("Lightning"))
        {
			healthSlider.value -= 1;
			tempState = state;
            state = NarwhalState.Hurt;
            //Debug.Log("Hurt");
        }
    }

    public void setPlayerTarget(Transform newTarget)
    {
        player = newTarget;
    }

}
