using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Threading;

public class PlayerControls : MonoBehaviour {

    private Controls playerControls;            //Getting the class of the action map for the controls of the game
    //Speed Variables
    [SerializeField] private float playerMovementSpeed;     //Movement speed of player
    [SerializeField] private float playerRotationSpeed;     //Rotational speed of player
    //Movement Variables
    Vector2 postionVector;
    //Rotation Variables
    Quaternion currentRotation;
    Vector2 controllerRotation;
    Quaternion rotationTarget;
    //Interaction Variables
    float interaction;
    [SerializeField] private Camera cam;
    private float distance = 2f;
    bool interact;
    //EyeSwap Variables
    float eyesSwap;
    //Spawn Point
    private GameObject spawnPoint;
    //Event system
    public EventSystem eventSystem;
    public GameObject spawnPrefab;
    public Transform spawnPTransform;
    GameObject pauseMenu;
    bool opened;


    private void Awake(){
        playerControls = new Controls();
        playerControls.Player.Enable();                                 //Enabling the player movement action map

        eventSystem = EventSystem.current;

        //playerControls.Player.Move.performed += PlayerMoved;          //Used for the debugging
        //playerControls.Player.Look.performed += PlayerLooked;
    }

    private void Update(){
        //Movement
        transform.position += new Vector3(postionVector.x, 0, postionVector.y) * Time.deltaTime * playerMovementSpeed;
        //Movement Animation
        GetComponentInChildren<Animator>().SetBool("Running", postionVector.magnitude != 0);

        //Rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, Time.deltaTime * playerRotationSpeed);     //Rotating torwards target
        
        //Interaction
        if (interaction == 1) {        //Checks if user pressed on somthing
            if (interact == false) {         //Checks if we are currently interacting with somthing
                interact = true;        //Sets interact to true to stop interaction while we interact with this object

                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance)) {     //Sets out the raycast and finds what it hits. Also sets the range
                    Debug.Log("Interacted with " + hit.transform.name);
                    if (hit.transform.CompareTag("Interactable")) {                                                  //Checks if the ray hit the vent
                         PlayerName.playerName = transform.name;                                                   //Find which player interacted
                        hit.transform.GetComponent<Interact>().interaction.Invoke();
                        if (hit.transform.GetComponent<Guard>())
                        {
                            GetComponentInChildren<Animator>().SetTrigger("Punch");
                        }
                        else
                        {
                            GetComponentInChildren<Animator>().SetTrigger("Button");
                        }
                    }
                }
            }
        }
        else if (interaction == 0) {
            interact = false;
        }

        //EyeSwap
        if (eyesSwap == 1) {        //Checks if user pressed on somthing
            Debug.Log(transform.name);
            if (transform.name == "PlayerSmall")
            {
                transform.Find("Eye").GetComponent<Light>().enabled = false;
                GameObject.Find("PlayerBig").GetComponentInChildren<Light>().enabled = true;
            }
            else
            {
                transform.Find("Eye").GetComponent<Light>().enabled = false;
                GameObject.Find("PlayerSmall").GetComponentInChildren<Light>().enabled = true;
            }


        }
    }

    public void Movement(InputAction.CallbackContext context)
    {
        //Movement  (Left JoyStick)
        if (context.performed)
        {
            postionVector = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            postionVector = new Vector2(0,0);
        }
    }
    public void Rotation(InputAction.CallbackContext context)
    {
        //Looking around  (Right JoyStick)
        if (context.performed)
        {
            //currentRotation = transform.rotation;
            controllerRotation = context.ReadValue<Vector2>();
            Vector3 playerDirection = Vector3.right * controllerRotation.x + Vector3.forward * controllerRotation.y;
            if (playerDirection.sqrMagnitude > 0.0f)        //If user is moving the controls
            {
                rotationTarget = Quaternion.LookRotation(playerDirection, Vector3.up);           //Finding target rotation
            }
        }
    }
    public void Interaction(InputAction.CallbackContext context) {
        if (context.performed) {
            interaction = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            interaction = 0;
        }
    }
    public void EyeSwap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eyesSwap = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            eyesSwap = 0;
        }
    }

    public void Options(InputAction.CallbackContext context) {
        if (context.performed) {
            if(opened != true) {
                playerControls.Player.Disable();

                spawnPTransform = GameObject.FindGameObjectWithTag("MainCanvas").transform;
                pauseMenu = Instantiate(spawnPrefab, (spawnPTransform == null ? transform.parent : spawnPTransform));
                eventSystem.SetSelectedGameObject(GameObject.Find("MmButton"));
                opened = true;
            }
            else {
                playerControls.Player.Enable();

                Destroy(pauseMenu);
                opened = false;
            }
        }
    }

    //public PlayerControls JoinPlayer(int playerIndex = null, int splitScreenIndex = null, string controlScheme = null, InputDevice pairWithDevice = null)

    //public void OnSpawn(){
    //    spawnPoint = GameObject.Find("SpawnPoint");
    //    transform.position = spawnPoint.transform.position;
    //}

    //Used to test if inputs are working

    // public void PlayerMoved(InputAction.CallbackContext context){
    //     Debug.Log(context);
    // }
    // public void PlayerLooked(InputAction.CallbackContext context){
    //     Debug.Log(context);
    // }
}

//Rotation attmepts
//Vector3 playerDirection = Vector3.right * controllerRotation.x + Vector3.forward * controllerRotation.y;
        //if(playerDirection.sqrMagnitude > 0.0f){                //If user is moving the controls
        //    rotationTarget = Quaternion.LookRotation(playerDirection,Vector3.up);           //Finding target rotation
        //    transform.rotation = Quaternion.RotateTowards(currentRotation,rotationTarget,Time.deltaTime * playerRotationSpeed);     //Rotating torwards target
        //}

        //currentRotation = transform.rotation;
        //rotationVector = playerControls.Player.Look.ReadValue<Vector2>();
        //rotationTarget = Quaternion.LookRotation(new Vector3(0,rotationVector.y,0),Vector3.up);
        //transform.rotation = Quaternion.RotateTowards(currentRotation,rotationTarget,Time.deltaTime * playerRotationSpeed);

        //transform.rotation = Quaternion.Euler(new Vector3(0,rotationY * Time.deltaTime * playerRotationSpeed,0));

        //Quaternion.LookRotation(new Vector3(0,0,0), new Vector3(0,rotationVector.y * Time.deltaTime * playerMovementSpeed,0));

        //transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(0,rotationVector.y * Time.deltaTime * playerMovementSpeed,0));

        //currentAngle += new Vector3(0, rotationVector.y, 0) * Time.deltaTime * playerMovementSpeed;
        //currentRotation.eulerAngles = currentAngle;
        //transform.rotation = currentRotation;

