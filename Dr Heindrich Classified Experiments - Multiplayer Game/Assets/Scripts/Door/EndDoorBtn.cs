using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoorBtn : MonoBehaviour
{
    //[SerializeField] GameObject

    bool active = false;
    float duration = 0;
    public void ActivateCurotine() {
        duration = 0.2f;
        if (!active) {
            GameObject.Find("EndDoorMan").GetComponent<EndDoorManager>().numOfButtons--;
            active = true;
            StartCoroutine(Activate());
        }
    }
    IEnumerator Activate() {
        while(duration > 0) {
            duration -= Time.deltaTime;
            yield return null;
        }
        GameObject.Find("EndDoorMan").GetComponent<EndDoorManager>().numOfButtons++;
        active = false;
    }

    public void Test() {
        GameObject.Find("EndDoorMan").GetComponent<EndDoorManager>().numOfButtons--;
    }
}
