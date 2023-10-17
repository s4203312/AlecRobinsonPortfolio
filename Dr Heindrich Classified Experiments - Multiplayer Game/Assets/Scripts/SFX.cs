
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour {

    private void Awake() {
        Handler.SfxAddMe(GetComponent<AudioSource>());
    }
    private void OnDestroy() {
        Handler.SfxRemoveMe(GetComponent<AudioSource>());
    }
}