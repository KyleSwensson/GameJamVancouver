using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 32;
    public int rows = 32;
    public Count wallCount = new Count(20, 30); // min 5 wells per level max 10
    public GameObject wallTile;
    public GameObject floorTile;

    private Transform boardHolder; // keeps hierarchy clean


    private List<Vector3> gridPositions = new List<Vector3>(); //keeps track of all spots in gameboard

    void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y =  1; y < columns - 1; y++)
            {
                // this is creating a list of spaces where stuff (walls, people can be placed)
                gridPositions.Add(new Vector3(x, y, 0f));
                //loops dont go from 0 to rows and instead form 1 to columns
                // we want a clear loop around the outside of the screen
            }
        }
    }

    void BoardSetup()
    {
        // sets up outer wall and floor of the game
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x ++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                // these go to cols + 1 because the edge is outside of the outer edge of the screen
                //prepares to instantiate a floortile
                GameObject toInstantiate = floorTile;
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    // if the tile is on the outer wall instantiate it as a wall tile instad of a floor tile
                    toInstantiate = wallTile;
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex); // removes the floortile in the space to replace it w/ something else
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject tileToPlace, int minimumTiles, int maximumTiles)
    {
        // determines the amount of objects to spawn
        int objectCount = Random.Range(minimumTiles, maximumTiles + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            Instantiate(tileToPlace, randomPosition, Quaternion.identity);

        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTile, wallCount.minimum, wallCount.maximum);
        // more stuff here
    }
	// Use this for initialization
	void Start () {
	
	}

 
	
	// Update is called once per frame
	void Update () {
	
	}
}
