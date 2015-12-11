using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

/**
 * The prefab manager is a tool which pools together
 * a list of prefabs to be used by the game
 * All prefabs are generated from the Resources folder
 */
public class PrefabManager : Manager {

    // Static reference to this gameobject
    public static PrefabManager prefabManager;

    // Below are lists of prefabs for use by the entire game
    public Building[] buildings { get; set; }
    public ResourceBuilding[] resourceBuildings { get; set; }
    public DecorativeBuilding[] decorativeBuildings { get; set; }
	public HeadQuarterBuilding[] headQuarterBuildings { get; set; }
    public Tile[] tiles { get; set; }
	public GameObject[] borders { get; set; }
    public NPC[] npcs { get; set; }
    public CanvasRenderer[] panels { get; set; }
    public Canvas canvas { get; set; }
	public Canvas buildingOptionCanvas { get; set; }
	public Image buildingInfo { get; set; }

    // Use this for initialization
    override public void Start () {
        if (prefabManager == null)
        {
            DontDestroyOnLoad(gameObject);
            prefabManager = this;
        }
        else if (prefabManager != this)
        {
            Destroy(gameObject);
        }

        GeneratePrefabLists();
        SetIDs();
    }
	
    /**
     * Generates all the lists of prefabs needed by the game 
     *
     * For now directory paths are hard coded
     */
    private void GeneratePrefabLists()
    {
        buildings = Resources.LoadAll("Prefabs/Buildings", typeof(Building)).Cast<Building>().ToArray();
        resourceBuildings = Resources.LoadAll("Prefabs/Buildings/Resource Buildings", typeof(ResourceBuilding)).Cast<ResourceBuilding>().ToArray();
        decorativeBuildings = Resources.LoadAll("Prefabs/Buildings/Decorative Buildings", typeof(DecorativeBuilding)).Cast<DecorativeBuilding>().ToArray();
		headQuarterBuildings = Resources.LoadAll ("Prefabs/Buildings/Headquarters", typeof(HeadQuarterBuilding)).Cast<HeadQuarterBuilding> ().ToArray ();
        tiles = Resources.LoadAll("Prefabs/Grid/Tile", typeof(Tile)).Cast<Tile>().ToArray();
		borders = Resources.LoadAll("Prefabs/Grid/border", typeof(GameObject)).Cast<GameObject>().ToArray();
        npcs = Resources.LoadAll("Prefabs/NPCs", typeof(NPC)).Cast<NPC>().ToArray();
        panels = Resources.LoadAll("Prefabs/UI/Panels", typeof(CanvasRenderer)).Cast<CanvasRenderer>().ToArray();
		canvas = (Canvas)Resources.Load("Prefabs/UI/Canvas", typeof(Canvas));
		buildingOptionCanvas = (Canvas)Resources.Load ("Prefabs/UI/local Panels/Building Option Canvas", typeof(Canvas));
		buildingInfo = (Image)Resources.Load ("Prefabs/UI/local Panels/Building Info", typeof(Image));
    }

    /**
     * Sets the ID of the building prefabs from the database
     * Sorts the building list with the ID
     */

    private void SetIDs()
    {
        // sets the ID of the buildings with the ID from the database
        foreach(Building b in resourceBuildings)
        {
            ResourceBuildingInformation info;
            if (SaveState.state.buildingInformationManager.ResourceBuildingInformationDict.TryGetValue(b.name, out info))
            {
                b.ID = info.ID;
            }
        }
        Array.Sort(resourceBuildings, new BuildingIDComparator());

        foreach(Building b in decorativeBuildings)
        {
            DecorationBuildingInformation info;
            if (SaveState.state.buildingInformationManager.DecorationBuildingInformationDict.TryGetValue(b.name,out info))
            {
                b.ID = info.ID;
                (b as DecorativeBuilding).poofGenerationRate = info.PoofAttractionRate;
            }
        }
        Array.Sort(decorativeBuildings, new BuildingIDComparator());
    }

    /**
     * Generates building object
     *
     * Deprecated -> Still using prefabs but generating list dynamically
     * instead of storing references
     */
    private Building GenerateBuildingObject(int size, string name, int fireCost, int waterCost, int earthCost, int airCost)
    {
        // This line creates an object -> We just want to generate a prefab
        GameObject obj = new GameObject();
        obj.name = name;

        obj.AddComponent<SpriteRenderer>();

        obj.AddComponent<ResourceBuilding>();
        Building ret = obj.transform.GetComponent<ResourceBuilding>();
        ret.size = size;
        ret.buildingName = name;
        ret.fireCost = fireCost;
        ret.waterCost = waterCost;
        ret.earthCost = earthCost;
        ret.airCost = airCost;

        Destroy(obj);

        return ret;
    }

    /**
     * Builds a sprite list
     *
     * Deprecated -> Use if you wish to generate your own prefabs
     */
    private void GenerateSpriteList(object[] objs, Sprite[] outList)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            System.Type type = objs[i].GetType();

            if (type == typeof(UnityEngine.Texture2D))
            {

                Texture2D tex = objs[i] as Texture2D;

                Sprite newSprite = Sprite.Create(objs[i] as Texture2D, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);

                outList[i] = newSprite;
            }
        }
    }
}
