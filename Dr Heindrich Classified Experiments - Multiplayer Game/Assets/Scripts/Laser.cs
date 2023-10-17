using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //public int Bouces = 5;
    //public int aBounces;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 lastPos = transform.position;
        Vector3 lastDir = transform.forward;
        bool done = false;
        line.positionCount = 0;
        while (!done) {
            RaycastHit hit;
            if (Physics.Raycast(lastPos, lastDir, out hit, 30f)) {
                Vector3[] newPoss;
                newPoss = new Vector3[line.positionCount];
                line.GetPositions(newPoss);
                if (line.positionCount < 2) {
                    line.positionCount += 2;
                    line.SetPositions(conCat(newPoss, new Vector3[2] { lastPos, lastPos + (lastDir * hit.distance) }));
                } else {
                    line.positionCount += 1;
                    line.SetPositions(conCat(newPoss, new Vector3[1] { lastPos + (lastDir * hit.distance) }));
                }
                lastPos += (lastDir * hit.distance);
                lastDir = Vector3.Reflect(lastDir, hit.normal);
                //Debug.Log(hit.transform.GetComponent<Renderer>().material.name.Substring(0, 10));
                if(hit.transform.name == "LaserGoHere") {
                    hit.transform.GetComponent<Interact>().interaction.Invoke();
                }
                if (hit.transform.GetComponent<Renderer>() != null && hit.transform.GetComponent<Renderer>().material.name.Substring(0,10) == "Reflective"){
                    //Debug.Log("reflect");
                }
                else {done = true;}
            } else { done = true; }
        }
    }

    Vector3[] conCat(Vector3[] first, Vector3[] second) {
        Vector3[] newArray = new Vector3[first.Length + second.Length];
        System.Array.Copy(first, newArray, first.Length);
        System.Array.Copy(second, 0, newArray, first.Length, second.Length);
        return newArray;
    }
}