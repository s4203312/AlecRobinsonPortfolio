using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorLaser : MonoBehaviour
{
    public void Open()
    {
        Debug.Log("OPen lase door");foreach (Animator i in gameObject.GetComponentsInChildren<Animator>())
            {
                i.SetTrigger("Open");
                GetComponent<AudioSource>().Play();
                
            }GetComponent<OffMeshLink>().enabled = true;
        //if (transform.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LevelDoor"))
        //{
            
        //}
    }
}