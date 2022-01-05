using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

    public float lifeTime = 2;
    public float speed = 5f;
    private int direction = 1;

    private Animator anim;

    // Use this for initialization
    void Start () {
		anim = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		lifeTime -= 1 * Time.deltaTime;

		if (direction == 1)
		{

			transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
		}
		else
		{
			transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
		}

		if (lifeTime <= 0.25f) {
			if (anim != null)
				anim.SetBool ("broken", true);
			tag = "Untagged";
		}

		if (lifeTime <= 0) {
			Destroy (gameObject);
			return;	
		}
    }



    public void setLifespan(float tempLifeTime)
    {
        lifeTime = tempLifeTime;
    }

    public void setDirection(int tempDirection)
    {
        direction = tempDirection;
        if (direction == 1)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

}
