using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class can be converted into a persistent save file
[System.Serializable]
public class GameState {

    private string name;
	private float playerX = -79.02f;
    private float playerY = -8.0f;
    private float playerZ = 0;
    private bool fishBoss = true;
    private bool birdBoss = true;
    private int birdPhase = 1;
    private int birdCheckpoint = 0;
    private bool narwhal = true;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public string getName()
    {
        return name;
    }

    public void setName(string newName)
    {
        name = newName;
    }

    public Vector3 getPlayerPos()
    {
        return new Vector3(playerX, playerY, playerZ);
    }

    public bool checkFishBoss()
    {
        return fishBoss;
    }

    public bool checkBirdBoss()
    {
        return birdBoss;
    }

    public bool checkNarwhal()
    {
        return narwhal;
    }

    public void updateGameState(Vector3 newPos, bool tempFishBoss, bool tempBirdBoss, bool tempNarwhal, int birdBossCheckpoint, int birdBossPhase)
    {
        playerX = newPos.x;
        playerY = newPos.y;
        playerZ = newPos.z;
        fishBoss = tempFishBoss;
        birdBoss = tempBirdBoss;
        birdCheckpoint = birdBossCheckpoint;
        birdPhase = birdBossPhase;
        narwhal = tempNarwhal;
    }

    public int getBirdBossPhase()
    {
        return birdPhase;
    }

    public int getBirdBossCheckpoint()
    {
        return birdCheckpoint;
    }

    

   
}
