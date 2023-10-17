using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float power = 300f;
    private float radius = 5f;
    [SerializeField] private GameObject particals;
    private GameObject source;

    public void StartExposion()     //Used to allow me to call the coroutine from another script
    {
        StartCoroutine("Explode");
    }

    private IEnumerator Explode(){
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);       //Checking whats in the area so the program knows what to effect
            foreach (Collider hit in colliders){
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.AddExplosionForce(power, transform.position, radius, 3.0f);      //Explosion code
            }
            }

            source = GameObject.Find("Explosion");                      //Playing the sound effect
            source.transform.GetComponentInParent<AudioSource>().Play();  

            particals.transform.GetComponentInParent<ParticleSystem>().Play();      //Creating a partical effect
        yield return null;
    }
}
