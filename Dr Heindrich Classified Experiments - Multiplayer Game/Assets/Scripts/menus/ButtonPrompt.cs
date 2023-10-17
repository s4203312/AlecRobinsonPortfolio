using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPrompt : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        transform.Find("Canvas").Find("Text").gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other) {
        transform.Find("Canvas").Find("Text").gameObject.SetActive(false);
    }
}
