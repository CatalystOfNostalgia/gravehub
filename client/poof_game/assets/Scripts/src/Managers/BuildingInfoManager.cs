using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingInfoManager : MonoBehaviour {

	private Dictionary<string, BuildingInfo> infoDict;

	public static BuildingInfoManager manager;
	public static BuildingInfoManager Instance(){
		if (!manager) {
			manager = FindObjectOfType(typeof (BuildingInfoManager)) as BuildingInfoManager;
			if(!manager)
				Debug.LogError ("There needs to be one active BuildingInfoManager script on a GameObject in your scene.");
		}
		return manager;
	}

	// Use this for initialization
	void Start () {
		infoDict = new Dictionary<string, BuildingInfo> ();
		sampleAddInfo ();
	}

	public BuildingInfo getInfo(string key){
		BuildingInfo value;
		if (infoDict.TryGetValue(key, out value)){
			return value;
		}
		return null;
	}

	public Dictionary<string, BuildingInfo> InfoDict{ get { return infoDict; } }

	//TODO
	/// <summary>
	/// Populates the dict from JSON.
	/// the server will determine what the user can build
	/// </summary>
	public void populateDictFromJSON(){

	}

	public void sampleAddInfo(){
		infoDict.Add ("Fire Tree", new BuildingInfo ("Fire Tree", "It's a burning Tree!", 200, 0, 0, 0));
		infoDict.Add ("Pond", new BuildingInfo ("Pond", "Poofs pee in this pond", 0, 200, 0, 0));
		infoDict.Add ("Windmill", new BuildingInfo ("Windmill", "Spinning~", 0, 0, 0, 200));
		//lets say cave is blocked
		//infoDict.Add ("Cave", new BuildingInfo ("Cave", "It's dark in here", 0, 0, 200, 0));
	}
}
