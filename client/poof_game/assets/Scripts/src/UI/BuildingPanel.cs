using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

/**
 * This menu should
 * 1) Allow users to place buildings on the grid
 * 2) Decorate buildings
 */
public class BuildingPanel : GamePanel {
	
	// A static reference to this object
	public static BuildingPanel buildingPanel;
	private Button[] resourceButtons;
    private Button[] decorativeButtons;

    private enum panel : int { DECORATIVE, RESOURCE };
    private panel activePanel; 
		
	/**
     * Generates references based on children
     */
	override public void Start()
	{
		if (buildingPanel == null) {
			DontDestroyOnLoad(gameObject);
			buildingPanel = this;
		} 
		else if (buildingPanel != this) {
			Destroy(gameObject);
		}
        activePanel = panel.DECORATIVE;
        SwitchPanels();
	}

    /**
     * A function for switching between the panels
     */
    public void SwitchPanels()
    {
        // Turn off current panel
        this.transform.GetChild((int)activePanel).gameObject.SetActive(false);

        // Set activePanel
        if (activePanel == panel.RESOURCE)
        {
            activePanel = panel.DECORATIVE;
        }
        else
        {
            activePanel = panel.RESOURCE;
        }

        // Turn on current panel
        this.transform.GetChild((int)activePanel).gameObject.SetActive(true);
    }

    /**
     * Adds functionality to all of the buttons on the panel
     */
    override public void GeneratePanel(){
        if (resourceButtons != null)
        {
            foreach(Button button in resourceButtons)
            {
                Destroy(button.gameObject);
            }
        }
        if (decorativeButtons != null)
        {
            foreach(Button button in decorativeButtons)
            {
                Destroy(button.gameObject);
            }
        }

        // Clear out arrays
        resourceButtons = null;
        decorativeButtons = null;

        // Rebuild them
        resourceButtons = CreateButtons(PrefabManager.prefabManager.resourceBuildings, "Resource Building Panel/Buttons");
        decorativeButtons = CreateButtons(PrefabManager.prefabManager.decorativeBuildings, "Decorative Building Panel/Buttons");

        foreach (Button b in resourceButtons)
		{
			b.onClick.RemoveAllListeners();
			b.onClick.AddListener(TogglePanel);
		}
        foreach (Button b in decorativeButtons)
        {
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(TogglePanel);
        }
	}

    /**
     * Generates buttons per panel
     */
    public Button[] AddButtonsToPanel(Building[] buildings, string path)
    {
        List<Button> list = new List<Button>();
        List<GameObject> temp = new List<GameObject>();
        // Mega hardcoded and risky
        foreach (Transform t in transform.GetChild(0).GetChild(0))
        {
            temp.Add(t.gameObject);
        }
        GameObject[] go = temp.ToArray();
        
        // If we are adding buttons to the decorative building panel
        for (int i = 0; i < buildings.Length; i++)
        {
            Button b = MakeButton(path, go[i].transform.position, buildings[i]);
            //b.transform.SetParent(go[i].transform);
            list.Add(b);
        }
        return list.ToArray();
    }

    /**
     * Generates a button
     */
    public Button MakeButton(string path, Vector3 position, Building b)
    {
        // Build new game object and attach components
        GameObject go = new GameObject();
        Button button = go.AddComponent<Button>();
        Image image = go.AddComponent<Image>();
        ButtonDragScript bds = go.AddComponent<ButtonDragScript>();

        // Attach a text object to the button
		Image buildingInfo = Instantiate (PrefabManager.prefabManager.buildingInfo);
		buildingInfo.transform.SetParent (go.transform);
		buildingInfo.rectTransform.localPosition = new Vector3 (0,40,0);
		Text text = buildingInfo.GetComponentInChildren<Text> ();
        text.text = b.name;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.color = Color.black;

        // Attach a cost object to the object
		buildingInfo = Instantiate (PrefabManager.prefabManager.buildingInfo);
		buildingInfo.transform.SetParent (go.transform);
		buildingInfo.rectTransform.localPosition = new Vector3 (0,-40,0);
		Text textCost = buildingInfo.GetComponentInChildren<Text> ();
        textCost.transform.SetParent(go.transform);
        ResourceBuildingInformation rbi;
        DecorationBuildingInformation dbi;
        if (SaveState.state.buildingInformationManager.ResourceBuildingInformationDict.TryGetValue(b.name, out rbi))
        {
            textCost.text = rbi.FireCost + "F," + rbi.WaterCost + "W," + rbi.AirCost + "A," + rbi.EarthCost + "E";
        }
        else if (SaveState.state.buildingInformationManager.DecorationBuildingInformationDict.TryGetValue(b.name, out dbi))
        {
            textCost.text = dbi.FireCost + "F," + dbi.WaterCost + "W," + dbi.AirCost + "A," + dbi.EarthCost + "E";
        }
        textCost.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textCost.color = Color.black;

        // Set the name and parent of the game object
        go.transform.SetParent(this.transform.Find(path));
        go.name = b.name;

        // Rect Transform stuff
        button.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 70);

        // Image component stuff
        SpriteRenderer sr = b.GetComponent<SpriteRenderer>();
        image.sprite = sr.sprite;

        button.transform.position = position;
        bds.building = b;
        return button;
    }

    /**
     * Dynamically creates buttons
     * path - supplies the path the the parent for the buttons
     * SaveState is never initialized from the Demo Scene, you must start from Login Scene
     * TODO: Call this when the user upgrades the hq so that the building menu is refreshed
     */
    public Button[] CreateButtons(Building[] buildingList, string path)
    {
       
        List<Building> list = new List<Building>();
        int i;
        for (i = 0; i < buildingList.Length && list.Count<4; i++) //i < index + 4 not sure what this does
        {
            // Checking if the user can build the building or not
            ResourceBuildingInformation resourceBuildingInfo;
            DecorationBuildingInformation decorationBuildingInfo;

            // SaveState is never initialized from the Demo Scene, you must start from Login Scene
            if (SaveState.state.buildingInformationManager.ResourceBuildingInformationDict.TryGetValue(buildingList[i].name, out resourceBuildingInfo))
            {
                ResourceBuildingLevelCheck(buildingList, list, i, resourceBuildingInfo);
            }
            else if (SaveState.state.buildingInformationManager.DecorationBuildingInformationDict.TryGetValue(buildingList[i].name, out decorationBuildingInfo))
            {
                DecorationBuildingLevelCheck(buildingList, list, i, decorationBuildingInfo);
            }
        }
        return AddButtonsToPanel(list.ToArray(), path);
    }

    /**
     * A function which checks the level requirement of a building
     */
    private void ResourceBuildingLevelCheck(Building[] buildingList, List<Building> list, int i, ResourceBuildingInformation resourceBuildingInfo)
    {
        int levelRequirement = resourceBuildingInfo.LevelRequirement;
        if (levelRequirement == 1 && (! BuildingManager.buildingManager.alreadyPlacedDownBuildings.Contains(buildingList[i].name)))
        {
            list.Add(buildingList[i]);
        }
    }

    /**
     * A function which checks the level requirement of a building
     */
    private void DecorationBuildingLevelCheck(Building[] buildingList, List<Building> list, int i, DecorationBuildingInformation decorationBuildingInfo)
    {
        int levelRequirement = decorationBuildingInfo.LevelRequirement;
        if (SaveState.state.hqLevel >= levelRequirement)
        {
            list.Add(buildingList[i]);
        }
    }
}
