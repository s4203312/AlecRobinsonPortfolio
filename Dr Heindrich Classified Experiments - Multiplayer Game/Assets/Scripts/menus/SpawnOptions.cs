using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnOptions : MonoBehaviour {
    public GameObject spawnPrefab;
    public Transform spawnPTransform;
    public EventSystem eventSystem;

    public void Start() {
        eventSystem = EventSystem.current;
    }

    public void spawn() {
        Instantiate(spawnPrefab, (spawnPTransform == null ? transform.parent : spawnPTransform));
        
        eventSystem.SetSelectedGameObject(GameObject.Find("MmButton"));
    }
}