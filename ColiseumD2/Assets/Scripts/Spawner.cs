using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Coliseum;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] bonus;
    public Vector3 spawnValues;
    public float spawnwait;
    public int startwait;
    public bool stopSpawning;

    private int randbonus;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitSpawner());
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    IEnumerator waitSpawner()
    {
        yield return new WaitForSeconds(startwait);
        while (true)
        {
            randbonus = Random.Range(0, 3);
           Vector3 spawnPosition = new Vector3(spawnValues.x, spawnValues.y, spawnValues.z);
           Instantiate(bonus[randbonus], spawnPosition, gameObject.transform.rotation);
           yield return new WaitForSeconds(spawnwait);
        }
    }
}
