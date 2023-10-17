using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DangerLasers : MonoBehaviour
{
    // Start is called before the first frame update
    public void buttonPressed() {
        Destroy(GetComponent<BoxCollider>());
        Debug.Log(transform.Find("Cylinders").name);
        for(int i = transform.Find("Cylinders").childCount; i > 0; i--) {
            Destroy(transform.Find("Cylinders").GetChild(i-1).gameObject);
        }
        GetComponent<OffMeshLink>().activated= true;
    }
    private void OnTriggerEnter(Collider other) {
        GameObject.Find("SceneManager").GetComponent<SceneManager>().Reset();
    }
}
