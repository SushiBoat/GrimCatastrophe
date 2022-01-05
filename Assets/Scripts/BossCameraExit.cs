using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCameraExit : MonoBehaviour {
    cameraFollow2DPlatformer cref;
	public int zone;
	bool enterOnce = false;

    void Start()
    {
        cref = GameObject.Find("Main Camera").GetComponent<cameraFollow2DPlatformer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Cat") || other.CompareTag("Fish") || other.CompareTag("Bird"))
        {
			if (zone == 1 || zone == 2) {
				cref.setInBossArea (false, 0);
			} else 
			{
				cref.setInBossArea (true, zone);
			}

        }
    }

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag ("Cat") || other.CompareTag ("Fish") || other.CompareTag ("Bird")) 
		{
			if (zone == 22 || zone == 23) 
			{
				Destroy (gameObject);
				return;
			}
		}
	}

	/*
    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Cat") || other.CompareTag("Fish") || other.CompareTag("Bird"))
        {
            cref.setInBossArea(false,0);
        }
    }
	*/
}
