using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingInfoMenu : MonoBehaviour {

	public Text buildingNameText;
	public Text buildingDescriptionText;
	public Text fireResourceCostText;
	public Text waterResourceCostText;
	public Text airResourceCostText;
	public Text earthResourceCostText;
	public GameObject buildingInfoPanel;
	public Button exitButton;

	
	private static BuildingInfoMenu modalPanel;
	
	public static BuildingInfoMenu Instance(){
		if (!modalPanel) {
			modalPanel = FindObjectOfType(typeof (BuildingInfoMenu)) as BuildingInfoMenu;
			if(!modalPanel)
				Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
		}
		return modalPanel;
	}

	public void openMenu(BuildingInfo info){
		openMenu (info.Name, info.Description, info.FireCost, info.WaterCost, info.EarthCost, info.AirCost);
	}
	public void openMenu(string buildingName, string buildingDescription, int fireCost, int waterCost, int airCost, int earthCost){
		buildingNameText.text = buildingName;
		buildingDescriptionText.text = buildingDescription;
		fireResourceCostText.text = ""+fireCost;
		waterResourceCostText.text = ""+waterCost;
		airResourceCostText.text = ""+airCost;
		earthResourceCostText.text = ""+earthCost;

		exitButton.onClick.RemoveAllListeners ();
		exitButton.onClick.AddListener (ClosePanel);

		buildingInfoPanel.SetActive (true);
	}

	void ClosePanel(){
		buildingInfoPanel.SetActive(false);
	}
}
