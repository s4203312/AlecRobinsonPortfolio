using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoorManager : MonoBehaviour
{
    public string nextScene;
    public bool active = false;
    public int numOfButtons = 2;

    private void Update() {
        if (numOfButtons == 0) {
            if (!active) {
                foreach (Animator i in gameObject.GetComponentsInChildren<Animator>()) {
                    i.SetTrigger("Open");
                }
            }
            active = true;
        }
    }

    public void ButtonDone() {
        numOfButtons--;
    }
}
