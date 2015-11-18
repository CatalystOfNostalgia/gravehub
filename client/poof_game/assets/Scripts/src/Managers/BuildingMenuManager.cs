using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine.UI;

public class BuildingMenuManager : MonoBehaviour {

	
	public Button[] buildingButtons;

	public static BuildingMenuManager manager;
	public static BuildingMenuManager Instance(){
		if (!manager) {
			manager = FindObjectOfType(typeof (BuildingMenuManager)) as BuildingMenuManager;
			if(!manager)
				Debug.LogError ("There needs to be one active BuildingMenuManager script on a GameObject in your scene.");
		}
		return manager;
	}

	// Use this for initialization
	void Start () {
	
	}

	public void populateMenu(){
		List<BuildingInfo> buildingList = BuildingInfoManager.Instance ().InfoDict.Values.ToList();

		for (int i = 0; i<buildingList.Count; i++) {
			Button buildingButtonPrefab = findBuildingButton(buildingList[i].Name);
			Button buttonGameobject = (Button)Instantiate(buildingButtons[i]);
			buttonGameobject.transform.parent = this.transform;
			buttonGameobject.transform.position = new Vector3(0, 0+i*30);
		}
	}

	private Button findBuildingButton (string name){
		foreach (Button button in buildingButtons) {
			if (button.name == name){
				return button;
			}
		}
		return null;
	}
}
