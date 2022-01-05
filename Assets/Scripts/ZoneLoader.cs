using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class ZoneLoader : MonoBehaviour {

    public GameObject underground1;
    public GameObject underground2;
    private GameObject fishBoss;
    private GameObject birdBoss;
    private GameObject narwhalBoss;

    // Use this for initialization
    void Start () {
        underground1.SetActive(false);
        underground2.SetActive(false);
        fishBoss = GameObject.Find("FishBoss");
        birdBoss = GameObject.Find("BirdBoss");
        narwhalBoss = GameObject.Find("NarwhalBoss");
    }
	
	// Update is called once per frame
	public void toggleZone(string zone, bool status)
    {
        switch (zone)
        {
            case "underground1":
                underground1.SetActive(status);
                break;


            case "underground2":
                underground2.SetActive(status);
                break;

            
        }
    }

  
}
