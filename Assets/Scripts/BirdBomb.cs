using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBomb : MonoBehaviour {
    private int direction;
    public float speed = 0.15f;
    public float lifeTime = 1.0f;
    private bool destroyed = false;
    private float animTimer = 0.25f;

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        lifeTime -= Time.deltaTime;

        if (destroyed == false)
        {
            transform.Translate(new Vector3(speed * direction, -speed, 0));
            if (lifeTime < 0)
            {
                destroyed = true;
				tag = "Untagged";
				anim.SetBool ("broken", true);
            }
        }
        else
        {
            animTimer -= Time.deltaTime;
            if (animTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    
        
	}

   public void setDirection(int tempDirection)
    {
        direction = tempDirection;
    }

    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

	public void setLifespan(float newLife)
	{
		lifeTime = newLife;
	}
}
