using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is now waterdrop only
public class Projectile : MonoBehaviour {
  
    public float lifeTime = 2;
    public float speed = 5f;
  
    private Vector3 target;
    private Vector3 rotationTarget;
    public float gravity = 0.1f;
    private float gravityCurve = 1f;
	private Animator anim;

	void Start() 
	{
		
			anim = GetComponentInChildren<Animator> ();
		
	}
    

    // Use this for initialization


    // Update is called once per frame
    void FixedUpdate () {
		lifeTime -= 1*Time.deltaTime;
		if (lifeTime <= 0.1f)
		{
			if (anim != null) 
			{
				if (!anim.GetBool ("broken")) 
				{
					anim.SetBool ("broken", true);
					transform.GetChild (0).tag = "Untagged";
				}
			}
		}   
		else
		{
			waterDrop();
		}


		if (lifeTime <= 0)
		{
			Destroy(gameObject);
			return;
		}
	}

   

    private void waterDrop()
    {
        target.y -= gravity;

        if (target != Vector3.zero)
        {
            
            Quaternion rotation = Quaternion.LookRotation
            (target, transform.TransformDirection(Vector3.forward));
          
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
                transform.position += target * speed;

        }
        else
        {
            if (target.y < transform.position.y)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
            }
        }

    }

    public void setTarget(Vector3 tempTarget,float tempSpeed,float tempGravity)
    {

       

            gravity = tempGravity;
            speed = tempSpeed;
            target = tempTarget - transform.position;
            
            target.Normalize();
            //Debug.Log(target);
            
       
        
    }

    public void setLifespan(float tempLifeTime)
    {
        lifeTime = tempLifeTime;
    }


    
}
