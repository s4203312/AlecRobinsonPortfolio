using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool done = false;
    [SerializeField] private Material floor;

    /*private void Awake() {
        GetComponent<MeshRenderer>().material = floor;
    }*/
    private void OnTriggerEnter(Collider other) {
        if (!done) {
            if (other.tag == "Player") {
                GameObject.Find("SceneManager").GetComponent<SceneManager>().SetCheckPoint(transform);
                done = true;
            }
        }
    }
}