using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] GameObject door;

    bool active = false;
    public void ActivateCurotine() {
        if (!active) {
            door.GetComponent<Door>().numOfButtons--;
            active = true;
        }
        StartCoroutine(Activate());
    }
    IEnumerator Activate(float duration = 0.2f) {
        while (duration > 0) {
            duration -= Time.deltaTime;
            yield return null;
        }
        door.GetComponent<Door>().numOfButtons++;
        active = false;
    }
}
