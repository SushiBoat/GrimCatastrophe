using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour {

    public ZoneLoader zoneLoader;
    public string zoneName;

   
    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.CompareTag("Cat") || other.CompareTag("Fish") || other.CompareTag("Bird"))
        {
         zoneLoader.toggleZone(zoneName, true);
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cat") || other.CompareTag("Fish") || other.CompareTag("Bird"))
        {
            zoneLoader.toggleZone(zoneName, false);
        }

    }
}
