﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* TileScript - Script attached to the Grid object in game; contains fields for to contain the
 * tiles, currently tiles are initialized by searching for GameObjects tagged as "Tile". Tiles 
 * are essentially hashed into an array by name, mimicing the current naming process within the editor. */

public class TileScript : MonoBehaviour {

    // Singleton variable so that only one grid variable is active per scene
    public static TileScript grid;

    // Fields that are used to contain and maintain all the tiles
    public List<Tile> tiles { get; set; }
	
	// Public fields
	public int mapLength = 6;
	public int mapWidth = 6;
    public GameObject[] prefab; // List of tile prefabs
	
    /**
     * Initializes the list of tiles
     * Generates the game grid
     */
	void Start() {
		
        if (grid == null)
        {
            // If the grid persists between scenes, then
            // we can't have the player go to other scenes
            //DontDestroyOnLoad(gameObject);
            grid = this;
        }
        else if (grid != this)
        {
            Destroy(gameObject);
        }

        tiles = new List<Tile>();
        Generate(prefab, transform.position, mapWidth, mapLength);
        //Debug.Log(tiles.ToJSON());
    }

    /**
     * A method used for building the game grid
     */
    public void Generate(GameObject[] tile, Vector3 orig, int width, int height)
    {
        for (int i = 0; i < mapLength; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                Vector3 location = orig + new Vector3(1.10f*(i + j - 5), .64f*(j - i), -2);
                GameObject gameObject = Instantiate(tile[(i + j) % tile.Length], location, Quaternion.identity) as GameObject;
                gameObject.transform.parent = this.transform;
                Tile t = gameObject.GetComponent<Tile>();
                t.index = new Tuple(i, j);
                tiles.Add(t);
            }
        }
    }
    
    /**
     * Generates a list of tiles adjacent to the
     * input tile
     */
    public List<Tile> GetAdjacentTiles(Tile tile)
    {
        List<Tile> listOfTiles = new List<Tile>();
        Tuple refer = tile.index;

        foreach (Tile test in tiles)
        {
            int x = refer.x - test.index.x;
            int y = refer.y - test.index.y;
            
            if (x <= 1 && x >= -1 && y <= 1 && y >= -1)
            {
                listOfTiles.Add(test);
            }
        }

        return listOfTiles;
    }

    /**
     * Given a tuple, this method will be able to produce
     * a list of legal tuples within a distance 1 from the start tuple
     *
     * This method takes advantage of the fact that tiles are indexed
     * by a tuple and treats that tuple as one of the corners of the tile
     * rather than the center
     */
    public List<Tuple> GetPossiblePaths(Tuple start)
    {
        List<Tuple> possiblePaths = new List<Tuple>();

        // Add the right path
        if (start.x < mapWidth)
        {
            possiblePaths.Add(new Tuple(start.x + 1, start.y));
        }
        // Add the left path
        if (start.x > 0)
        {
            possiblePaths.Add(new Tuple(start.x - 1, start.y));
        }
        // Add the top path
        if (start.y < mapLength)
        {
            possiblePaths.Add(new Tuple(start.x, start.y + 1));
        }
        // Add the bottom path
        if (start.y > 0)
        {
            possiblePaths.Add(new Tuple(start.x, start.y - 1));
        }

        return possiblePaths;
    }

    /**
     * A quick method for retrieving a tile based
     * on a Tuple 
     */
    public Tile GetTile(Tuple index)
    {
        foreach(Tile test in tiles)
        {
            // Implement equals in tuple
            if (test.index.Equals(index))
            {
                return test;
            }
        }
        return null;
    }
}

