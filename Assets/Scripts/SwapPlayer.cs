using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapPlayer : MonoBehaviour {
    [Range(0,10)]
    public float morphBar;//bar representing time left for morphing
    public float coolDownTicker; //time to reduce per second for morphbar when it is running
    public float coolDownRefillMultiplier; //Multiplier for cooldown to refill
    float morphBarMax;
    bool morphBarRunning;
    bool canMorph;
    public GameObject cat;
    public GameObject bird;
    public GameObject fish;
    string characterName;

	// LV
	Animator MyAnim;
	SpriteRenderer MySprite;
	public Canvas worldSpaceUI;
	public Slider miniCooldownSlider;
	public Slider cooldownSlider;
	Image cooldownSliderImage;
	Sprite cooldownSliderSpriteDefault;
	Color defaultMiniCDColor;
	public Sprite cooldownSliderImageOff;
    public Slider healthSlider;
	public Image scytheUI;
	public bool fishUnlocked;
	public bool birdUnlocked;
	bool inCutscene;

    //Changes to merge lives into one
    public int startinglives;
    public int playerLives;
    [Range(0, 20)]
    public float damageCooldown; //Time in seconds you want between each time player takes damage, i.e. invulnerability period
    float damageTimer;


    // Use this for initialization
    void Start () {
        //initial character
        characterName = "Cat";

        //disable all secondary characters
        cat.SetActive(true);
        bird.SetActive(false);
        fish.SetActive(false);

        //initialise morphBar variables
        morphBarMax = morphBar;
        morphBarRunning = false;
        canMorph = true;

		// LV
		MyAnim = GetComponent<Animator>();
		MySprite = GetComponent<SpriteRenderer> ();
		MySprite.enabled = false;
		cooldownSlider.value = morphBarMax;
		cooldownSliderImage = cooldownSlider.GetComponentInChildren<Image>();
		cooldownSliderSpriteDefault = cooldownSliderImage.sprite;
		fishUnlocked = false;
		birdUnlocked = false;

        playerLives = startinglives;
        damageTimer = damageCooldown;

		healthSlider.value = playerLives;
		defaultMiniCDColor = miniCooldownSlider.GetComponentInChildren<Image> ().color;

		inCutscene = false;
    }

	// Update is called once per frame
	void Update () {
		/*
        if (Input.GetKeyDown(KeyCode.J))
        {
            catMorph();
        }

        if (Input.GetKeyDown(KeyCode.K) && canMorph == true && fishUnlocked)
        {
            fishMorph();
        }

        if (Input.GetKeyDown(KeyCode.L) && canMorph == true && birdUnlocked)
        {
            birdMorph();
        }
		*/

		if (!inCutscene) {
			if ((Input.GetKeyDown(KeyCode.J) && characterName == "Fish" ) || (Input.GetKeyDown(KeyCode.K) && characterName == "Bird"))
			{
				if (FindObjectOfType<AudioManager>() != null)
					FindObjectOfType<AudioManager>().Play ("transform");
				catMorph();
			}
			else if (Input.GetKeyDown(KeyCode.J) && canMorph == true && fishUnlocked && characterName != "Fish")
			{
				if (FindObjectOfType<AudioManager>() != null)
					FindObjectOfType<AudioManager>().Play ("transform");
				fishMorph();
			}
			else if (Input.GetKeyDown(KeyCode.K) && canMorph == true && birdUnlocked && characterName != "Bird")
			{
				if (FindObjectOfType<AudioManager>() != null)
					FindObjectOfType<AudioManager>().Play ("transform");
				birdMorph();
			}
		}

        if (characterName == "Cat")
        {
            this.transform.position = cat.transform.position;
            morphBarRunning = false;
        }

        else if(characterName == "Bird")
        {
            this.transform.position = bird.transform.position;
            morphBarRunning = true;
            if(morphBar >= 0.01)
            {
                morphBar -= coolDownTicker * Time.deltaTime;
            }
        }

        else if (characterName == "Fish")
        {
            this.transform.position = fish.transform.position;
            morphBarRunning = true;
            if (morphBar >= 0.01)
            {
                morphBar -= coolDownTicker * Time.deltaTime;
            }
        }

        morphBarLogic();

    }


    void catMorph()
    {
        if (characterName == "Fish")
        {
            cat.GetComponent<Rigidbody2D>().velocity = new Vector2(fish.GetComponent<Rigidbody2D>().velocity.x, fish.GetComponent<Rigidbody2D>().velocity.y);
            cat.transform.position = fish.transform.position;
            // LV
            transform.position = cat.transform.position;
            MySprite.enabled = true;
            MyAnim.SetTrigger("transform");
        }
        if (characterName == "Bird")
        {
            cat.GetComponent<Rigidbody2D>().velocity = new Vector2(bird.GetComponent<Rigidbody2D>().velocity.x, bird.GetComponent<Rigidbody2D>().velocity.y);
            cat.transform.position = bird.transform.position;
            // LV
            transform.position = cat.transform.position;
            MySprite.enabled = true;
            MyAnim.SetTrigger("transform");
        }

        characterName = "Cat";
        cat.SetActive(true);
        bird.SetActive(false);
        fish.SetActive(false);
    }

    void birdMorph()
    {
        if (characterName == "Cat")
        {
            bird.GetComponent<Rigidbody2D>().velocity = new Vector2(cat.GetComponent<Rigidbody2D>().velocity.x, cat.GetComponent<Rigidbody2D>().velocity.y);
            bird.transform.position = cat.transform.position;
            // LV
            transform.position = bird.transform.position;
            MySprite.enabled = true;
            MyAnim.SetTrigger("transform");
        }
        if (characterName == "Fish")
        {
            bird.GetComponent<Rigidbody2D>().velocity = new Vector2(fish.GetComponent<Rigidbody2D>().velocity.x, fish.GetComponent<Rigidbody2D>().velocity.y);
            bird.transform.position = fish.transform.position;
            // LV
            transform.position = cat.transform.position;
            MySprite.enabled = true;
            MyAnim.SetTrigger("transform");
        }

        characterName = "Bird";
        cat.SetActive(false);
        bird.SetActive(true);
        fish.SetActive(false);
    }

    void fishMorph()
    {
        if (characterName == "Cat")
        {
            fish.GetComponent<Rigidbody2D>().velocity = new Vector2(cat.GetComponent<Rigidbody2D>().velocity.x, cat.GetComponent<Rigidbody2D>().velocity.y);
            fish.transform.position = cat.transform.position;
            // LV
            transform.position = bird.transform.position;
            MySprite.enabled = true;
            MyAnim.SetTrigger("transform");
        }
        if (characterName == "Bird")
        {
            fish.GetComponent<Rigidbody2D>().velocity = new Vector2(bird.GetComponent<Rigidbody2D>().velocity.x, bird.GetComponent<Rigidbody2D>().velocity.y);
            fish.transform.position = bird.transform.position;
            // LV
            transform.position = cat.transform.position;
            MySprite.enabled = true;
            MyAnim.SetTrigger("transform");
        }

        characterName = "Fish";
        cat.SetActive(false);
        bird.SetActive(false);
        fish.SetActive(true);
    }

    void morphBarLogic()
    {
        if (morphBar < morphBarMax && morphBarRunning == false) // morphbar is recharging
        {
			cooldownSliderImage.sprite = cooldownSliderImageOff;
			miniCooldownSlider.GetComponentInChildren<Image> ().color = Color.gray;
            morphBar += coolDownRefillMultiplier * coolDownTicker * Time.deltaTime;
            canMorph = false;
        }

        else if (morphBar >= morphBarMax)  // morphbar is back to full
        {
			cooldownSliderImage.sprite = cooldownSliderSpriteDefault;
			miniCooldownSlider.GetComponentInChildren<Image> ().color = defaultMiniCDColor;
			miniCooldownSlider.gameObject.SetActive (false);
            canMorph = true;
        }

        else if (morphBar <= 0.01) // morphbar is empty
        {
            catMorph();
            canMorph = false;
        }

		//LV
		cooldownSlider.value = morphBar;
		if (morphBarRunning) 
		{
			if (!miniCooldownSlider.IsActive ()) 
			{
				miniCooldownSlider.gameObject.SetActive (true);
			}
		}
		miniCooldownSlider.value = morphBar;
		worldSpaceUI.transform.position = this.transform.position;


    }

    public string getCharacterName()
    {
        return characterName;
    }

	public void unlockFish(){
		fishUnlocked = true;
		scytheUI.GetComponent<SytheUIScript>().unlockFish ();
	}

	public void unlockBird(){
		birdUnlocked = true;
		scytheUI.GetComponent<SytheUIScript>().unlockBird ();

	}

	public void Heal() {
		cat.GetComponent<CatPlayerController> ().HealPlayer ();
		fish.GetComponent<FishPlayerController> ().HealPlayer ();
		bird.GetComponent<BirdPlayerController> ().HealPlayer ();

        
        if (characterName == "Cat") {
			healthSlider.value = cat.GetComponent<CatPlayerController> ().GetLives ();
		} else if (characterName == "Fish") {
			healthSlider.value = fish.GetComponent<FishPlayerController> ().GetLives ();
		} else if (characterName == "Bird") {
			healthSlider.value = bird.GetComponent<BirdPlayerController> ().GetLives ();
		}
        
	}



    public int GetLives()
    {
        return playerLives;
    }

    public void SetLives(int changeLivesBy)
    {
        playerLives += changeLivesBy;
        healthSlider.value = playerLives;
    }

    public void DamagePlayer(int livesToDamageBy)
    {
		if (FindObjectOfType<AudioManager>() != null)
			FindObjectOfType<AudioManager>().Play ("hurt");
        damageTimer = damageCooldown;
        playerLives = playerLives - livesToDamageBy;
        healthSlider.value = playerLives;
    }

    public void HealPlayer()
    {
        playerLives = startinglives;
		healthSlider.value = playerLives;
    }

    public float getDamageCooldown()
    {
        return damageCooldown;
    }

	public void startedCutscene()
	{
		inCutscene = true;
		catMorph ();
		return;
	}

	public void endCutscene()
	{
		inCutscene = false;
	}

	public void ForceCatMorph()
	{ 
		catMorph (); 
	}
}


