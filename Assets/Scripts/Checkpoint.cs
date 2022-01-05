using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    // Use this for initialization
    public SaveManager saveManager = new SaveManager();
    public GameManager gameManager = new GameManager();
    private List<GameState> savedGames = new List<GameState>();
    private GameState currentGameState = new GameState();
	Animator anim;
	private SwapPlayer swapper;

	void Start()
	{
		swapper = gameManager.Swapper.GetComponent<SwapPlayer>();
		anim = GetComponent<Animator>();
	}
   
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision");
		if (collision.CompareTag("Cat")|| collision.CompareTag("Fish") || collision.CompareTag("Bird"))
        {
			if (collision.CompareTag("Fish") || collision.CompareTag("Bird"))
				swapper.ForceCatMorph ();
			anim.SetTrigger ("activate");

            //Retrieve Updated GameState from Game Manager
            gameManager.OverwriteGameState();
            currentGameState = gameManager.getGameState();
            currentGameState.setName(System.DateTime.Now.ToString());

            //Retrieve List of Saved Games 
            savedGames = saveManager.Load();
            //Overwrite Savegame at given SaveSlot
            savedGames[PlayerPrefs.GetInt("SaveSlot")] = currentGameState;
            
            saveManager.save();
            

           // Debug.Log("Saved");
        }

    }
}
