using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimizer : MonoBehaviour
{

    public GameObject player;
    public GameObject[][] mapArray;
    public MapLoad map;
    public float viewRange;
    private bool clearFlag = false;
    public EnemyPathing enemyPathing;
    private bool spawnFlag = false;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // for (int i = 0; i < map.mapSize; i++)
        // {
        //     for (int j = 0; j < map.mapSize; j++)
        //     {
        //         if (map.mapArrayObjects[i][j] != null)
        //         {

        //             if (Vector3.Distance(map.mapArrayObjects[i][j].transform.position, player.transform.position) > viewRange)
        //             {
        //              //   Debug.Log("Destroy test inside i j:" + i + " " + j);
        //                 Destroy(map.mapArrayObjects[i][j]);
        //             }
        //         }
        //         else
        //         {
        //              if (Vector3.Distance(new Vector3(i + (float)0.5, 0, j + (float)0.5), player.transform.position) <= viewRange)
        //             {
        //               //  Debug.Log("CREATE test inside 2 i j:" + i + " " + j);
        //                 map.mapArrayObjects[i][j] = Instantiate(map.mapChunk, new Vector3(i + (float)0.5, 0, j + (float)0.5), Quaternion.identity);
        //                 if(map.mapArraySpriteNumber[i][j]!=-1)
        //                 {
        //                    map.mapArrayObjects[i][j].transform.Find("MapChunkSprite").gameObject.GetComponent<SpriteRenderer>().sprite = map.tempSprite[map.mapArraySpriteNumber[i][j]];
        //                 }
        //             }
        //         }
        //     }
        // }



        for (int i = (int)(player.transform.position.x - viewRange); i < (int)(player.transform.position.x + viewRange); i++)
        {
            for (int j = (int)(player.transform.position.z - viewRange); j < (int)(player.transform.position.z + viewRange); j++)
            {
                if (i < map.mapSize && i >= 0 && j < map.mapSize && j >= 0)
                {
                    if (map.mapArrayObjects[i][j] == null)
                    {
                        if (Vector3.Distance(new Vector3(i + (float)0.5, 0, j + (float)0.5), player.transform.position) <= viewRange)
                        {
                            //  Debug.Log("CREATE test inside 2 i j:" + i + " " + j);
                            map.mapArrayObjects[i][j] = Instantiate(map.mapChunk, new Vector3(i + (float)0.5, 0, j + (float)0.5), Quaternion.identity);
                            if (map.mapArraySpriteNumber[i][j] != -1)//-1 means default
                            {
                                map.mapArrayObjects[i][j].transform.Find("MapChunkSprite").gameObject.GetComponent<SpriteRenderer>().sprite = map.tempSprite[map.mapArraySpriteNumber[i][j]];
                            }
                            int random = Random.Range(0, 100);
                            if (map.mapArray[i][j] == 0 && random == 0)
                            {
                                int random2 = Random.Range(0, 2);
                                GameObject enemy = Instantiate(enemyPathing.enemyResources[random2], new Vector3(i + (float)0.5, 0.5f, j + (float)0.5), Quaternion.identity);
                                EnemyAI test = enemy.GetComponent<EnemyAI>();
                                test.player = player;
                                test.tempMap = map;
                                test.enemyPathing=enemyPathing;
                                enemyPathing.enemies.Add(enemy);

                            }
                        }
                    }
                }
            }
        }

        for (int i = (int)(player.transform.position.x - (viewRange + 1)); i < (int)(player.transform.position.x + (viewRange + 1)); i++)
        {
            for (int j = (int)(player.transform.position.z - (viewRange + 1)); j < (int)(player.transform.position.z + (viewRange + 1)); j++)
            {
                if (i < map.mapSize && i >= 0 && j < map.mapSize && j >= 0)
                {
                    //   Debug.Log("optmizer i j:"+i+" "+j+" mapsize: "+map.mapSize);
                    if (map.mapArrayObjects[i][j] != null)
                    {

                        if (Vector3.Distance(map.mapArrayObjects[i][j].transform.position, player.transform.position) > viewRange)
                        {
                            //   Debug.Log("Destroy test inside i j:" + i + " " + j);
                            Destroy(map.mapArrayObjects[i][j]);
                            // for (int element = enemyPathing.enemies.Count - 1; element >= 0; element--)
                            // {
                            //     GameObject var = enemyPathing.enemies[element];
                            //     if (i == (int)var.transform.position.x && j == (int)var.transform.position.z)
                            //     {
                            //         enemyPathing.enemies.Remove(var);
                            //         Destroy(var);
                            //     }
                            // }
                        }
                    }
                }
            }
        }
         for (int element = enemyPathing.enemies.Count - 1; element >= 0; element--)
                            {
                                GameObject var = enemyPathing.enemies[element];
                                if (Vector3.Distance(var.transform.position, player.transform.position) > viewRange)
                                {
                                    enemyPathing.enemies.Remove(var);
                                    Destroy(var);
                                }
                            }

        if (spawnFlag == false)
        {
                GameObject enemy = Instantiate(enemyPathing.enemyResources[0], new Vector3(3.0f + 0.5f, 0.5f, 5.0f + 0.5f), Quaternion.identity);
                                EnemyAI test = enemy.GetComponent<EnemyAI>();
                                test.player = player;
                                test.tempMap = map;
                                test.enemyPathing=enemyPathing;
                                enemyPathing.enemies.Add(enemy);
                                enemy = Instantiate(enemyPathing.enemyResources[0], new Vector3(5.0f + 0.5f, 0.5f, 5.0f + 0.5f), Quaternion.identity);
                                test = enemy.GetComponent<EnemyAI>();
                                test.player = player;
                                test.tempMap = map;
                                test.enemyPathing=enemyPathing;
                                enemyPathing.enemies.Add(enemy);
                                spawnFlag=true;
        }
    }
}
