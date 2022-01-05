using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveMenuController : MonoBehaviour {


    public GameObject MainMenuPanel;
    public GameObject saveLoadMenuPanel;
    public GameObject gameSavedPopup;

    public List<InputField> SaveSlotFields;

    

    public SaveManager saveManager = new SaveManager();
    private List<GameState> savedGames = new List<GameState>();

    private GameState currentGameState = new GameState();

    private bool menuOpen;

    public void Save(int saveSlot)
    {
        savedGames = saveManager.Load();

        currentGameState.setName(SaveSlotFields[saveSlot].text);
        savedGames[saveSlot] = currentGameState;
        saveManager.save();
        currentGameState = savedGames[saveSlot];

        gameSavedPopup.SetActive(true);

    }

    public void UpdateGamestate(GameState newGameState)
    {
        currentGameState = newGameState;
    }

    public void Load(int saveSlot)
    {
        if (saveSlot >= 0)
        {
            PlayerPrefs.SetInt("NewGame", 0);
            savedGames = saveManager.Load();

            currentGameState = savedGames[saveSlot];
           

            PlayerPrefs.SetInt("SaveSlot", saveSlot);
        }
        else { currentGameState = new GameState(); }

    }

    public void TitleLoad(int saveSlot)
    {
        PlayerPrefs.SetInt("SaveSlot", saveSlot);
        SceneManager.LoadScene("Main");
    }

    public GameState getGamestate()
    {
        return currentGameState;
    }

    public void Back()
    {
        MainMenuPanel.SetActive(true);
        saveLoadMenuPanel.SetActive(false);

    }

    //Game Saved Popup

    public void dismissPopup()
    {
        gameSavedPopup.SetActive(false);
        Back();
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

    public void Reset()
    {
        saveManager.reset();
    }
}
