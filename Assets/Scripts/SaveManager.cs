using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager
{

    List<GameState> savedGames = new List<GameState>();
    Config config = new Config();


    //Save Game Progress
    public void save()
    {
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, savedGames);
        Debug.Log("Saved to " + Application.persistentDataPath + "/gamesave.save");
        file.Close();
    }

    //Save Game Settings
    public void saveConfig()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/config.dat");
        bf.Serialize(file, config);
        Debug.Log("Saved to " + Application.persistentDataPath + "/config.dat");
        file.Close();



    }

    //Load Game Progress
    public List<GameState> Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            savedGames = (List<GameState>)bf.Deserialize(file);
            file.Close();

            return savedGames;
        }
        else
        {
            Debug.Log("No Save Available");
            for (int i = 0; i < 3; i++)
            {
                savedGames.Add(new GameState());
            }
            return savedGames;
        }

    }
    //Load Game Settings
    public Config LoadConfig() {
        if (File.Exists(Application.persistentDataPath + "/config.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/config.dat", FileMode.Open);
            config = (Config)bf.Deserialize(file);
            file.Close();
            return config;
        }
        else
        {
            Debug.Log("Config not found");
            config = new Config();
            return config;
           }
    }


    public void reset()
    {
        if(File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            File.Delete(Application.persistentDataPath + "/gamesave.save");
        }

        if(File.Exists(Application.persistentDataPath + "/config.dat"))
        {
            File.Delete(Application.persistentDataPath + "/config.dat");
        }
    }

}
