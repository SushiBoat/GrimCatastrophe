using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow2DPlatformer : MonoBehaviour {

	Transform target;  // What the camera is following
	public float smoothing;  // The movespeed of the camera; how smooth it is
	float smoothDefault;

	Vector3 offset;  // the distance between the player character and camera

	float lowY;  // the lowest point the camera can go - so if character falls of edge, it doesnt follow
	float tempY;
	float tempX;
	float xOffset; // for birdboss, choose if offsetting to the left or right

	public SwapPlayer swapperReference; //reference to player swapper to know which character is active

	bool inBossArea;
	private int areaNo;
	bool cutscenePlaying;
	// Use this for initialization
	void Start () {
		//target = GameObject.FindGameObjectWithTag("Cat").GetComponent<Transform>();
		target = swapperReference.GetComponent<Transform>();
		offset = transform.position - target.position;
		lowY = transform.position.y;
		smoothDefault = smoothing;


		inBossArea = false;
		cutscenePlaying = false;
		xOffset = -10;
	}


	void Update()
	{
//		if (!inBossArea)
//		{
//			//smoothing = smoothDefault;
//			//Ensure the camera follows the player even when the swapping of characters is done.
//			//            if (swapperReference.getCharacterName() == "Cat")
//			//            {
//			//                if (GameObject.FindGameObjectWithTag("Cat") != null)
//			//                {
//			//                    target = GameObject.FindGameObjectWithTag("Cat").GetComponent<Transform>();
//			//                }
//			//            }
//			//            else if (swapperReference.getCharacterName() == "Fish")
//			//            {
//			//                if (GameObject.FindGameObjectWithTag("Fish") != null)
//			//                {
//			//                    target = GameObject.FindGameObjectWithTag("Fish").GetComponent<Transform>();
//			//                }
//			//            }
//			//            else if (swapperReference.getCharacterName() == "Bird")
//			//            {
//			//                if (GameObject.FindGameObjectWithTag("Bird") != null)
//			//                {
//			//                    target = GameObject.FindGameObjectWithTag("Bird").GetComponent<Transform>();
//			//                }
//			//            }
//		}
//		else 

		if (inBossArea)
		{
			if (areaNo == 1)
			{
				smoothing = 1.5f;
				target = GameObject.FindGameObjectWithTag("FishBossCamera").GetComponent<Transform>();
			}
			else if(areaNo == 2)
			{
				target = GameObject.FindGameObjectWithTag ("BirdBossCamera").GetComponent<Transform> ();	
				if (cutscenePlaying) 
				{
					smoothing = 1.5f;
				} else 
				{
					target.position = new Vector3 (swapperReference.GetComponent<Transform>().position.x + xOffset, tempY, swapperReference.GetComponent<Transform>().position.z - 5.0f);
					//					smoothing = smoothDefault;
					//					target.transform.position = new Vector3(swapperReference.GetComponent<Transform>().position.x - 10, GameObject.FindGameObjectWithTag ("BirdBossCamera").GetComponent<Transform> ().position.y, GameObject.FindGameObjectWithTag ("BirdBossCamera").GetComponent<Transform> ().position.z);	
				}
			} 
			else if (areaNo == 22 || areaNo == 23)
			{
				//smoothing = 1.0f;
				target.position = new Vector3(swapperReference.GetComponent<Transform>().position.x, swapperReference.GetComponent<Transform>().position.y + 6.5f, swapperReference.GetComponent<Transform>().position.z - 5.0f); //tempX
			}
			else if(areaNo == 3)
			{
				target = GameObject.FindGameObjectWithTag("NarwhalCamera").GetComponent<Transform>();
			}


		}


	}
	// Update is called once per frame  whereas fixedUpdate is called based on one physics engine cycle
	void FixedUpdate () {
		Vector3 targetCamPos = target.position + offset;

		if (Vector3.Distance (transform.position, targetCamPos) > 50.0f)
			transform.position = targetCamPos;

		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);  // deltaTime is time it took to copmlete last frame

		// If camera goes past the lowY value, makes sure the camera stays at the lowY value
		//if (transform.position.y < lowY)
		//transform.position = new Vector3 (transform.position.x, lowY, transform.position.z);
	}

	public void setInBossArea(bool newStatus,int tempAreaNo)
	{
		if (tempAreaNo == 0)
			target = swapperReference.GetComponent<Transform> ();
		else if (tempAreaNo == 2)
			tempY = target.position.y + 6.5f;
		else if (tempAreaNo == 22 || tempAreaNo == 23)
			tempX = target.position.x;

		if (areaNo == 22 && tempAreaNo == 2) 
		{
			xOffset = 10;
			tempY = target.position.y;
		} 
		else if (areaNo == 23 && tempAreaNo == 2) 
		{
			xOffset = -10;
			tempY = target.position.y;
		}

		if (!newStatus)
			smoothing = smoothDefault;
		inBossArea = newStatus;
		areaNo = tempAreaNo;

	}

	public void setCutscenePlaying(bool b)
	{
		cutscenePlaying = b;
	}

	public bool getCutscenePlaying() 
	{
		return cutscenePlaying;
	}

	public int GetCurrentAreaNum()
	{
		return areaNo;
	}
}
