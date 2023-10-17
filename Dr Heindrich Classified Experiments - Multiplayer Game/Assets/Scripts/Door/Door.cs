using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool active = false;
    public int numOfButtons = 2;

    private void Update() {
        if (numOfButtons == 0) {
            if (!active) {
                GetComponent<Animator>().SetTrigger("Open");
            }
            active = true;
        }
    }
}
