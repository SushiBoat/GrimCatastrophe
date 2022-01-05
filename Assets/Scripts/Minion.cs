using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour {

    public bool JumpingMinion;
    public bool SpittingMinion;
	public bool SwimmingMinion;
    public bool DroppingMinion;
    public bool SwoopingMinion;
	public float startDelayAmount; // initial delay before it starts jumping, in order to offset the fish timing with others
	bool startDelay; // flag so that the startDelay is only added once
    Rigidbody2D myRB; //rigidbody component of the minions

    //jumping minion variables
    [Header("Jumping minion")]
    [Range(250, 1500)]
    public float jumpSpeed; //players max jump height
    public float jumpCooldown; // The initial value for the jumping time in milliseconds

    float jumpTimer; //for jumping minions to jump in a specified interval
	// LV - need ground check to properly rotate minion when jumping
	float groundCheckRadius;
	public LayerMask groundLayer;
	public Transform groundCheck;
	bool checkGrounded;


    //spitting minion variable
    [Header ("Spitting minion")]
    public GameObject spit; //reference to the gameobject for spitting
    public float xVariation; //variation in x axis for the spit
    public float yVariation; //variation in y axis for the spit
    [Range(0, 1)]
    public float spitSpeed; //speed of the spit
    [Range(0, 1)]
    public float spitGravity; //gravity the spit if affected by
    public float spitCooldown; //The initial value for the spitting time in milliseconds
    float spitTimer; //for spitting minions to spit in a specified interval
    public float spitLifespan = 5;

	//spitting minion variable
	[Header ("Swimming minion")]
	public bool horizontalSwimmer;
	public bool verticalSwimmer;
	[Range(0, 10)]
	public int swimSpeed;
	Vector2 startingPos;
	[Range(0, 30)]
	public int distanceToMove;
	public bool flipStartingDirection;
	bool movingBack;

    //dropping minion variable
    [Header ("Dropping minion")]
    public GameObject bomb;
    [Range(-1, 1)]
    public int direction;
    [Range(0, 1)]
    public float dropSpeed;
    public float dropCooldown;
    float dropTimer;
	public float bombLifespan = 1;


    //swooping minion variables
    [Header ("Swooping minion")]
    public float swoopDepth;
    public Vector2 swoopDistance;
    private  Vector2 swoopEndpoint;
    public float swoopCooldown;
    public float swoopSpeed;
    public int swoopDirection;
	Vector2 originalPosition;
	float swoopTimer;
	float denom;
	float a;
	float b;
	float c;
	int swoopPhase = 0;

	//LV Animator variables
	Animator myAnim;


    // Use this for initialization
    void Start () {
        jumpTimer = jumpCooldown;
        spitTimer = spitCooldown;
        dropTimer = dropCooldown;
        swoopTimer = swoopCooldown;
        originalPosition = new Vector2(transform.position.x, transform.position.y);
		startingPos = this.transform.position;
        myRB = GetComponent<Rigidbody2D>();
		startDelay = true;

		movingBack = flipStartingDirection;
		if (SwimmingMinion)
			GetComponent<SpriteRenderer> ().flipX = flipStartingDirection;

		// LV Animator
		myAnim = GetComponent<Animator>();
		checkGrounded = false;
		groundCheckRadius = 1.0f;
		if (verticalSwimmer)
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));
       
    }
	
	// Update is called once per frame
	void Update () {
        if (JumpingMinion)
        {
			checkGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

			if (checkGrounded) // Only performs jump if grounded - prevents fish from jumping up again midair if cooldown is shorter than airtime
			{
				if (startDelay) 
				{
					jumpTimer = Time.fixedTime + startDelayAmount + jumpCooldown;
					startDelay = false;
				}
				if(Time.fixedTime > jumpTimer)
				{
					jumpTimer = Time.fixedTime + jumpCooldown; //reset the cooldown
					myRB.velocity = Vector3.zero;
					myRB.AddForce(new Vector2(0.0f, jumpSpeed));
				}
			}

			if (myRB.velocity.y > 1 && !checkGrounded) {
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -90),0.2f);
			} else if (myRB.velocity.y < -1 && !checkGrounded) {
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 90),0.2f);
			} else if (checkGrounded) {
				transform.eulerAngles = new Vector3 (0, 0, 0); // Can't lerp for reset on ground, otherwise it bugs out or changes the minions x position
			}


        }

		if (SpittingMinion)
		{
			if (startDelay) 
			{
				spitTimer = Time.fixedTime + startDelayAmount + spitCooldown;
				startDelay = false;
			}
			if(Time.fixedTime > spitTimer)
			{
				myAnim.SetTrigger ("spit"); // LV
				spitTimer = Time.fixedTime + spitCooldown;
				if (transform.localScale.x < 1)
				{
					GameObject tempSpit = Instantiate(spit, transform.position, transform.rotation);
					tempSpit.GetComponent<Projectile>().setTarget(new Vector3(transform.position.x + xVariation, transform.position.y + yVariation, transform.position.z), spitSpeed, spitGravity);
                    tempSpit.GetComponent<Projectile>().setLifespan(spitLifespan);
				}
				else
				{
					GameObject tempSpit = Instantiate(spit, transform.position, Quaternion.Inverse(transform.rotation));
					tempSpit.GetComponent<Projectile>().setTarget(new Vector3(transform.position.x + xVariation, transform.position.y + yVariation, transform.position.z), spitSpeed, spitGravity);
                    tempSpit.GetComponent<Projectile>().setLifespan(spitLifespan);
                }

			}
		}

		if (SwimmingMinion) 
		{
			if (Time.fixedTime > startDelayAmount) 
			{
				if (horizontalSwimmer && distanceToMove != 0) {
					//ensure platform swaps boolean when reached target on x axis
					if (transform.position.x > (startingPos.x + distanceToMove)) {
						movingBack = false;
						GetComponent<SpriteRenderer> ().flipX = movingBack;
					} else if (transform.position.x < (startingPos.x - distanceToMove)) {
						movingBack = true;
						GetComponent<SpriteRenderer> ().flipX = movingBack;
					}

					//Make platform move left or right
					if (movingBack) {
						this.transform.position = new Vector2 (this.transform.position.x + swimSpeed * Time.deltaTime, this.transform.position.y);
					} else {
						this.transform.position = new Vector2 (this.transform.position.x - swimSpeed * Time.deltaTime, this.transform.position.y);
					}
				}
				if (verticalSwimmer && distanceToMove != 0)
				{
					// ensure platform swaps boolean when reached target on y axis
					if (transform.position.y > (startingPos.y + distanceToMove))
					{
						movingBack = false;
						GetComponent<SpriteRenderer> ().flipX = movingBack;
					}
					else if (transform.position.y < (startingPos.y - distanceToMove))
					{
						movingBack = true;
						GetComponent<SpriteRenderer> ().flipX = movingBack;
					}

					//Make platform move left or right
					if (movingBack)
					{
						this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + swimSpeed * Time.deltaTime);
					}
					else
					{
						this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - swimSpeed * Time.deltaTime);
					}
				}
			}
		}

        if (DroppingMinion)
        {
			if (startDelay) 
			{
				dropTimer = Time.fixedTime + startDelayAmount + dropCooldown;
				startDelay = false;
			}
            if(Time.fixedTime > dropTimer)
            {
				myAnim.SetTrigger ("drop"); // LV
				dropTimer = Time.fixedTime + dropCooldown;
                GameObject tempBomb = Instantiate(bomb, transform.position, transform.rotation);
                tempBomb.GetComponent<BirdBomb>().setSpeed(dropSpeed);
                tempBomb.GetComponent<BirdBomb>().setDirection(direction);
				tempBomb.GetComponent<BirdBomb>().setLifespan(bombLifespan);
            }
        }

        if (SwoopingMinion)
        {
            switch (swoopPhase)
            {
				case 0:
					myAnim.SetBool ("returning", false); // LV
					if (startDelay) 
					{
						swoopTimer = Time.fixedTime + startDelayAmount + swoopCooldown;
						startDelay = false;
					}
                    if (Time.fixedTime > swoopTimer)
                    {
                        swoopPhase = 1;
						GetComponent<BoxCollider2D> ().isTrigger = false;
                    }
                    break;

				case 1:
					myAnim.SetBool ("swooping", true);
                    swoopEndpoint = new Vector2(originalPosition.x + swoopDistance.x, originalPosition.y + swoopDistance.y);
                    calculateSwoop(swoopEndpoint, originalPosition.y - swoopDepth);
                    swoopPhase = 2;
                    break;

                case 2:
                    incrementSwoop(swoopDirection);
                    if (transform.position.y > swoopEndpoint.y)
                    {
                        swoopPhase = 3;
						myAnim.SetBool ("returning", true);
						myAnim.SetBool ("swooping", false);
						GetComponent<BoxCollider2D> ().isTrigger = true;
                    }
                    break;

                case 3:
                    calculateSwoop(originalPosition, originalPosition.y + 1.0f);
                    swoopPhase = 4;
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    break;

                case 4:
                    incrementSwoop(swoopDirection*-1);
                    if((transform.position.x <= originalPosition.x && swoopDirection == 1) || (transform.position.x >= originalPosition.x && swoopDirection == -1))
                    {
                        swoopPhase = 0;
                        swoopTimer = Time.fixedTime + swoopCooldown;
                        transform.rotation = Quaternion.identity;
                        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                        transform.position = originalPosition;
                    }
                    break;
            }
        }
	}

    void calculateSwoop(Vector2 swoopTarget,float depth)
    {
        //swoopSpeed = 0.2f;

        float x1 = transform.position.x;
        float y1 = transform.position.y;

        float x2 = (transform.position.x + swoopTarget.x)/2;

        float y2 = depth;

        float x3 = swoopTarget.x;
        float y3 = swoopTarget.y;

        denom = (x1 - x2) * (x1 - x3) * (x2 - x3);
        a = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom;
        b = ((x3 * x3) * (y1 - y2) + (x2 * x2) * (y3 - y1) + (x1 * x1) * (y2 - y3)) / denom;
        c = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;

    }

    void incrementSwoop(int direction)
    {
       // anim.SetTrigger("swoop");
        float targetX;
        float targetY;


        targetX = transform.position.x + swoopSpeed*direction;
        targetY = (a * targetX * targetX) + (b * targetX) + c;

        Vector3 target = new Vector3(targetX, targetY);
        Quaternion rotation = Quaternion.LookRotation
       (target - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

        transform.position = new Vector3(targetX, targetY, 0.0f);
    }
}
