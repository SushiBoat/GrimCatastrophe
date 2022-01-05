using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour {
    [Header("General Platform Settings")]
    [Range(0, 10)]
    public int platformMovementSpeed;
    Vector2 startingPos;

    [Header("Side Moving Platform Settings")]
    [Tooltip("Enable upMovingPlatform and sideMovingPlatform to make funky platforms moving diagonally")]
    public bool sideMovingPlatform;
    [Range(0, 30)]
    public int distanceToMoveOnX;
    bool movingRight;

    [Header("Up Moving Platform Settings")]
    [Tooltip("Enable upMovingPlatform and sideMovingPlatform to make funky platforms moving diagonally")]
    public bool upMovingPlatform;
    [Range(0, 30)]
    public int distanceToMoveOnY;
    bool movingUp;




    // Use this for initialization
    void Start () {
        startingPos = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (sideMovingPlatform && distanceToMoveOnX != 0)
        {
            //ensure platform swaps boolean when reached target on x axis
            if (transform.position.x > (startingPos.x + distanceToMoveOnX))
            {
                movingRight = false;
            }
            else if(transform.position.x < (startingPos.x - distanceToMoveOnX))
            {
                movingRight = true;
            }

            //Make platform move left or right
            if (movingRight)
            {
                this.transform.position = new Vector2(this.transform.position.x + platformMovementSpeed * Time.deltaTime, this.transform.position.y);
            }
            else
            {
                this.transform.position = new Vector2(this.transform.position.x - platformMovementSpeed * Time.deltaTime, this.transform.position.y);
            }
        }

        if (upMovingPlatform && distanceToMoveOnY != 0)
        {
            // ensure platform swaps boolean when reached target on y axis
            if (transform.position.y > (startingPos.y + distanceToMoveOnY))
            {
                movingUp = false;
            }
            else if (transform.position.y < (startingPos.y - distanceToMoveOnY))
            {
                movingUp = true;
            }

            //Make platform move left or right
            if (movingUp)
            {
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + platformMovementSpeed * Time.deltaTime);
            }
            else
            {
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - platformMovementSpeed * Time.deltaTime);
            }
        }
	}
}
