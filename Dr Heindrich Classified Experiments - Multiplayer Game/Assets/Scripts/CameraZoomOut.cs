using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] int amountIn = 0;
    // Start is called before the first frame update
    LayerMask layerMask;
    void Start()
    {
        layerMask = 1 << 7;  //layer mask for layer 7
        layerMask |= 1 << 8; // adding layer 8
    }

    // Update is called once per frame
    void Update()
    {
        //Collider[] colliders = Physics.OverlapBox(GetComponent<BoxCollider>().center + transform.position
            //, GetComponent<BoxCollider>().size, new Quaternion(), layerMask);
        //amountIn = colliders.Length;
    }
    private void OnTriggerEnter(Collider other) {
        exitEnter();
    }
    private void OnTriggerExit(Collider other) {
        exitEnter();
    }
    void exitEnter() {
        Collider[] colliders = Physics.OverlapBox( GetComponent<BoxCollider>().center + transform.position
            , GetComponent<BoxCollider>().size/2, new Quaternion(),layerMask);
        /*foreach(Collider collider in colliders) {
            Debug.Log(collider.name);
        }*/
        if (colliders.Length == 2) {
            if (!active) { State(true); }
        } else if (colliders.Length == 0) {
            if(active) { State(false); }
        }
        amountIn = colliders.Length;
    }

    void State(bool state) {
        active= state;
        transform.GetChild(1).gameObject.SetActive(state);
        Camera.main.transform.GetComponent<CameraFollowPlayer>().enabled = !state;
        Camera.main.transform.position = transform.GetChild(0).position;
    }
}
