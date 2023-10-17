using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UITrigger : MonoBehaviour
{
    const string PLAYER_TAG = "Player";     //Setting the tag player as a constant variable as we know it wont change

    public UnityEvent TriggerEnter;       //Creating two of my own unity events
    public UnityEvent TriggerExit;
    
    private void OnTriggerEnter(Collider other){        //Entering the trigger collider
        if(other.CompareTag(PLAYER_TAG)){
            Debug.Log("Entered Trigger");
            TriggerEnter.Invoke();
        }
    }
    private void OnTriggerExit(Collider other){         //Exiting the trigger collider
        if (other.CompareTag(PLAYER_TAG)){
            Debug.Log("Exited Trigger");
            TriggerExit.Invoke();
        }
    }
}
