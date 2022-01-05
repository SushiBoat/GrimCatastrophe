using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatPlayerController : MonoBehaviour {

    //character variables
    [Range(0, 20)]
    public float moveSpeed; //player's max speed
    [Range(250, 1500)]
    public float jumpSpeed; //players max jump height ranging 1-20
    [Range(0.0f,1000.5f)]
    public float jumpCooldown; //Time in seconds you want between each time player can Jump
    float groundedTimerCooldown; //This is the time a player is allowed to jump after he leaves the ground (smoother jump)
    [Range(0,20)]
    public float damageCooldown; //Time in seconds you want between each time player takes damage, i.e. invulnerability period
    [Range(0, 1000)]
    public float buoyancy; //force pushing character out of resricted zones i.e. water for cats
    float jumpTimer;
    float groundedTimer;
    float damageTimer;
    bool facingRight; //is player facing right?
    Rigidbody2D myRB; //player's rigidbody2D
    public int catLives; //player's lives
    bool inWater;
    bool hasJumped; //this is being used in jump to check if player has pressed jump button so we can set the cooldown  for jump if they touch the ground rather than when they press jump
    
    //Animator variables
    Animator myAnim; //player's animator
    float groundCheckRadius;
    public LayerMask groundLayer;
    public Transform groundCheck;
    bool checkGrounded; //boolean to determine if player is on ground
	public Slider healthSlider;

	int maxLives;
	bool inputEnabled;

    //Changes to merge lives into one
    GameObject swapperRef;


    // Use this for initialization
    void Start () {
        swapperRef = GameObject.FindGameObjectWithTag("Swapper");

        facingRight = true;
        myRB = GetComponent<Rigidbody2D>();
        jumpTimer = jumpCooldown;
        groundedTimerCooldown = 0.1f;
        groundedTimer = groundedTimerCooldown;
        //damageTimer = damageCooldown;
        damageTimer = swapperRef.GetComponent<SwapPlayer>().getDamageCooldown();
        myAnim = GetComponent<Animator>();
        checkGrounded = false;
        groundCheckRadius = 0.2f;

		maxLives = catLives;
		inputEnabled = true;
        hasJumped = false;

        myRB.drag = 1;
    }

	// Update is called once per frame, Fixed update is called once every physics engine loop
    void Update()
    {
        damageTimer -= Time.deltaTime;

        if (GetGlobalLives() <= 0)
        {
            this.transform.parent = GameObject.Find("Player").transform;
            Destroy(transform.parent.gameObject);
        }
    }
	void FixedUpdate () {

		// LV - Checking if damage cooldown is over to turn off the hurt animation
		if (damageTimer <= 0.001f) 
		{
			myAnim.SetBool ("hurt", false);
		}

        if (inWater)
        {
            if (damageTimer <= 0.001f)
            {
                //DamagePlayer(1);
                DamageGlobalPlayer(1);
            }
            myRB.velocity = Vector2.zero;
            myRB.AddForce(new Vector2(0.0f, buoyancy));
        }
        // Check if we are grounded; draws a circle, checks if it is intersecting with anything - if no, then we are falling
        checkGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
		myAnim.SetBool("isGrounded", checkGrounded);
		myAnim.SetFloat("verticalSpeed", myRB.velocity.y);
        
		if (myAnim.GetFloat ("verticalSpeed") < 0) {
			myAnim.SetTrigger ("fall");
		}

        /*
         //* NOTE: I COMMENTED THIS OUT BECAUSE I REALISED THAT HAVING THE CHECK TO SEE IF PLAYER HAS LANDED ON THE GROUND CAUSES A SLOW RESPONSE FOR JUMPING, WHICH CAN BE QUITE ANNOYING.
        if (Input.GetButton("Jump") && myRB.velocity.y < 0.001f && myRB.velocity.y > -0.001f && Time.fixedTime > jumpTimer) // Check if input from Unity "Jump" ie. space bar is pressed & make sure the player is not moving in the y axis AND if cooldown is complete
        {
            checkGrounded = false;
            myAnim.SetBool("isGrounded", checkGrounded);

			//if (myAnim.GetFloat ("verticalSpeed") > 0) {
			//	myAnim.SetTrigger ("jump");
			//} 
            jumpTimer = Time.fixedTime + jumpCooldown; //Set the new cooldown timer point

            myRB.velocity = Vector2.zero;
            //add upward force
            myRB.AddForce(new Vector2 (0.0f, jumpSpeed));
			myAnim.SetTrigger ("jump");
        }
        */

        /* new jumping implementation 
        groundedTimer -= Time.deltaTime;
        if (checkGrounded)
        {
            groundedTimer = groundedTimerCooldown;
        }

        jumpTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = jumpCooldown;
        }

        if((jumpTimer) > 0 && (groundedTimer > 0))
        {
            jumpTimer = 0;
            groundedTimer = 0;
            myAnim.SetBool("isGrounded", checkGrounded);

            //myRB.velocity = Vector2.zero;

            myRB.velocity = Vector2.zero;
            //add upward force
            myRB.AddForce(new Vector2(0.0f, jumpSpeed));
            myAnim.SetTrigger("jump");
        }
        */

		if (inputEnabled) {
			float move = Input.GetAxis ("Horizontal"); // Gets the input from Unity pane "horizontal" ie. A/D or left/right arrow keys
			transform.Translate (move * Time.deltaTime * moveSpeed, 0, 0);
			myAnim.SetFloat ("speed", Mathf.Abs (move));
			/************************** New Jumping Version 2.0******************************/
			groundedTimer -= Time.deltaTime;
			if (checkGrounded) {
				groundedTimer = groundedTimerCooldown;
                if (hasJumped)
                {
                    jumpTimer = Time.fixedTime + jumpCooldown; //Set the new cooldown timer point
                    hasJumped = false;
                }
            }
			//* NOTE: I COMMENTED THIS OUT BECAUSE I REALISED THAT HAVING THE CHECK TO SEE IF PLAYER HAS LANDED ON THE GROUND CAUSES A SLOW RESPONSE FOR JUMPING, WHICH CAN BE QUITE ANNOYING.
			if ((Input.GetButton ("Jump") || Input.GetKey (KeyCode.W)) && (groundedTimer > 0) && Time.fixedTime > jumpTimer && !(inWater)) { // Check if input from Unity "Jump" ie. space bar is pressed & make sure the player is not moving in the y axis AND if cooldown is complete
				groundedTimer = 0;
				checkGrounded = false;
                hasJumped = true;
				myAnim.SetBool ("isGrounded", checkGrounded);

                //Sound effects
                if(FindObjectOfType<AudioManager>() != null)
                {
                    FindObjectOfType<AudioManager>().Play("PlayerJump");
                }

                //if (myAnim.GetFloat ("verticalSpeed") > 0) {
                //	myAnim.SetTrigger ("jump");
                //} 

                myRB.velocity = Vector2.zero;
				//add upward force
				myRB.AddForce (new Vector2 (0.0f, jumpSpeed));
				myAnim.SetTrigger ("jump");

                
			}
			// Flip sprite depending on which direction theyre facing
			if (move > 0 && !facingRight) {
				flip ();
			} else if (move < 0 && facingRight) {
				flip ();
			}

		} else 
		{
			myAnim.SetFloat ("speed", 0);
		}

        

    }

    void flip()
    {
        facingRight = !facingRight;
		GetComponent<SpriteRenderer> ().flipX = !facingRight;
		/*
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;  // makes the scale of the sprite x negative, so it flips horizontally
        transform.localScale = theScale;
        */
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       // Debug.Log("something triggered player getting hit");

		if (other.gameObject.CompareTag("Projectile") && damageTimer <= 0.001f)
		{
            DamageGlobalPlayer(1);
			Destroy(other.gameObject);
		}
		else if (other.gameObject.CompareTag("Spike") && damageTimer <= 0.001f)
		{
			DamageGlobalPlayer(1);

		}

		if (other.gameObject.CompareTag("Enemy") && damageTimer <= 0.001f)
		{
			//Debug.Log("fish hit player");
			DamageGlobalPlayer(1);
		}

        if (other.gameObject.CompareTag("laserShooter") && damageTimer <= 0.001f)
        {
            if (other.gameObject.GetComponent<Transform>().position.x > this.transform.position.x)
            {
                myRB.velocity = Vector2.zero;
                myRB.AddForce(new Vector2(-800.0f, 0.0f));
            }
            else
            {
                myRB.velocity = Vector2.zero;
                myRB.AddForce(new Vector2(800.0f, 0.0f));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Projectile") && damageTimer <= 0.001f)
        {
            DamageGlobalPlayer(1);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Spike") && damageTimer <= 0.001f)
        {
            myRB.velocity = Vector2.zero;
            myRB.AddForce(new Vector2(0.0f, buoyancy));
            DamageGlobalPlayer(1);
        }

        if (other.gameObject.CompareTag("Enemy") && damageTimer <= 0.001f)
        {
            //Debug.Log("fish hit player");
            DamageGlobalPlayer(1);
        }

        if (other.gameObject.CompareTag("movingPlatform") && GetGlobalLives() > 0)
        {
            Debug.Log("on a moving platform");
            this.transform.parent = other.transform;
        }

        
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("electricPlatform") && other.gameObject.GetComponent<Obstacles>().getCurrent() == true && damageTimer <= 0.001f)
        {
            //if (other.gameObject.GetComponent<Transform>().position.x > this.transform.position.x)
            //{
            //    myRB.velocity = Vector2.zero;
            //    myRB.AddForce(new Vector2(-400.0f, 0.0f));
            //}
            //else
            //{
            //    myRB.velocity = Vector2.zero;
            //    myRB.AddForce(new Vector2(400.0f, 0.0f));
            //}

            myRB.velocity = Vector2.zero;
            myRB.AddForce(new Vector2(0.0f, buoyancy));

            Debug.Log("on an electric platform");
            DamageGlobalPlayer(1);

        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("movingPlatform"))
        {
            Debug.Log("off a moving platform");
            this.transform.parent = GameObject.Find("Player").transform;
        }
    }

    //Specific character lives functions
    public int GetLives()
    {
        return catLives;
    }

    void SetLives(int changeLivesBy)
    {
        catLives += changeLivesBy;
		healthSlider.value = catLives; // LV
    }

    public void DamagePlayer(int livesToDamageBy)
    {
        damageTimer = damageCooldown;
        catLives = catLives - livesToDamageBy;
		myAnim.SetTrigger ("getsHurt");
		healthSlider.value = catLives;
    }

	public void HealPlayer()
	{
		catLives = maxLives;
		//healthSlider.value = catLives;
	}

    public bool checkIfInWater()
    {
        return inWater;
    }

    public void setIfInWater(bool newStatus)
    {
        inWater = newStatus;
    }


    //Global character lives functions
    public int GetGlobalLives()
    {
        return swapperRef.GetComponent<SwapPlayer>().GetLives();
    }

    void SetGlobalLives(int changeLivesBy)
    {
        swapperRef.GetComponent<SwapPlayer>().SetLives(changeLivesBy);
    }

    public void DamageGlobalPlayer(int livesToDamageBy)
    {
        damageTimer = swapperRef.GetComponent<SwapPlayer>().getDamageCooldown();
        swapperRef.GetComponent<SwapPlayer>().DamagePlayer(livesToDamageBy);
		myAnim.SetBool("hurt", true); // LV - start the hurting animation
        //if (catLives <= 0)
        //{
        //    Destroy(transform.parent.gameObject);
        //}
    }

    public void HealGlobalPlayer()
    {
        swapperRef.GetComponent<SwapPlayer>().HealPlayer();
    }

	public void SetInputEnabled(bool x)
	{
		inputEnabled = x;
	}
}
