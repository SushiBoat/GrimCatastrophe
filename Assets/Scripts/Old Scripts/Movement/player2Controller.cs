using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player2Controller : MonoBehaviour
{

    // Movement Variables
    public float maxSpeed;


    // Jumping Variables
    bool grounded = false;
    float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;

    Rigidbody2D myRB;
    Animator myAnim;
    bool facingRight;

    // Use this for initialization
    void Start()
    {
        // Assigns components located in the "character" object to a variable
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (grounded && Input.GetAxis("Jump") > 0)
        {
            grounded = false;
            myAnim.SetBool("isGrounded", grounded);
            myRB.AddForce(new Vector2(0, jumpHeight));
        }
    }


    void FixedUpdate()
    {

        // Check if we are grounded; draws a circle, checks if it is intersecting with anything - if no, then we are falling
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        myAnim.SetBool("isGrounded", grounded);

        myAnim.SetFloat("verticalSpeed", myRB.velocity.y);

        float move = Input.GetAxis("Horizontal");  // Gets the input from Unity pane "horizontal" ie. A/D or left/right arrow keys
        myAnim.SetFloat("speed", Mathf.Abs(move));  // sets variable "speed" to Absolute value of move; since A will be negative value

        myRB.velocity = new Vector2(move * maxSpeed, myRB.velocity.y);  // Applies a velocity to the direction pressed

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
}
