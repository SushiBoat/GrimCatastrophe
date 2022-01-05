using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class can be converted into a persistent save file
[System.Serializable]
public class Config
{

    //Volume
    private float volMaster = 1.0f;
    private float volMusic;
    private float volEffects;

    private bool windowedMode;

    //Keybindings


   
    public void setVolMaster(float newVol)
    {
        volMaster = newVol;
    }

   

    public float getVolMaster()
    {
        return volMaster;
    }


}
