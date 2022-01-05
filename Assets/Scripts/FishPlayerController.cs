using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishPlayerController : MonoBehaviour {

    //character variables
    [Range(0,20)]
    public float moveSpeed;
    [Range(0, 20)]
    public float floatSpeed;
    [Range(250, 1500)]
    public float jumpSpeed;
    [Range(0, 20)]
    public float damageCooldown; //Time in seconds you want between each time player takes damage, i.e. invulnerability period
    [Range(0, 1000)]
    public float buoyancy; //force pushing character out of resricted zones i.e. water for cats
    float damageTimer;
    bool facingRight;
    Rigidbody2D myRB;
    public int fishLives; //player's lives as a fish
    bool inWater; //boolean to determine if player is in water

    // LV - Animator variables
    Animator myAnim; //player's animator
    float groundCheckRadius;
    public LayerMask groundLayer;
    public Transform groundCheck;
    bool checkGrounded; //boolean to determine if player is on ground

    public Slider healthSlider;

	int maxLives;

    //Changes to merge lives into one
    GameObject swapperRef;

    // Use this for initialization
    void Start () {
        swapperRef = GameObject.FindGameObjectWithTag("Swapper");

        facingRight = true;
        myRB = GetComponent<Rigidbody2D>();
        //damageTimer = damageCooldown;
        damageTimer = swapperRef.GetComponent<SwapPlayer>().getDamageCooldown();

        myAnim = GetComponent<Animator>();
        checkGrounded = false;
        groundCheckRadius = 0.2f;

		maxLives = fishLives;
    }
	
    void Update()
    {
        damageTimer -= Time.deltaTime;
        if (GetGlobalLives() <= 0)
        {
            this.transform.parent = GameObject.Find("Player").transform;
            Destroy(transform.parent.gameObject);
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
		// LV - Checking if damage cooldown is over to turn off the hurt animation
		if (damageTimer <= 0.001f) 
		{
			myAnim.SetBool ("hurt", false);
		}

        // Check if we are grounded; draws a circle, checks if it is intersecting with anything - if no, then we are falling
        checkGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float move = Input.GetAxis("Horizontal"); // Gets the input from Unity "horizontal" ie. A/D or left/right arrow keys
		myAnim.SetFloat("speed", Mathf.Abs(move));

        if (inWater)
        {
            myRB.velocity = Vector2.zero;
            myRB.gravityScale = 0.7f;
            transform.Translate(move * Time.deltaTime * moveSpeed, 0, 0);
            float verticalMove = Input.GetAxis("Vertical");
			myAnim.SetFloat("verticalSpeed", Mathf.Abs(verticalMove));
            transform.Translate(0, verticalMove * Time.deltaTime * floatSpeed, 0);
            if (Input.GetButton("Jump")) // Check if input from Unity "Jump" ie. space bar is pressed
            {
                myRB.velocity = Vector2.zero;
                myRB.AddForce(new Vector2(0, jumpSpeed));
            }
        }

        else if(!inWater)
        {
            myRB.gravityScale = 1;
            transform.Translate(move * Time.deltaTime * moveSpeed, 0, 0);
            if(checkGrounded == true)
            {
                myRB.velocity = Vector2.zero;
                myRB.AddForce(new Vector2(0.0f, buoyancy));
				if (damageTimer <= 0.001f) 
				{
					DamageGlobalPlayer(1);	
				}
                
            }

        }


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
            this.transform.parent = GameObject.Find("Player").transform;
        }
    }

    public int GetLives()
    {
        return fishLives;
    }

    void SetLives(int changeLivesBy)
    {
        fishLives += changeLivesBy;
    }

    public void DamagePlayer(int livesToDamageBy)
    {
        damageTimer = damageCooldown;
        fishLives = fishLives - livesToDamageBy;
        myAnim.SetTrigger("getsHurt");
        healthSlider.value = fishLives;

    }

	public void HealPlayer()
	{
		fishLives = maxLives;
		//healthSlider.value = fishLives;
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
        damageTimer = swapperRef.GetComponent<SwapPlayer>().getDamageCooldown(); ;
        swapperRef.GetComponent<SwapPlayer>().DamagePlayer(livesToDamageBy);
		myAnim.SetBool("hurt", true); // LV - start the hurting animation
        //if (fishLives <= 0)
        //{
        //    Destroy(transform.parent.gameObject);
        //}
    }

    public void HealGlobalPlayer()
    {
        swapperRef.GetComponent<SwapPlayer>().HealPlayer();
    }
}

