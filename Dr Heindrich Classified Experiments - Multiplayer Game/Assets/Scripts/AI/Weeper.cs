using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Weeper : MonoBehaviour
{

    public float distanceCanSee = 8f;
    Vector3 dirToPlayer;
    float dotProduct;
    float mag;
    bool seen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!seen) {
            seen = seeMee();
            if (seen) {
                Debug.Log("yay");
                GetComponent<Guard>().StopAllCoroutines();
                GetComponent<NavMeshAgent>().isStopped = true;
                GetComponent<Guard>().state = Guard.State.None;
            }
        } else if (seen) {
            seen = seeMee();
            if (!seen) {
                GetComponent<Guard>().ToDefault();
                GetComponent<NavMeshAgent>().isStopped = false;
            }
        }
    }
    bool seeMee() {

        foreach (GameObject target in GameObject.FindGameObjectsWithTag("Player")) {
            dirToPlayer = GetVec(new Vector3(target.transform.position.x, 0, target.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));

            dotProduct = (target.transform.forward.x * dirToPlayer.x) + (target.transform.forward.z * dirToPlayer.z);
            if (dotProduct > 0.8 && mag <= distanceCanSee) {

                Debug.Log("bruh");
                dirToPlayer = GetVec(target.transform.position + Vector3.up, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));
                RaycastHit hit;
                Debug.DrawRay(target.transform.position + Vector3.up, dirToPlayer * mag);
                if (Physics.Raycast(target.transform.position + Vector3.up, dirToPlayer, out hit, 20f)) {
                    if (hit.transform.gameObject == gameObject) {
                        Debug.DrawRay(target.transform.position + Vector3.up, dirToPlayer * mag, color:Color.green);
                        return true;
                    } else {
                        Debug.Log(hit.transform.name);
                    }
                }
            }
        }
        return false;
    }

    Vector3 GetVec(Vector3 org, Vector3 tar) {
        mag = Mathf.Sqrt(Mathf.Pow(tar.x - org.x, 2) + Mathf.Pow(tar.y - org.y, 2) + Mathf.Pow(tar.z - org.z, 2));
        return new Vector3(tar.x - org.x, tar.y - org.y, tar.z - org.z) / mag;
        //Vector3((tar.x - org.x)/mag, (tar.y - org.y)/mag,(tar.z - org.z)/mag);
    }
}
