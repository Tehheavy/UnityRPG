using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    public List<GameObject> enemies;
    public Optimizer Fog;
    public GameObject[] enemyResources;
    // Start is called before the first frame update
    void Start()
    {
        enemyResources = Resources.LoadAll<GameObject>("Prefabs/Enemies");
        if (enemyResources == null)
        {
            Debug.LogError("Enemy Pathing: enemyResources NULL");
        }
        if (enemyResources[0] == null)
        {
            Debug.Log("Enemy Pathing:NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
