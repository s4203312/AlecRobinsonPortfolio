using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour{
    [SerializeField]
    private InputActionReference playerInputs;      //Creating variables for the interactions
    float pointerInputValue;
    bool interact;
    private Camera cam;
    private float distance = 10f;
    
    private GameObject fakewall;     //Used in the target system
    private int targetsHit = 0;  

    private GameObject platform;        //Used in the explosion and platform system
    Explosion explode;

    private void Awake(){
        playerInputs.action.performed += context => pointerInputValue = context.action.ReadValue<float>();
    }

    private void Start(){
        cam = Camera.main;      //Setting the cam variable to be the main camera
        if (Cursor.lockState != CursorLockMode.Locked)      //Locks the cursor into the screen
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update(){
        if (pointerInputValue == 1){        //Checks if we are looking at somthing
            if (interact == false){         //Checks if we are currently interacting with somthing
                Interact();                 //Calls the interact function
            }
        } else if (pointerInputValue == 0){
            interact = false;
        }
    }

    void Interact(){
        interact = true;        //Sets interact to true to stop interaction while we interact with this object

        RaycastHit hit; 
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance)){     //Sets out the raycast and finds what it hits. Also sets the range
            Debug.Log("Interacted with " + hit.transform.name);
            if (hit.transform.CompareTag("Door")){                                                  //Checks if the ray hit the door
                hit.transform.GetComponentInParent<Animator>().SetTrigger("UpdateState");           //Calls the door animation
                hit.transform.GetComponentInParent<AudioSource>().Play();
            } else if (hit.transform.CompareTag("Target")){
                hit.transform.GetComponentInParent<Animator>().SetTrigger("UpdateState");
                targetsHit = targetsHit + 1;
                TargetCheck();
            } else if (hit.transform.CompareTag("Gun")){
                distance = 100f;                                                                    //Sets the distance to be larger as gun is collected
                GameObject.Destroy(GameObject.Find("Gun"));
                GameObject.Destroy(GameObject.Find("GunTrigger"), 10f);
            } else if (hit.transform.CompareTag("Button2")){
                platform = GameObject.Find("Platform");                                                     //Moves the platform
                platform.transform.GetComponentInParent<Animator>().SetTrigger("UpdateState");
                explode = GameObject.FindGameObjectWithTag("Explosion").GetComponent<Explosion>();          //Calls the explosion
                explode.StartExposion();
            }
        }
    }

    void TargetCheck(){                                                           //A test to see if all three targets have been knocked down
        if (targetsHit == 3){
            fakewall = GameObject.Find("FakeWall");
            fakewall.transform.GetComponentInParent<Animator>().SetTrigger("UpdateState");
        }
    }
}
