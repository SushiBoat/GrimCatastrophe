using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCutscenes : MonoBehaviour {

	public bool OpeningCutscene;

	// Use this for initialization
	void Start () {
		if (OpeningCutscene)
			StartCoroutine (LoadMain ());
		else
			StartCoroutine (LoadMenu ());
	}

	IEnumerator LoadMain() {
		yield return new WaitForSeconds (56.0f);
		SceneManager.LoadScene ("main");
	}

	IEnumerator LoadMenu() {
		yield return new WaitForSeconds (65.0f);
		SceneManager.LoadScene ("TitleMenu");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
