using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    public GameObject[] gridPrefabs;
    public int rows;
    public int cols;
    public float roomWidth = 50.0f;
    public float roomHeight = 50.0f;
    private Room[,] grid;

    public bool isMapRandom;
    public bool isDailyMap;
    public int mapSeed;

    private bool checkMapGenerationOnLoad = false;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        /*if (checkMapGenerationOnLoad == true)
        {
            checkMapGenerationOnLoad = false;
        }*/

        //OnStartGameButtonPress();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartGameButtonPress()
    {
        if (checkMapGenerationOnLoad == false)
        {
            if (isDailyMap)
            {
                mapSeed = DateToInt(DateTime.Now.Date);
            }
            else
            {
                mapSeed = DateToInt(DateTime.Now);
                //UnityEngine.Random.InitState(DateToInt(DateTime.Now));
            }
            GenerateMap();
        }
    }


    public void GenerateMap()
    {
        //set seed for random generator
        UnityEngine.Random.InitState(mapSeed);

        // clear out grid - "column" is x // "row" is y
        grid = new Room[cols, rows];

        // for each grid row
        for (int currentRow = 0; currentRow < rows; currentRow++)
        {
            // for each column in that row
            for (int currentCol = 0; currentCol < cols; currentCol++)
            {
                //figure out location
                float xPosition = roomWidth * currentCol;
                float zPosition = roomHeight * currentRow;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);

                //create new grid at location
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                //set its parent
                tempRoomObj.transform.parent = this.transform;

                //give it a meaningful name
                tempRoomObj.name = "Room_" + currentCol + "," + currentRow;

                //get room object
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                //save it to the grid array
                grid[currentCol, currentRow] = tempRoom;

                RowChecker(currentRow, tempRoom);
                ColumChecker(currentCol, tempRoom);
            }
        }
    }

    public void RowChecker(int currentRow, Room tempRoom)
    {
        if (currentRow == 0)
        {
            tempRoom.doorNorth.SetActive(false);
        }
        else if (currentRow == rows - 1)
        {
            Destroy(tempRoom.doorSouth);
        }
        else
        {
            Destroy(tempRoom.doorNorth);
            Destroy(tempRoom.doorSouth);
        }
    }

    public void ColumChecker(int currentCol, Room tempRoom)
    {
        if (currentCol == 0)
        {
            tempRoom.doorEast.SetActive(false);
        }
        else if (currentCol == cols - 1)
        {
            Destroy(tempRoom.doorWest);
        }
        else
        {
            Destroy(tempRoom.doorEast);
            Destroy(tempRoom.doorWest);
        }
    }

    public GameObject RandomRoomPrefab()
    {
        // returns random room
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    public int DateToInt(DateTime dateToUse)
    {
        // Add our date up and return it
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }
}
