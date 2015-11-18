using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildingMenu : MonoBehaviour {
	
	public Text question;
	public Image iconImage;
	public Button button1;
	public Button button2;
	public Button button3;
	public Button button4;
	public Button exit;
	public GameObject modalPanelObject;

	private static BuildingMenu modalPanel;
	
	public static BuildingMenu Instance(){
		if (!modalPanel) {
			modalPanel = FindObjectOfType(typeof (BuildingMenu)) as BuildingMenu;
			if(!modalPanel)
				Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
		}
		return modalPanel;
	}
	public void Choice(){
		//modal panel should be visible on screen
		modalPanelObject.SetActive (true);
		
		button1.onClick.RemoveAllListeners ();
		button1.onClick.AddListener (() => BuildingInfoMenu.Instance().openMenu(BuildingInfoManager.Instance().getInfo("Fire Tree")));
		
		
		button2.onClick.RemoveAllListeners ();
		button2.onClick.AddListener (() => BuildingInfoMenu.Instance().openMenu(BuildingInfoManager.Instance().getInfo("Pond")));
		
		
		button3.onClick.RemoveAllListeners ();
		button3.onClick.AddListener (() => BuildingInfoMenu.Instance().openMenu(BuildingInfoManager.Instance().getInfo("Cave")));
		
		button4.onClick.RemoveAllListeners ();
		button4.onClick.AddListener (() => BuildingInfoMenu.Instance().openMenu(BuildingInfoManager.Instance().getInfo("Windmill")));
		
		exit.onClick.RemoveAllListeners ();
		exit.onClick.AddListener (ClosePanel);

		string buildquestion = "Select a Building";
		this.question.text = buildquestion; //sets question in dialogue box
		//this.iconImage.gameObject.SetActive (false);
		
		button1.gameObject.SetActive (true);
		button2.gameObject.SetActive (true);
		button3.gameObject.SetActive (true);
		
	}
	
	void ClosePanel(){
		modalPanelObject.SetActive(false);
	}
}
