using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSign : MonoBehaviour {

	TextMesh text;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<TextMesh> ();
		text.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag ("Cat") || collision.gameObject.CompareTag ("Bird") || collision.gameObject.CompareTag ("Fish")) 
		{
			text.gameObject.SetActive (true);
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag ("Cat") || collision.gameObject.CompareTag ("Bird") || collision.gameObject.CompareTag ("Fish")) 
		{
			text.gameObject.SetActive (false);
		}
	}
}
