using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCutscene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (LoadMenu ());
	}

	IEnumerator LoadMenu() {
		yield return new WaitForSeconds (23.0f);
		SceneManager.LoadScene ("TitleMenu");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
