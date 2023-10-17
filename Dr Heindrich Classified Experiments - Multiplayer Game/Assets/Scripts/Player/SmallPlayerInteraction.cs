using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPlayerInteraction : MonoBehaviour
{
    [SerializeField] GameObject outSpawn;
    int distractDistance = 4;

    public void Vent()
    {
        if (PlayerName.playerName == "PlayerSmall")
        {
            GameObject playerSmall = GameObject.Find("PlayerSmall");
            playerSmall.transform.position = outSpawn.transform.position;
        }
    }

    public void Knock() {
        if (PlayerName.playerName == "PlayerSmall") {
            //GetComponent<AudioSource>().Play();
            GameObject playerSmall = GameObject.Find("PlayerSmall");
            Collider[] colliders = Physics.OverlapBox(playerSmall.transform.position, new Vector3(distractDistance, 1, distractDistance), Quaternion.identity);
            if (colliders.Length != 0) {
                foreach (Collider collider in colliders) {
                    if (collider.gameObject.GetComponent<Guard>() != null) {
                        collider.gameObject.GetComponent<Guard>().Distract(transform.position - (Vector3.forward * 2));
                    }
                }
            }
        }
    }
}

