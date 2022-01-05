using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject mainMenuPanel;
    public GameObject loadMenuPanel;
    public GameObject newGamePanel;
    public GameObject optionsPanel;

    public List<InputField> SaveSlotFields;
    public List<Button> loadButtons;
    public List<Button> overwriteButtons;

    public SaveManager saveManager = new SaveManager();
    private List<GameState> savedGames = new List<GameState>();

    private GameState currentGameState = new GameState();

    //Main Menu

    public void NewGame()
    {
        LoadMenuPanel();
      
        for(int i = 0; i < 3; i++)
        {
            loadButtons[i].gameObject.SetActive(false);
            overwriteButtons[i].gameObject.SetActive(true);
        }
      
        
    }

    public void LoadGame()
    {
        //loadMenuPanel.SetActive(true);
        // mainMenuPanel.SetActive(false);

        LoadMenuPanel();

        for (int i = 0; i < 3; i++)
        {
            overwriteButtons[i].gameObject.SetActive(false);
            loadButtons[i].gameObject.SetActive(true);
        }
        PlayerPrefs.SetInt("NewGame", 0);
    }

    public void Continue() {
        SceneManager.LoadScene("main");
        PlayerPrefs.SetInt("NewGame", 0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void UpdateSaveSlots()
    {


        for (int i = 0; i < 3; i++)
        {
            if (savedGames[i] != null)
            {
                SaveSlotFields[i].text = savedGames[i].getName();
            }
        }
    }

    public void selectSaveSlot(int saveSlot)
    {
        PlayerPrefs.SetInt("SaveSlot", saveSlot);
        PlayerPrefs.SetInt("NewGame", 1);
        SceneManager.LoadScene("OpeningCutscene");
    }

    private void LoadMenuPanel()
    {
        mainMenuPanel.SetActive(false);
        loadMenuPanel.SetActive(true);

        savedGames = saveManager.Load();

        UpdateSaveSlots();
    }

    public void options()
    {
        
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

   

    //Load Menu

    /* public void LoadSave() {
         savedGames = saveManager.Load();



     }

     public void DeleteSave() {

     }

     public void Back()
     {
         mainMenuPanel.SetActive(true);
         loadMenuPanel.SetActive(false);
     }
     */





}
