using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other) {
        if (transform.parent.GetComponent<EndDoorManager>().active) {
            if (other.tag == "Player") {
                Handler.ChangeScene(transform.parent.GetComponent<EndDoorManager>().nextScene);
            }
        }
    }



    public void Test() {
        Debug.Log("Test");
    }
}

