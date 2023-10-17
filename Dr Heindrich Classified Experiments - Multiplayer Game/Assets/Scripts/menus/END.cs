using System;
using TMPro;
using UnityEngine;

public class END : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("Time").GetComponent<TMP_Text>().text = DateTime.Now.Subtract(Handler.strt).ToString().Substring(0, 8);
    }
}
