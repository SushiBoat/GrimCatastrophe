using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SytheUIScript : MonoBehaviour {
	
	Image scytheImage;
	public Sprite scytheFish;
	public Sprite scytheBird;

	// Use this for initialization
	void Start () {
		scytheImage = GetComponent<Image>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void unlockFish(){
		scytheImage.sprite = scytheFish;
	}

	public void unlockBird(){
		scytheImage.sprite = scytheBird;
	}
}
