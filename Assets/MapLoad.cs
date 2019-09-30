using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapLoad : MonoBehaviour
{
    public int mapSize = 500;
    public GameObject mapChunk;
    public int[][] mapArray;
    public Sprite[] tempSprite;
    public GameObject[][] mapArrayObjects;
    public int[][] mapArraySpriteNumber;
    // Start is called before the first frame update
    void Start()
    {
        tempSprite = Resources.LoadAll<Sprite>("Materials/Sprites");
        mapArrayObjects = new GameObject[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            mapArrayObjects[i] = new GameObject[mapSize];
        }
          mapArraySpriteNumber = new int[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            mapArraySpriteNumber[i] = new int[mapSize];
        }

        mapArray = new int[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            mapArray[i] = new int[mapSize];
        }

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                mapArraySpriteNumber[i][j]=-1;
                mapArray[i][j] = 0;
            }
        }
        
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {

                MeshRenderer rend = mapChunk.GetComponent<MeshRenderer>();

             //   GameObject ChildGameObject0 = mapChunk.transform.GetChild(0).gameObject;
                // tempSprite=Resources.Load<Sprite>("treeDefault");
                //    Sprite tempSprite=Resources.Load <Sprite> ("treeDefault"); 


                //    Sprite[] sprites = (Sprite[])Resources.LoadAll("Materials/");
                if (tempSprite == null || tempSprite.Length == 0)
                    Debug.LogError("MapLoad: Cannot find sprites");
                //    mapChunk.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite=Resources.Load <Sprite> ("treeDefault");

                //     Vector3 tempPos= mapChunk.transform.Find("MapChunkSprite").gameObject.transform.position;
                //    tempPos.y=0.5f;
                //    mapChunk.transform.Find("MapChunkSprite").gameObject.transform.position=tempPos;
                //Set the main Color of the Material to green
                //rend.material.color= new Color(0,84,0,1);
               //! mapArrayObjects[i][j] = Instantiate(mapChunk, new Vector3(i + (float)0.5, 0, j + (float)0.5), Quaternion.identity);
                               if (j == 0 || i == 0 || i == mapSize - 1 || j == mapSize - 1)
                {
                    Debug.Log("i j:" + i + " " + j);
               //!     mapArrayObjects[i][j].transform.Find("MapChunkSprite").gameObject.GetComponent<SpriteRenderer>().sprite = tempSprite[1];
                    mapArraySpriteNumber[i][j]=1;
                    //        mapChunk.transform.Find("MapChunkSprite").gameObject.GetComponent<SpriteRenderer>().sprite=tempSprite[1];
                    mapArray[i][j] = 1;
                }
            
            }
        }
         for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {

                MeshRenderer rend = mapChunk.GetComponent<MeshRenderer>();
                if (j == 0 || i == 0 || i == mapSize - 1 || j == mapSize - 1)
                {
                  
                }
                else{
                    int random=Random.Range(0,50);
                    if(random<2){
                    //    mapArray[i][j]=1;
                    //!    mapArrayObjects[j][i].transform.Find("MapChunkSprite").gameObject.GetComponent<SpriteRenderer>().sprite = tempSprite[random];
                       // mapArraySpriteNumber[j][i]=random;
                    }
                }
            
            }
        }

                        //  mapArray[4][3]=1;
                        //  MeshRenderer rendtest=mapArrayObjects[3][4].GetComponent<MeshRenderer>();
                        //  rendtest.material.color=new Color(0,100,100,1);



    
        mapArray[2][3]=1;
        mapArraySpriteNumber[2][3]=1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
