using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Buttons : MonoBehaviour
{
    const string PLAYER_TAG = "Player";     //Setting the tag player as a constant variable as we know it wont change
    const string BLOCK_TAG = "Block";     //Setting the tag block as a constant variable as we know it wont change
    
    public UnityEvent ButtonON;       //Creating two of my own unity events
    public UnityEvent ButtonOFF;
    
    private void OnTriggerEnter(Collider other){        //Entering the trigger collider
        if(other.CompareTag(BLOCK_TAG) || other.CompareTag(PLAYER_TAG)){
            Debug.Log("Button On");
            ButtonON.Invoke();
        }
    }
    private void OnTriggerExit(Collider other){         //Exiting the trigger collider
        if (other.CompareTag(BLOCK_TAG) || other.CompareTag(PLAYER_TAG)){
            Debug.Log("Button Off");
            ButtonOFF.Invoke();
        }
    }
}
