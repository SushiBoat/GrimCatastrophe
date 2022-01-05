using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterTrigger : MonoBehaviour {
    FishPlayerController fishController;
    CatPlayerController catController;
    BirdPlayerController birdController;
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Cat"))
        {
            //Debug.Log("player entered the trigger");
            GameObject Cat = GameObject.FindGameObjectWithTag("Cat");
            catController = Cat.GetComponent<CatPlayerController>();
            catController.setIfInWater(true);
        }

        else if (other.CompareTag("Fish"))
        {
            GameObject fish = GameObject.FindGameObjectWithTag("Fish");
            fishController = fish.GetComponent<FishPlayerController>();
            fishController.setIfInWater(true);
        }

        else if (other.CompareTag("Bird"))
        {
            //Debug.Log("player is inside trigger");
            GameObject Bird = GameObject.FindGameObjectWithTag("Bird");
            birdController = Bird.GetComponent<BirdPlayerController>();
            birdController.setIfInWater(true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Cat"))
        {
            //Debug.Log("player is inside trigger");
            GameObject Cat = GameObject.FindGameObjectWithTag("Cat");
            catController = Cat.GetComponent<CatPlayerController>();
            catController.setIfInWater(true);
        }

        else if (other.CompareTag("Fish"))
        {
            GameObject fish = GameObject.FindGameObjectWithTag("Fish");
            fishController = fish.GetComponent<FishPlayerController>();
            fishController.setIfInWater(true);
        }
        
        else if (other.CompareTag("Bird"))
        {
            //Debug.Log("player is inside trigger");
            GameObject Bird = GameObject.FindGameObjectWithTag("Bird");
            birdController = Bird.GetComponent<BirdPlayerController>();
            birdController.setIfInWater(true);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cat"))
        {
            //Debug.Log("player left the trigger");
            GameObject Cat = GameObject.FindGameObjectWithTag("Cat");
            catController = Cat.GetComponent<CatPlayerController>();
            catController.setIfInWater(false);
        }

       else if (other.CompareTag("Fish"))
        {
            GameObject fish = GameObject.FindGameObjectWithTag("Fish");
            fishController = fish.GetComponent<FishPlayerController>();
            fishController.setIfInWater(false);
        }

        else if (other.CompareTag("Bird"))
        {
            //Debug.Log("player is inside trigger");
            GameObject Bird = GameObject.FindGameObjectWithTag("Bird");
            birdController = Bird.GetComponent<BirdPlayerController>();
            birdController.setIfInWater(false);
        }
    }
}
