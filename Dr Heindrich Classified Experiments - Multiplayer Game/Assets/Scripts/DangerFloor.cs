using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerFloor : MonoBehaviour
{
    public void ButtonPressed()
    {
        foreach (Collider i in gameObject.GetComponentsInChildren<BoxCollider>())
        {
            i.enabled = false;
        }
    }
}
