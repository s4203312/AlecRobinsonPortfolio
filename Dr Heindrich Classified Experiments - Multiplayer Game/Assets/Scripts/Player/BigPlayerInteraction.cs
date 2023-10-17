using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigPlayerInteraction : MonoBehaviour {

    public bool inArea;

    public void Door() {
        if (PlayerName.playerName == "PlayerBig") {
            if (inArea) {
                foreach (Animator i in gameObject.GetComponentsInChildren<Animator>()) {
                    i.SetTrigger("Open");
                    GetComponent<AudioSource>().Play();
                    GetComponent<OffMeshLink>().enabled = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider player) {
        if (player.gameObject.name == "PlayerBig") {
            inArea = true;
        }
    }
    private void OnTriggerExit(Collider player) {
        if (player.gameObject.name == "PlayerBig") {
            if (transform.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Open")) {
                transform.parent.GetComponent<AudioSource>().Play();
                GetComponent<OffMeshLink>().enabled = false;
                foreach (Animator i in gameObject.GetComponentsInChildren<Animator>()) {
                    i.SetTrigger("Close");
                }
            }
            inArea = false;
        }
    }

    public void Guard() {
        if (PlayerName.playerName == "PlayerBig") {
            gameObject.GetComponent<Guard>().Stun();
        }
    }

    public void Moss() {
        if (PlayerName.playerName == "PlayerBig") {
            Debug.Log(gameObject + "111");
            Destroy(gameObject);
        }
    }
}
