using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
/**
 * Let's add a settings menu. It will be used to control music mainly.
 * This menu should
 * 1) Allow the user to change the song
 * 2) Set a single/not play other songs
 * 3) Adjust the volume ratio between sound effects and music.
 */
public class SettingsMenu : MonoBehaviour {

	public Button playButtonAir;
	public Button playButtonEarth;
	public Button playButtonFire;
	public Button playButtonWater;

	public Button exit;
	public GameObject settingsPanel;
	public SoundManager soundManager;

	/**
	 * 1. get a reference to the Sound Manager and the songs
	 */
	// Use this for initialization
	void Start () {
	}

	public void generatePanel(){
		settingsPanel.SetActive (true);
		AudioSource[] music = soundManager.playlist;
		//idk why loop approach doesn't work. it only plays water theme
//		foreach (Button button in playButtons) {
//			button.onClick.RemoveAllListeners();
//			button.onClick.AddListener (() => soundManager.playSong(button.name));
//			Debug.Log("SettingsMenu: added " + button.name + "'s action listener");
//		}
		playButtonAir.onClick.RemoveAllListeners ();
		playButtonAir.onClick.AddListener (() => soundManager.playSong(playButtonAir.name));
		playButtonEarth.onClick.RemoveAllListeners ();
		playButtonEarth.onClick.AddListener (() => soundManager.playSong(playButtonEarth.name));
		playButtonFire.onClick.RemoveAllListeners ();
		playButtonFire.onClick.AddListener (() => soundManager.playSong(playButtonFire.name));
		playButtonWater.onClick.RemoveAllListeners ();
		playButtonWater.onClick.AddListener (() => soundManager.playSong(playButtonWater.name));
		exit.onClick.RemoveAllListeners ();
		exit.onClick.AddListener (ClosePanel);
	}

	void ClosePanel(){
		settingsPanel.SetActive(false);
	}
}
