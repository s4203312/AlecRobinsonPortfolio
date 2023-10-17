using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private GameObject playerSmall;
    private GameObject playerBig;

    private Vector3 avaragepos;
    private float distanceBetween;
    private bool inRange = true;
    private bool lerping = false;

    private void Start() {
        playerSmall = GameObject.Find("PlayerSmall");
        playerBig = GameObject.Find("PlayerBig");
    }

    private void Update()
    {
        avaragepos = (playerSmall.transform.position + playerBig.transform.position) / 2 + new Vector3(0, 10, 0);
        distanceBetween = Vector3.Distance(playerSmall.transform.position, playerBig.transform.position);

        if (distanceBetween < 13) {
            if (inRange) {
                transform.position = avaragepos;
            } else {
                if (!lerping) {
                    StartCoroutine(backToAvg());
                }
            }
            
        } else {
            inRange = false;
            if (lerping) {
                StopAllCoroutines();
                lerping = false;
            }
        }
    }

    IEnumerator backToAvg() {
        lerping = true;
        Vector3 strPos = transform.position;
        float time = 0;
        while (time < 1) {
            transform.position = Vector3.Lerp(strPos, avaragepos, time);
            time += Time.deltaTime / 0.4f;
            yield return null;
        }
        inRange = true;
        lerping = false;
    }
}