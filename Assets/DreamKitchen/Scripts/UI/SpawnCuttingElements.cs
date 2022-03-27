using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCuttingElements : MonoBehaviour
{
    [SerializeField] private GameObject cuttingElementPrefab;
    [SerializeField] private float fRespawnTime;
    private Vector2 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // setting spawning area
        StartCoroutine(CuttingElementsWave()); // starting coroutine
    }


    private void SpawnCuttingElement()
    {
        GameObject go = Instantiate(cuttingElementPrefab) as GameObject; // instantiating a prefab
        go.transform.position = new Vector2(screenBounds.x, Random.Range(-screenBounds.y, screenBounds.y)); // with random position on Y
    }

    //creating a coroutine to make function to be called once in few seconds
    IEnumerator CuttingElementsWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(fRespawnTime);
            SpawnCuttingElement(); // spawn elements
        }
    }
}
