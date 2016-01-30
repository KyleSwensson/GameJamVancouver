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
    public Count wallCount = new Count(110, 112); // min 5 wells per level max 10
    public GameObject wallTile;
    public GameObject floorTile;

    private Transform boardHolder; // keeps hierarchy clean


    private List<Vector3> gridPositions = new List<Vector3>(); //keeps track of all spots in gameboard
    private List<Vector3> walledGridPositions = new List<Vector3>(); // keeps track of all of the spaces where walls have already been placed

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

    private Boolean canCreateBlockAtPos(Vector3 pos)
    {
        Vector3 pos1 = pos;
        Vector3 pos2 = new Vector3(pos.x + 1, pos.y);
        Vector3 pos3 = new Vector3(pos.x - 1, pos.y);
        Vector3 pos4 = new Vector3(pos.x, pos.y + 1);
        Vector3 pos5 = new Vector3(pos.x, pos.y + 1);
        if (walledGridPositions.Contains(pos) || walledGridPositions.Contains(pos2) || walledGridPositions.Contains(pos3) || walledGridPositions.Contains(pos4) || walledGridPositions.Contains(pos5))
            return false;
        else
        {
            return true;
        }
    }

    void LayoutObjectAtRandom(GameObject tileToPlace, int minimumTiles, int maximumTiles)
    {
        // determines the amount of objects to spawn
        int objectCount = Random.Range(minimumTiles, maximumTiles + 1);

        for (int i = 0; i < objectCount; i++)
        {
            //TODO: the next thing that I should do is to make it so that it will only create a tile if pos1 pos2 pos3 and pos4 all 
            // have floor tiles on all sides beofre they are instantiated 



            // if the block is a square block it makes walls at (x,y)(x-1,y-1)(x-1,y)(y-1,x)
            // if the block is a t shaped block it makes walls at (x,y)(x,y-1)(x-1,y-1)(x+1,y-1)
            // if the block is I shaped it makes walls at (x,y)(x+1,y)(x+2,y)(x+3,y)
            // if the block is L shaped it makes walls at (x,y)(x+1,y)(x,y-1)(x,y-2)
            int shapeType = Random.Range(0, 4); // creates a number 0,1,2,3
            // if tile is 0 square block, if tile is 1 T block, if type is 2 I block, if type is 3 L block
            Vector3 pos1;
            Vector3 pos2;
            Vector3 pos3;
            Vector3 pos4;

            Vector3 randomPosition = RandomPosition();

            //while (!canCreateBlockAtPos(randomPosition))
            do
            {
                randomPosition = RandomPosition();

                if (shapeType == 0)
                {
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos2 = new Vector3(randomPosition.x - 1, randomPosition.y - 1);
                    pos3 = new Vector3(randomPosition.x, randomPosition.y - 1);
                    pos4 = new Vector3(randomPosition.x - 1, randomPosition.y);
                }
                else if (shapeType == 1)
                {
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos2 = new Vector3(randomPosition.x, randomPosition.y - 1);
                    pos3 = new Vector3(randomPosition.x - 1, randomPosition.y - 1);
                    pos4 = new Vector3(randomPosition.x + 1, randomPosition.y - 1);
                }
                else if (shapeType == 2)
                {
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos2 = new Vector3(randomPosition.x + 1, randomPosition.y);
                    pos3 = new Vector3(randomPosition.x + 2, randomPosition.y);
                    pos4 = new Vector3(randomPosition.x + 3, randomPosition.y);
                }
                else  // if shapetile == 3
                {
                    pos1 = new Vector3(randomPosition.x, randomPosition.y);
                    pos2 = new Vector3(randomPosition.x + 1, randomPosition.y);
                    pos3 = new Vector3(randomPosition.x, randomPosition.y - 1);
                    pos4 = new Vector3(randomPosition.x, randomPosition.y - 2);
                }
            } while (!canCreateBlockAtPos(randomPosition) || !canCreateBlockAtPos(pos1) || !canCreateBlockAtPos(pos2) || !canCreateBlockAtPos(pos3) || !canCreateBlockAtPos(pos4));
            Instantiate(tileToPlace, pos1, Quaternion.identity);
            walledGridPositions.Add(pos1);
            Instantiate(tileToPlace, pos2, Quaternion.identity);
            walledGridPositions.Add(pos2);
            Instantiate(tileToPlace, pos3, Quaternion.identity);
            walledGridPositions.Add(pos3);
            Instantiate(tileToPlace, pos4, Quaternion.identity);
            walledGridPositions.Add(pos4);

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
