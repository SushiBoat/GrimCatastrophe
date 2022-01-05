using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {
	private float lifespan = 3;

	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		lifespan -= Time.deltaTime;

		if (lifespan <= 0.2f && lifespan > 0) 
		{
			if (anim != null) 
			{
				anim.SetBool ("end", true);
			}
		}
		else if(lifespan <= 0)
        {
            Destroy(gameObject);
			return;
        }
	}

	public void setLifeSpan(float tempLifespan)
    {
        lifespan = tempLifespan;
    }
}
