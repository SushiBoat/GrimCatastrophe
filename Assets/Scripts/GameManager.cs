using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum ManagerState { Initiate,Spawn,Running,GameOver}
public class GameManager : MonoBehaviour {
    //This class manages game information while running, relies on the GameState class for persistant storage
    GameState LastCheckpoint;
    public GameObject Player;
    public GameObject Swapper;
	public GameObject Camera;
    public SaveMenuController saveController;

    public GameObject fishBoss;
	public Slider fishHealthSlider;

    public GameObject birdBoss;
	public Slider birdHealthSlider;

    public GameObject narwhal;
	public Slider narwhalHealthSlider;

    public GameObject gameOverScreen;
    public Vector3 birdBossSpawn;
    public Vector3 narwhalBossSpawn;
    public Vector3 playerSpawn;
    public ZoneLoader zoneLoader;
    private Vector3 fishBossSpawn;

    private ManagerState state = ManagerState.Initiate;
     
    //These check which bosses have been defeated/which powers have been unlocked
    private bool fishBossCheck;
    private bool birdBossCheck;
    private int birdBossCheckpoint = 0;
    private int birdBossPhase = 1;
    private bool narwhalCheck;

	private bool givenFishPower;
	private bool givenBirdPower;

   

    

	// Use this for initialization
	void Start () {
        //Game should not update until spawning is complete
        Time.timeScale = 0;
		givenFishPower = false;
		givenBirdPower = false;
	}
	
	// Update is called once per frame
	void Update () {
       // Debug.Log(PlayerPrefs.GetInt("SaveSlot"));
        
        switch (state)
        {
            case ManagerState.Initiate:
                
                if (PlayerPrefs.GetInt("NewGame") != 1)
                {
                    saveController.Load(PlayerPrefs.GetInt("SaveSlot"));
                    LastCheckpoint = saveController.getGamestate();
                    playerSpawn = LastCheckpoint.getPlayerPos();
					Camera.transform.position = new Vector3 (playerSpawn.x, playerSpawn.y, Camera.transform.position.z);
                }
                else {LastCheckpoint = new GameState(); }
                saveController.Load(PlayerPrefs.GetInt("SaveSlot"));
                fishBossCheck = LastCheckpoint.checkFishBoss();
                birdBossCheck = LastCheckpoint.checkBirdBoss();
                birdBossCheckpoint = LastCheckpoint.getBirdBossCheckpoint();
                birdBossPhase = LastCheckpoint.getBirdBossPhase();
                narwhalCheck = LastCheckpoint.checkNarwhal();
                state = ManagerState.Spawn;
                

                break;

		case ManagerState.Spawn:
               // Debug.Log("Spawn");
                //Debug.Log(LastCheckpoint.getPlayerPos().x);
                //Player = Instantiate(Player, LastCheckpoint.getPlayerPos(),Quaternion.identity);
			Player.transform.position = playerSpawn;
			if (LastCheckpoint.checkFishBoss ()) {
				fishBoss = Instantiate (fishBoss, new Vector3 (38, -16, 0), Quaternion.identity);
				fishBoss.gameObject.GetComponent<FishBoss> ().setPlayerTarget (Swapper.transform);
				fishBoss.gameObject.GetComponent<FishBoss> ().healthSlider = fishHealthSlider;
	                   
			} else {
				Swapper.GetComponent<SwapPlayer>().unlockFish();
				givenFishPower = true;	
			}
			if (LastCheckpoint.checkBirdBoss()) {
				birdBoss.gameObject.GetComponent<BirdBoss> ().healthSlider = birdHealthSlider;
                    if (birdBossCheckpoint > 0)
                    {
                        if (birdBossPhase == 1)
                        {
                            birdBossSpawn = birdBoss.gameObject.GetComponent<BirdBoss>().getCheckpointPosition(birdBossCheckpoint - 1);
                           
                        }
                        if (birdBossPhase == 2)
                        {
                            birdBossSpawn = birdBoss.gameObject.GetComponent<BirdBoss>().getBirdPosts(0);
                        }
                      }
                    birdBoss = Instantiate(birdBoss, birdBossSpawn, Quaternion.identity);
                    birdBoss.gameObject.GetComponent<BirdBoss>().setPlayerTarget(Swapper.transform);
                    birdBoss.gameObject.GetComponent<BirdBoss>().setPhase(birdBossPhase);
                    birdBoss.gameObject.GetComponent<BirdBoss>().setCheckpoint(birdBossCheckpoint);

                } else {
				Swapper.GetComponent<SwapPlayer>().unlockBird();
				givenBirdPower = true;
			}
            if (LastCheckpoint.checkNarwhal())
            {
                narwhal = Instantiate(narwhal, narwhalBossSpawn, Quaternion.identity);
                narwhal.gameObject.GetComponent<NarwhalBoss>().setPlayerTarget(Swapper.transform);
				narwhal.gameObject.GetComponent<NarwhalBoss> ().healthSlider = narwhalHealthSlider;
            }


               
                Time.timeScale = 1;
                state = ManagerState.Running;

                break;
			case ManagerState.Running:
				if (Swapper.GetComponent<SwapPlayer>().GetLives() <= 0) {
					state = ManagerState.GameOver;
				}
				if (!fishBoss && givenFishPower == false) {
					Swapper.GetComponent<SwapPlayer> ().unlockFish ();
					Swapper.GetComponent<SwapPlayer> ().HealPlayer ();
					givenFishPower = true;	
				}
				if (!birdBoss && givenBirdPower == false) {
					Swapper.GetComponent<SwapPlayer> ().unlockBird ();
					Swapper.GetComponent<SwapPlayer> ().HealPlayer ();
					givenBirdPower = true;
				}
				if (!narwhal) {
					SceneManager.LoadScene("EndingCutscene");
				}

                break;
            case ManagerState.GameOver:
                //Display Game Over
                gameOverScreen.SetActive(true);
                Time.timeScale = 0.0f;
               
                break;
        }
    }



    public void LoadGamestate(GameState newState)
    {
        LastCheckpoint = newState;
    }

    //Attach to savebutton onclick listener to run before saveMethod
    public void OverwriteGameState()
    {
        //Debug.Log("Overwrite");
        if (!fishBoss)
        {
            fishBossCheck = false;
        }

        if (!birdBoss)
        {
            birdBossCheck = false;
        }
        else
        {
            birdBossPhase = birdBoss.gameObject.GetComponent<BirdBoss>().getPhase();
            birdBossCheckpoint = birdBoss.gameObject.GetComponent<BirdBoss>().getCheckpoint();
            
        }


        if (!narwhal)
        {
            narwhalCheck = false;
        }

        LastCheckpoint.updateGameState(Player.transform.position, fishBossCheck, birdBossCheck, narwhalCheck,birdBossCheckpoint,birdBossPhase);

        saveController.UpdateGamestate(LastCheckpoint);
    }

    public void reload()
    {

        SceneManager.LoadScene("main");
        
        Time.timeScale = 1.0f;
    }

    public GameState getGameState()
    {
        return LastCheckpoint;
    }

    

   
}
