using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Guard : MonoBehaviour
{
    [SerializeField] private bool patrol;
    private int patrolI = 0;
    [SerializeField] private Vector3 startLocation;
    [SerializeField] private Quaternion startRotation;

    private AudioSource aud;
    private EnemySight eyes;
    public enum State {
        None,idle,chase,move,search,patrol,stunned
    }

    public State state = State.None;

    private void Awake() {
        if(state == 0) {
            ToDefault();
        }
        aud = GetComponent<AudioSource>();
        eyes = transform.Find("Head").GetComponent<EnemySight>();
        if(!patrol) { startLocation = transform.position; startRotation = transform.rotation;  }
    }
    private void Update() {
        aud.pitch = Mathf.Lerp(0, 1.5f, Mathf.InverseLerp(0, 5, GetComponent<NavMeshAgent>().velocity.magnitude));
        if (state != State.None)
            stateMachine();
    }

    void stateMachine()
    {
        if (state != State.chase && state != State.stunned)
        {
            if (eyes.CanSeePlayer())
            {
                GetComponent<Animator>().SetBool("Movement", true);
                changeState(State.chase);
                GetComponent<NavMeshAgent>().speed = 5f;
                transform.Find("Head").GetComponent<AudioSource>().Play();
                StopAllCoroutines();
                StartCoroutine(updateTarget());
            }
        }
        switch (state)
        {
            case State.idle:
                break;
            case State.chase:
                Vector3 dirToPlayer = GetVec(transform.position, eyes.playerSee.transform.position);
                Debug.DrawRay(transform.position, dirToPlayer, UnityEngine.Color.red);
                if (((transform.forward.x * dirToPlayer.x) + (transform.forward.z * dirToPlayer.z)) > 0)
                {
                    transform.Find("Head").transform.LookAt(eyes.playerSee.transform.position + Vector3.up);
                }
                if (!eyes.CanSeePlayer())
                {
                    changeState(State.move);
                    GetComponent<NavMeshAgent>().speed = 2.5f;
                    StopAllCoroutines();
                }

                break;
            case State.move:
                if (GetComponent<NavMeshAgent>().remainingDistance == 0){
                    transform.Find("Head").transform.rotation = new Quaternion();
                    changeState(State.search);
                    StartCoroutine(Search());
                }
                break;
            case State.search:
                break;
            case State.patrol:
                if (GetComponent<NavMeshAgent>().remainingDistance == 0){
                    if (++patrolI == transform.parent.childCount)
                    {
                        patrolI = 1;
                    }
                    GetComponent<NavMeshAgent>().SetDestination(transform.parent.GetChild(patrolI).transform.position);
                }
                break;
        }
    }
    public void ToDefault()
    {
        if (patrol)
        {
            changeState(State.patrol);
            GetComponent<Animator>().SetBool("Movement", true);
        }
        else
        {
            changeState(State.idle);
            StartCoroutine(ToStartPosRot());
            GetComponent<Animator>().SetBool("Movement", false);
        }

    }


    Vector3 GetVec(Vector3 org, Vector3 tar) {
        return new Vector3(tar.x - org.x, tar.y - org.y, tar.z - org.z);
    }
    IEnumerator updateTarget() {
        while (true) {
            GetComponent<NavMeshAgent>().SetDestination(eyes.playerSee.transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator Search() {
        float time = 0;
        while (time < 5)
        {
            transform.Find("Head").Rotate(0f, 500f * Time.deltaTime, 0f);
            time += Time.deltaTime;
            yield return null;
        }
        transform.Find("Head").transform.rotation = new Quaternion();
        ToDefault();
    }

    IEnumerator Stunned() {
        GetComponent<Animator>().SetBool("Movement", false);
        float time = 0;
        while (time < 5) {
            time += Time.deltaTime;
            if(time >= 4)
            {
                GetComponent<Animator>().SetInteger("State",1);
            }
            yield return null;
        }
        transform.Find("Spot Light").GetComponent<Light>().enabled = true;
        GetComponent<NavMeshAgent>().isStopped = false;
        ToDefault();
    }

    IEnumerator ToStartPosRot()
    {
        while(transform.position.x != startLocation.x || transform.position.z != startLocation.z)
        {
            if (GetComponent<NavMeshAgent>().remainingDistance == 0)
            {
                GetComponent<NavMeshAgent>().SetDestination(startLocation);
            }
            yield return new WaitForSeconds(1f);
        }
        if (transform.rotation != startRotation)
        {
            Quaternion curRot = transform.rotation;
            float time = 0;
            while (time <= 1)
            {
                time += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(curRot,startRotation,time);
                yield return null;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            GameObject.Find("SceneManager").GetComponent<SceneManager>().Reset();
        }
    }

    public void Stun() {
        StopAllCoroutines();
        transform.Find("Spot Light").GetComponent<AudioSource>().Play();
        transform.Find("Spot Light").GetComponent<Light>().enabled= false;
        changeState(State.stunned);
        GetComponent<Animator>().SetTrigger("Stun");
        GetComponent<NavMeshAgent>().isStopped = true;
        StartCoroutine(Stunned());
    }

    public void Distract(Vector3 location) {
        if(state != State.chase) {
            StopAllCoroutines();
            GetComponent<NavMeshAgent>().SetDestination(location);
            StartCoroutine(Stunned());
            changeState(State.stunned);
        }
    }

    void changeState(State newState) {
        state = newState;
        GetComponent<Animator>().SetInteger("State",(int)state);
    }
}
