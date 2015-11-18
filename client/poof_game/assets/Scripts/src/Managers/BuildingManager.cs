using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/**
 * Handles all building operations
 */
public class BuildingManager : Manager {

    // The target building
	private Building target;

	// The tile the mouse is currently one
	public Tile selectedTile { get; set; }

	// The dictionary containing all the different types of buildings that can be made
	Dictionary <string, Building> buildingTypeDict;
	Dictionary<Tuple, Building> existingBuildingDict;

    Dictionary<string, ResourceBuilding> prefabs;

	// The dictionary containing buildings on the grid
	public static BuildingManager buildingManager;

    // Does a thing
	public bool buildingMode;

    // A convenience object for holding all instantiated buildings
    private GameObject buildings;

    /**
     * Initializes BuildingManager as a singleton
     *
     * Initializes fields
     */
    override public void Start()
    {

        if (buildingManager == null)
        {
            DontDestroyOnLoad(gameObject);
            buildingManager = this;
        }
        else if (buildingManager != this)
        {
            Destroy(gameObject);
        }

        buildings = new GameObject();
        buildings.name = "Buildings";
        buildingTypeDict = new Dictionary<string, Building>();
        existingBuildingDict = new Dictionary<Tuple, Building>();
        //prefabs = createDictionary();
        //Debug.Log(prefabs.Keys.ToString());
    }

    /**
     * Generates a list of buildings
     */
    private Dictionary<string, ResourceBuilding> createDictionary()
    {
        Debug.Log(PrefabManager.prefabManager.resourceBuildings.ToString());
        return PrefabManager.prefabManager.resourceBuildings.ToDictionary(key => key.name, key => key);
    }

    /**
     * This overload does nothing right now
     * TODO: Make this do something
     */ 
    public void dragNewBuilding (int buildingNum, Vector3 cursor){
		buildingMode = true;
        target = PrefabManager.prefabManager.resourceBuildings[buildingNum];
	}

    /**
     * An overload to handle building conflicts
     */
    public void dragNewBuilding(string name, Vector3 cursor)
    {
        buildingMode = true;
        foreach(Building b in PrefabManager.prefabManager.resourceBuildings)
        {
            if (b.name == name)
            {
                target = b;
                return;
            }
        }
        Debug.Log(string.Format("The building [{0}] you are trying to map from a button to a physical building was not found", name));
    }

    /**
	 * 1. check building cost
	 * 2. see if user has enough resource to cover the cost
	 * 3. decrement resource
	 * 4. build
	 */
    public void makeNewBuilding (int buttonNum){
		buildingMode = true;
        target = PrefabManager.prefabManager.resourceBuildings[buttonNum];
	}

    /**
     * Maps the button to a string
     */
    public void makeNewBuilding(string name)
    {
        buildingMode = true;
        foreach (Building b in PrefabManager.prefabManager.resourceBuildings)
        {
            // Does not work because the buttons have generic names + Button(clone)
            if (b.name == name)
            {
                target = b;
                return;
            }
        }
        Debug.Log(string.Format("The building [{0}] you are trying to map from a button to a physical building was not found", name));
    }

	public bool isOccupied (){
		return false;
	}

	/**
     * Places a building on the currently selected tile
     */
	public void PlaceBuilding(Building prefab) {

		PlaceBuilding (prefab, selectedTile);
	}
	
	/**
     * Places a building on the given tile
     */
	public void PlaceBuilding (Building prefab, Tile tile) {

		if (tile == null) {
			Debug.Log ("tile is null");
		}

		Building newBuilding = tile.PlaceBuilding (prefab);
        newBuilding.created = true;
        
        // Sets the new building's parent to our convenience object
        newBuilding.transform.SetParent(buildings.transform);

		// TODO this feels pretty iffy
		if (!SaveState.state.resourceBuildings.ContainsKey (tile.index)) {
			SaveState.state.resourceBuildings.Add (tile.index, newBuilding);
		}
	}

    /**
     * TODO: Give description
     */
	private bool isTileTaken(Tuple t){
		return SaveState.state.resourceBuildings.ContainsKey (t);

	}

	/**
     * Rough estimate of distance because sqrt takes a lot of computation
     */
	private float getDistance(float x1, float y1, float x2, float y2){
		return (x1-x2)*(x1-x2) + (y1-y2)*(y1-y2);
	}

    /**
     * Helper method to get mouse position
     */
    private Vector3 getCurrentMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }
	
	/**
     * Update is called once per frame
     */
	void Update () {
		if (buildingMode) {
			buildingMode = false;
			if (!target) {
				Debug.Log ("no target");
			}
			PlaceBuilding(target);
			Debug.Log ("building mode set to false");
		}

	}
}
