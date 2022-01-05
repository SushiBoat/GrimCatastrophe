using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenuPanel;
    public GameObject saveLoadMenuPanel;
    public GameObject gameSavedPopup;

    public List<InputField> SaveSlotFields;

    public SaveManager saveManager = new SaveManager();
    private List<GameState> savedGames = new List<GameState>();

    private GameState currentGameState = new GameState();
    
    private bool menuOpen;

  

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && menuOpen == false)
        {
            Time.timeScale = 0;
            pauseMenuPanel.SetActive(true);

            menuOpen = true;
            
        }
    }

    //Pause Menu

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        menuOpen = false;
        Time.timeScale = 1.0f;
        Debug.Log("resumes");

    }

    public void SaveLoadMenu()
    {
        pauseMenuPanel.SetActive(false);
        saveLoadMenuPanel.SetActive(true);

        savedGames = saveManager.Load();

        UpdateSaveSlots();
      
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleMenu");
        menuOpen = false;
        Time.timeScale = 1.0f;
    }

    public void LoadCheckpoint()
    {
        SceneManager.LoadScene("main");
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Save/Load Menu

  /*  public void Save(int saveSlot)
    {
        
        currentGameState.setName(SaveSlotFields[saveSlot].text);
        savedGames[saveSlot] = currentGameState;
        saveManager.save();
        currentGameState = savedGames[saveSlot];

        gameSavedPopup.SetActive(true);
       
    }

    public void Load(int saveSlot)
    {
        currentGameState = savedGames[saveSlot];
    }

    public void Back()
    {
        pauseMenuPanel.SetActive(true);
        saveLoadMenuPanel.SetActive(false);

    }

    //Game Saved Popup

        public void dismissPopup()
    {
        gameSavedPopup.SetActive(false);
        Back();
    }


   */

    private void UpdateSaveSlots()
    {
        

       for(int i = 0; i < 3; i++)
        {
            if(savedGames[i] != null)
            {
                SaveSlotFields[i].text = savedGames[i].getName();
            }
        }
    }


   
    
   

}
