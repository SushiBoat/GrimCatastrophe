using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum PoolState { active,standby }
public class Pool : EnvironmentInteraction {

    public float refillCooldown;
    private float cooldown;

    private Vector3 moveTarget;

    private Vector3 moveIncrement;

    public float speed = 0.1f;

    private int position = 1;

    public float moveDistance = 1;

	private bool destroy = false;

    PoolState state = PoolState.standby;

    void Start() {
        cooldown = refillCooldown;


    }


    void FixedUpdate() {

        switch (state) {

            case PoolState.standby:
                if (position == 2)
                {
                    cooldown-= Time.deltaTime;
                    if (cooldown < 1)
                    {
                        state = PoolState.active;
                        position = 1;
                        setTarget(moveDistance);
                    }
                }
                break;

            case PoolState.active:
                transform.Translate(moveIncrement * speed);

                if (transform.position.y < moveTarget.y+0.1 && transform.position.y > moveTarget.y - 0.1)
                {
                    transform.position = new Vector3(transform.position.x, moveTarget.y,transform.position.z);
					if (destroy) {
						Destroy (gameObject);
						return;
					}

                	state = PoolState.standby;
                }

                break;
        }
}

    private void setTarget(float modifier)
    {
        moveTarget = new Vector3(transform.position.x, transform.position.y + modifier, transform.position.z);
        moveIncrement = moveTarget - transform.position;
        moveIncrement.Normalize();
        
    }

    public override void Activate()
    {
        
        state = PoolState.active;
        position = 2;
        cooldown = refillCooldown;
        setTarget(-moveDistance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.GetComponent<FishBoss>())
        {
           // Debug.Log("enter");
            collision.gameObject.GetComponent<FishBoss>().setSubmerged(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("exit");
        if (collision.gameObject.GetComponent<FishBoss>())
        {
            collision.gameObject.GetComponent<FishBoss>().setSubmerged(false);
        }
    }

	public void ActivateandDestroy()
	{
		speed = 0.15f;
		destroy = true;
		Activate();

	}
}
