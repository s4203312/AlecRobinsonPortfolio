using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlidingBlockPuzzle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform end;
    public UnityEvent done;
    public void Move() {
        GameObject activator = GameObject.Find(PlayerName.playerName);

        Vector3 relativePosition = new Vector3(0f, 0f, 0f);
        relativePosition.x = activator.transform.position.x - transform.position.x;
        relativePosition.z = activator.transform.position.z - transform.position.z;
        relativePosition *= -1;
        relativePosition.Normalize();
        relativePosition.x += transform.position.x;
        relativePosition.z += transform.position.z;

        Debug.Log("0");
        Collider[] hitColliders;
        hitColliders =  Physics.OverlapSphere(relativePosition, 0.35f);
        if(hitColliders.Length == 1) {
            Debug.Log("1");
            GameObject oppisiteObj = hitColliders[0].gameObject;
            if (oppisiteObj.name == "SlidingPuzzleTile" || oppisiteObj.name == "SlidingPuzzleTIle") {
                relativePosition = oppisiteObj.transform.position;
                oppisiteObj.transform.position = transform.position;
                transform.position = relativePosition;
                if (oppisiteObj.transform == end) {
                    done.Invoke();
                    gameObject.tag = "Untagged";
                    Destroy(GetComponent<Interact>());
                    oppisiteObj.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Floor");
                    Destroy(this);
                }
            }
        }
    }
}
