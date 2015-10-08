﻿using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Web;

public class SaveState : MonoBehaviour {

	// Allows the scene to access this object without searching for it
	public static SaveState state;
	// List game state variables here
	// Format: public <Type> <Name> { get; set; }
	public int gold { get; set; }
	public int maxGoldHeld { get; set; }
	public int silver { get; set; }
	public int wood { get; set; }
    public int fireEle { get; set; }
    public int waterEle { get; set; }
    public int earthEle { get; set; }
    public int airEle { get; set; }
    public Dictionary<Tuple, Building> existingBuildingDict { get; set; }


    /**
	 * A helper method for passing data from this
	 * game state to the serializable object
	 */
    private void SetPlayerData(PlayerData pd) {
		pd.gold = this.gold;
        pd.maxGoldHeld = this.maxGoldHeld;
        pd.silver = this.silver;
        pd.wood = this.wood;
        pd.fireEle = this.fireEle;
        pd.waterEle = this.fireEle;
        pd.earthEle = this.earthEle;
        pd.airEle = this.airEle;
        pd.existingBuildingDict = this.existingBuildingDict;
	}

	/**
	 * A helper method for pulling data from 
	 * the serializable object to this game state
	 */
	private void GetPlayerData(PlayerData pd) {
		this.gold = pd.gold;
        this.maxGoldHeld = pd.maxGoldHeld;
        this.silver = pd.silver;
        this.wood = pd.wood;
        this.fireEle = pd.fireEle;
        this.waterEle = pd.waterEle;
        this.earthEle = pd.earthEle;
        this.airEle = pd.airEle;
        this.existingBuildingDict = pd.existingBuildingDict;
	}

	/**
	 * Produces a singleton on awake
	 */
	public void Awake() {

		if (state == null) {
			DontDestroyOnLoad(gameObject);
			state = this;
		} else if (state != this) {
			Destroy(gameObject);
		}

		// set the fields until we can load
		state.gold = 0;
		state.existingBuildingDict = new Dictionary<Tuple, Building>();
        fireEle = 5;
        earthEle = 5;
        waterEle = 5;
        airEle = 5;
	}

	/**
	 * Pushes player data to server
	 */
	public void PushToServer() {
		PlayerData data = new PlayerData ();
		SetPlayerData (data);
		string clientJson = data.ToJSON ();
		Debug.Log (clientJson);
		// TODO Send JSON to server
	}

	/**
	 * Pulls player data from server
	 */
	public void PullFromServer() {
		// TODO Get JSON from server
		string serverJson = "{\"gold\":100,\"silver\":0,\"wood\":0";
		//PlayerData data = JSON.Deserialize<PlayerData> (serverJson);
		//GetPlayerData (data);
	}

	/**
	 * Saves all relevant data to a local file
	 * NOTE: This does not perform a write back but
	 * rather creates a fresh file every time.
	 */
	public void Save() {
		PlayerData data = new PlayerData();
		// Insert data from controller to data object
		// ie: data.setExp(this.getExp());
		SetPlayerData (data);

        // Dumps JSON to text file
        System.IO.File.WriteAllText(Application.persistentDataPath + "/save_state.dat", data.ToJSON());
	}

	/**
	 * Loads all relevant data from a file
	 */
	public void Load() {
		if (File.Exists (Application.persistentDataPath + "/save_state.dat")) {
            string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/save_state.dat");

            //PlayerData data = JSON.Deserialize<PlayerData>(json);
			// Reassign all variables here
			// ie: this.setHealth(data.getHealth());
			//GetPlayerData(data);
		}
	}

    /**
     * A testing function for observing
     * JSON output
     */
    public void CheckJson()
    {
        Debug.Log(new Tuple(3, 5).ToJSON());
        Building test;
        bool b = existingBuildingDict.TryGetValue(new Tuple(0, 0), out test);
        Debug.Log(existingBuildingDict.Values.Count);
        Debug.Log(existingBuildingDict.Keys.ToJSON());
        if (b)
        {
            Debug.Log(test.ToJSON());
        }
        else
        {
            Debug.Log("No building at 0,0");
        }
    }
}

[Serializable]
class PlayerData {
	
	/**
	 * Place the variables you want to save in this section
	 * Format: public <Type> <Name> { get; set; }
	 */
	public int gold { get; set; }
    public int maxGoldHeld { get; set; }
    public int silver { get; set; }
	public int wood { get; set; }
    public int fireEle { get; set; }
    public int waterEle { get; set; }
    public int earthEle { get; set; }
    public int airEle { get; set; }
    public Dictionary<Tuple, Building> existingBuildingDict { get; set; }

}
