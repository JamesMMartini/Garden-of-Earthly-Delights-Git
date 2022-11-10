using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLightToggle : MonoBehaviour
{
    [SerializeField] GameObject playerLamp;

    private void OnTriggerExit(Collider other)
    {
        if (playerLamp.activeInHierarchy)
        {
            playerLamp.SetActive(false);
        }
        else
        {
            playerLamp.SetActive(true);
        }
    }
}
