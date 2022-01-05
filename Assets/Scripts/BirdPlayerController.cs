using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdPlayerController : MonoBehaviour {

    //character variables
    [Range(0, 20)]
    public float moveSpeed; //player's max speed
    [Range(250, 1500)]
    public float jumpSpeed; //players max jump height ranging 1-20
    [Range(0.0f,0.5f)]
    public float jumpCooldown; //Time in seconds you want between each time player can "fly"
    float groundedTimerCooldown; //This is the time a player is allowed to jump after he leaves the ground (smoother jump)
    [Range(0, 20)]
    public float damageCooldown; //Time in seconds you want between each time player takes damage, i.e. invulnerability period
    [Range(0, 1000)]
    public float buoyancy; //force pushing character out of resricted zones i.e. water for birds
    float jumpTimer;
    float groundedTimer;
    float damageTimer;
    bool facingRight; //is player facing right?
    Rigidbody2D myRB; //player's rigidbody2D
    public int birdLives;
    bool inWater; //boolean to determine if player is in water

    // LV - Animator variables
    Animator myAnim; //player's animator
	float groundCheckRadius;
	public LayerMask groundLayer;
	public Transform groundCheck;
	bool checkGrounded; //boolean to determine if player is on ground

	int maxLives;

    public Slider healthSlider;

    //Changes to merge lives into one
    GameObject swapperRef;

    // Use this for initialization
    void Start()
    {
        swapperRef = GameObject.FindGameObjectWithTag("Swapper");

        //initialise default values and assign variables
        facingRight = true;
        myRB = GetComponent<Rigidbody2D>();
        jumpTimer = jumpCooldown;
        //damageTimer = damageCooldown;
        damageTimer = swapperRef.GetComponent<SwapPlayer>().getDamageCooldown();

        // LV - setup animator variables
        myAnim = GetComponent<Animator>(); 
		checkGrounded = false;
		groundCheckRadius = 0.2f;

		maxLives = birdLives;
		//Debug.Log (maxLives);
    }

    void Update()
    {
        damageTimer -= Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

		// LV - Checking if damage cooldown is over to turn off the hurt animation
		if (damageTimer <= 0.001f) 
		{
			myAnim.SetBool ("hurt", false);
		}


        if (inWater)
        {
            myRB.velocity = Vector2.zero;
            myRB.AddForce(new Vector2(0.0f, buoyancy));
            if (damageTimer <= 0.001f)
            {
                DamageGlobalPlayer(1);
            }
        }

        float move = Input.GetAxis("Horizontal"); // Gets the input from Unity pane "horizontal" ie. A/D or left/right arrow keys
        
		if (checkGrounded == false) // LV
		{
			transform.Translate(move * Time.deltaTime * moveSpeed, 0, 0);
		}
        
		// LV - check if grounded, used for falling animation
		checkGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
		myAnim.SetBool("isGrounded", checkGrounded);
		myAnim.SetFloat("verticalSpeed", myRB.velocity.y);
        
        if (Input.GetButton("Jump") && Time.fixedTime > jumpTimer) // Check if input from Unity "Jump" ie. space bar is pressed AND check if current time is greater than jump cooldown timer
        {
			if (FindObjectOfType<AudioManager>() != null)
				FindObjectOfType<AudioManager>().Play ("flap");
			// LV
			myAnim.SetTrigger("spacePressed");
			checkGrounded = false;
			myAnim.SetBool ("isGrounded", checkGrounded);

			jumpTimer = Time.fixedTime + jumpCooldown; //Set the new cooldown timer point

            //add upward force
            myRB.velocity = Vector2.zero; //ensure the previous velocity wont affect the flight when going up.
            myRB.AddForce(new Vector2(0.0f, jumpSpeed)); 

        }

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

        if ((jumpTimer) > 0 && (groundedTimer > 0))
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
        // Flip sprite depending on which direction theyre facing
        if (move > 0 && !facingRight)
        {
            flip();
        }
        else if (move < 0 && facingRight)
        {
            flip();
        }
    }

    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;  // makes the scale of the sprite x negative, so it flips horizontally
        transform.localScale = theScale;
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

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("electricPlatform") && other.gameObject.GetComponent<Obstacles>().getCurrent() == true && damageTimer <= 0.001f)
        {
            myRB.velocity = Vector2.zero;
            myRB.AddForce(new Vector2(0.0f, buoyancy));
            DamageGlobalPlayer(1);
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

        if (other.gameObject.CompareTag("movingPlatform"))
        {
            this.transform.parent = other.transform;
        }


    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("movingPlatform"))
        {
            this.transform.parent = null;
        }
    }
    public int GetLives()
    {
        return birdLives;
    }

    void SetLives(int changeLivesBy)
    {
        birdLives += changeLivesBy;
    }

    public void DamagePlayer(int livesToDamageBy)
    {
        damageTimer = damageCooldown;
        birdLives = birdLives - livesToDamageBy;
        myAnim.SetTrigger("getsHurt");
        healthSlider.value = birdLives;
        //if (birdLives <= 0)
        //{
        //    Destroy(transform.parent.gameObject);
          
        //}
    }

	public void HealPlayer()
	{
		birdLives = maxLives;
		//Debug.Log (birdLives);
		//healthSlider.value = birdLives;
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
        damageTimer = damageCooldown;
        swapperRef.GetComponent<SwapPlayer>().DamagePlayer(livesToDamageBy);
		myAnim.SetBool("hurt", true);
        if (birdLives <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void HealGlobalPlayer()
    {
        swapperRef.GetComponent<SwapPlayer>().HealPlayer();
    }
}
