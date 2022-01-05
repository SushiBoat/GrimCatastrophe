using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossCameraSwapper : MonoBehaviour
{
    cameraFollow2DPlatformer cref;
	public GameObject player;
    public int zone;
	public GameObject exit;
	public Image bossUI;
	public float cutsceneEndTime;
	bool startCutscenePlaying = false;
	bool startCutsceneTriggered = false;
	bool startCutsceneDone = false;
	public RectTransform letterbox;
	public TutorialSign sign;
	public GameObject music;
	AudioSource[] musicArray;
	AudioSource ambientMusic;
	AudioSource bossMusic;

	private SwapPlayer swapper;

    void Start()
    {
        cref = GameObject.Find("Main Camera").GetComponent<cameraFollow2DPlatformer>();

		swapper = player.GetComponentInChildren<SwapPlayer> ();

		musicArray = music.GetComponents<AudioSource> ();
		ambientMusic = musicArray [0];
		bossMusic = musicArray [1];

		if (zone == 1) 
		{
			sign.gameObject.SetActive (false);
		}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Cat") || other.CompareTag("Fish") || other.CompareTag("Bird"))
        {
			if ((zone == 1 && swapper.fishUnlocked) || (zone == 2 && swapper.birdUnlocked)) 
			{
				//Debug.Log ("DESTROY");
				Destroy (gameObject);
				return;
			}

			//if (cref.GetCurrentAreaNum() != zone)
				cref.setInBossArea(true,zone);
            
			if (zone == 1 && startCutscenePlaying == false) 
			{
				ambientMusic.Stop ();
				bossMusic.Play ();
				cref.setCutscenePlaying (true);
				startCutscenePlaying = true;
				letterbox.GetComponent<Animator> ().SetBool ("started", true);
				if (other.CompareTag ("Fish") || other.CompareTag ("Bird"))
					swapper.startedCutscene ();
				player.GetComponentInChildren<CatPlayerController> ().SetInputEnabled (false);
				GameObject.FindGameObjectWithTag ("FishBossCamera").GetComponent<Animator> ().SetBool ("play", true);
				exit.GetComponent<Collider2D> ().isTrigger = false;

			}

			if (zone == 2) 
			{
				gameObject.GetComponent<BoxCollider2D> ().offset += new Vector2 (0.0f, 23.0f);
				if (startCutscenePlaying == false && !startCutsceneTriggered) 
				{
					ambientMusic.Stop ();
					bossMusic.Play ();
					startCutsceneTriggered = true;
					cref.setCutscenePlaying (true);
					startCutscenePlaying = true;
					letterbox.GetComponent<Animator> ().SetBool ("started", true);
					player.GetComponentInChildren<CatPlayerController> ().SetInputEnabled (false);
					if (other.CompareTag ("Fish") || other.CompareTag ("Bird"))
						swapper.startedCutscene ();
					GameObject.FindGameObjectWithTag ("BirdBossCamera").GetComponent<Animator> ().SetBool ("play", true);
					exit.GetComponent<Collider2D> ().isTrigger = false;	
				}
			}

			if (zone == 3 && !startCutscenePlaying && !startCutsceneDone) 
			{
				ambientMusic.Stop ();
				bossMusic.Play ();
				cref.setCutscenePlaying (true);
				startCutscenePlaying = true;
				letterbox.GetComponent<Animator> ().SetBool ("started", true);
				if (other.CompareTag ("Fish") || other.CompareTag ("Bird"))
					swapper.startedCutscene ();
				player.GetComponentInChildren<CatPlayerController> ().SetInputEnabled (false);
				GameObject.FindGameObjectWithTag ("NarwhalCamera").GetComponent<Animator> ().SetBool ("play", true);
				exit.GetComponent<Collider2D> ().isTrigger = false;
			}
        }
    }

	/*
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Cat") || other.CompareTag("Fish") || other.CompareTag("Bird"))
        {
            cref.setInBossArea(true,zone);
        }
    }
	
    void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.CompareTag("Cat") || other.CompareTag("Fish") || other.CompareTag("Bird"))
        {
			if (zone == 2) 
			{
				cref.setInBossArea(true, 22);
			}
        }
    }
    */

	void Update()
	{
		if (startCutscenePlaying && !startCutsceneDone) 
		{
			cutsceneEndTime -= Time.deltaTime;
			if (cutsceneEndTime <= 3 && cutsceneEndTime > 0) {
				bossUI.gameObject.SetActive (true);
				letterbox.GetComponent<Animator>().SetTrigger ("end");
				letterbox.GetComponent<Animator> ().SetBool ("started", false);
			}
			if (cutsceneEndTime <= 0) 
			{	
				startCutscenePlaying = false;
				swapper.endCutscene ();
				player.GetComponentInChildren<CatPlayerController> ().SetInputEnabled (true);
				if (zone == 1) {
					cutsceneEndTime = 5;
				} else if (zone == 2) {
					cutsceneEndTime = 3;
					GameObject.FindGameObjectWithTag ("BirdBossCamera").GetComponent<Animator> ().enabled = false;
				} else if (zone == 3) {
					startCutsceneDone = true;
				}
				cref.setCutscenePlaying (false);
			}
		}


		float bossHP;
		bossHP = bossUI.GetComponentInChildren<Slider> ().value;
		if (bossHP == 0) 
		{
			if (zone == 1) {
				drainPools ();
				GameObject.FindGameObjectWithTag ("FishBossCamera").GetComponent<Animator> ().SetBool ("end", true);
			} else if (zone == 2) {
				GameObject.FindGameObjectWithTag ("BirdBossCamera").GetComponent<Animator> ().enabled = true;
				GameObject.FindGameObjectWithTag ("BirdBossCamera").GetComponent<Animator> ().SetBool ("end", true);
			} else if (zone == 3) {
				//SceneManager.LoadScene ("EndingCutscene");
				Destroy (gameObject);
				return;
			}
			player.GetComponentInChildren<CatPlayerController> ().SetInputEnabled (false);
			letterbox.GetComponent<Animator> ().SetBool ("started", true);
			cutsceneEndTime -= Time.deltaTime;
			if (cutsceneEndTime <= 0) 
			{
				bossMusic.Stop ();
				ambientMusic.Play ();
				letterbox.GetComponent<Animator>().SetTrigger ("end");
				letterbox.GetComponent<Animator> ().SetBool ("started", false);
				player.GetComponentInChildren<CatPlayerController> ().SetInputEnabled (true);
				bossUI.gameObject.SetActive (false);
				exit.GetComponent<Collider2D> ().isTrigger = true;
				if (zone == 1) 
					sign.gameObject.SetActive (true);
				else if (zone == 2)
					cref.setInBossArea(false, 0);
				Destroy (gameObject);
				return;
			}
		}
	}

	void drainPools()
	{
		GameObject.FindGameObjectWithTag ("Pool1").GetComponent<Pool> ().ActivateandDestroy ();
		GameObject.FindGameObjectWithTag ("Pool2").GetComponent<Pool> ().ActivateandDestroy ();
		GameObject.FindGameObjectWithTag ("Pool3").GetComponent<Pool> ().ActivateandDestroy ();
	}
}
