using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPost : EnvironmentInteraction {

    public float duration;
    private bool active = false;
    private float timer;
	// Use this for initialization
	void Start () {
        setActivation(false);
    }
	
	// Update is called once per frame
	void Update () {
		if(active == true)
        {
            if(timer <= 0)
            {
                active = false;
                setActivation(false);

            }
        }
	}

    public override void Activate()
    {
       
        active = !active;
        setActivation(active);
        timer = duration;
    }

    private void setActivation(bool activation)
    {
        //Debug.Log(active);
        if (activation == false)
        {
            gameObject.tag = "Untagged";
        }
        else
        {
            gameObject.tag = "Lightning";
        }
        transform.GetChild(0).gameObject.SetActive(activation);
    }

   
}
