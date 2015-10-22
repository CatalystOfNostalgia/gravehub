using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

    // Location
    public Tuple index { get; set; }

    // A boolean to determine if this tile is vacant
    public bool isVacant { get; set; }

    // Tile ID value
    public int id { get; set; }

    // References to nearby tiles for easy access
    public Tile upTile { get; set; }
    public Tile downTile { get; set; }
    public Tile leftTile { get; set; }
    public Tile rightTile { get; set; }
    public Tile upLeftTile { get; set; }
    public Tile upRightTile { get; set; }
    public Tile downLeftTile { get; set; }
    public Tile downRightTile { get; set; }

    // A private field for the color of this object
    private Color startColor;

    /**
     * Used for initialization of
     * Unity game objects
     */
    void Start()
    {
        isVacant = true;
    }

    /**
     * A method that works with
     * the collider of this object
     */
    void OnMouseDown()
    {
        Debug.Log(index.ToString());
        foreach (Tile t in GetAdjacentTiles()) {
            //Debug.Log(t.id);
        }
    }

    /**
     * A method that works with
     * the collider of this object
     */
    void OnMouseEnter()
    {
        startColor = GetComponent<Renderer>().material.color;

		BuildingManager.manager.selectedTile = this;

        if (isVacant)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    /**
     * A method that works with
     * the collider of this object
     */
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = startColor;
    }

    /**
     * Produces an array of all the tiles with 1 of this tile
     */
    public Tile[] GetAdjacentTiles()
    {
        List<Tile> nearbyTiles = new List<Tile>();
        AddTileToList(nearbyTiles, upTile);
        AddTileToList(nearbyTiles, downTile);
        AddTileToList(nearbyTiles, leftTile);
        AddTileToList(nearbyTiles, rightTile);
        AddTileToList(nearbyTiles, upLeftTile);
        AddTileToList(nearbyTiles, upRightTile);
        AddTileToList(nearbyTiles, downLeftTile);
        AddTileToList(nearbyTiles, downRightTile);
        return nearbyTiles.ToArray();
    }

    /**
     * A private helper method for adding a tile
     * to a list if it exists
     */
    private void AddTileToList(List<Tile> list, Tile tile)
    {
        if (tile != null)
        {
            list.Add(tile);
        }
    }
}
