using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public float distanceCanSee = 8f;
    public GameObject playerSee;
    Vector3 dirToPlayer;
    float dotProduct;
    float mag;
    LineRenderer line;
    // Start is called before the first frame update
    void Start() {
        //Player = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(1.25f, 0));
        line = GetComponent<LineRenderer>();
    }


    public bool CanSeePlayer() {
        //if (Player == null) {
        //    Player = GameObject.FindGameObjectWithTag("Player").transform;
        //}
        foreach (GameObject target in GameObject.FindGameObjectsWithTag("Player")) {
            dirToPlayer = GetVec(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.transform.position.x, 0, target.transform.position.z));
            
            dotProduct = (transform.forward.x * dirToPlayer.x) + (transform.forward.z * dirToPlayer.z);
            if (dotProduct > 0.8) {
                if (mag <= distanceCanSee) {
                    dirToPlayer = GetVec(transform.position, new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));
                    RaycastHit hit;
                    //Debug.DrawRay(transform.position, dirToPlayer * mag);
                    if (Physics.Raycast(transform.position, dirToPlayer, out hit, 20f)) {
                        if (hit.transform.gameObject == target) {
                            Debug.DrawRay(transform.position, dirToPlayer * mag);
                            playerSee = target;
                            return true;
                        } else {
                            Debug.Log(hit.transform.name);
                        }
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
