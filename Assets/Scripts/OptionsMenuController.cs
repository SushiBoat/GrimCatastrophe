using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour {
    public GameObject mainMenuPanel;
    public GameObject optionsMenuPanel;
    public SaveManager saveManager = new SaveManager();

    //This will temporarily track the music game object until the audioManager is updated to manage all sound
    public AudioSource audioManager;
    public Slider volMasterSlider;

   
    Config config;
   

	
	void Start () {
        //load from config to update sliders/settings	
        config = saveManager.LoadConfig();
        volMasterSlider.value = config.getVolMaster();
        
	}
	
    public void back()
    {
        //load saved config settings(to reverse changes is config is not saved)
        config = saveManager.LoadConfig();

        //reset sliders based on settings
        volMasterSlider.value = config.getVolMaster();


        optionsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        //Apply settings
        audioManager.volume = config.getVolMaster();
    }

    public void apply()
    {
        //update saved config settings
        saveManager.saveConfig();
        back();
    }

    public void setVolMaster(float newVol)
    {
        config.setVolMaster(newVol);
        audioManager.volume = config.getVolMaster();
    }
	
}
