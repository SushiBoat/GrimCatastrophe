using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpout : EnvironmentInteraction {

    //private bool colliding;
	public Animator topAnim;
	public Animator botAnim;
	public Collider2D topTrigger;
	public Collider2D botTrigger;

	[Header("Water Spout Settings")]
	[Tooltip("Water Spout starts on or off")]
	public bool switchedOn;
    public float duration;
    private float timer;
	private bool startingState;
	// Use this for initialization
	void Start () {
        timer = duration;
		startingState = switchedOn;
	}


	void Update () {
		if (switchedOn) 
		{
			topTrigger.enabled = true;
			botTrigger.enabled = true;
			topAnim.SetBool("switchedOn", true);
			botAnim.SetBool("switchedOn", true);
		}

		if (!switchedOn)
		{
			topTrigger.enabled = false;
			botTrigger.enabled = false;
			topAnim.SetBool("switchedOn", false);
			botAnim.SetBool("switchedOn", false);
		}

		timer -= Time.deltaTime;
		if(timer <= 0 && duration > 0)
		{
			switchedOn = startingState;
		}

	}

   public override void Activate()
    {
		timer = duration;
       	switchedOn = !switchedOn;
    }

    

	/*
	// Update is called once per frame
	void FixedUpdate () {
		if(colliding == false)
        {
            transform.Translate(new Vector3(0, 0.1f));
        }

	}


    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("spoutEnter");
        if (collision.gameObject.GetComponent<BreakablePlatform>())
        {
            colliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BreakablePlatform>())
        {
            //Debug.Log("SpoutExit");
            colliding = false;
        }
	}*/
}
