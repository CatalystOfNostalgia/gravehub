using UnityEngine;
using System.Collections;

public class BuildingInfo : MonoBehaviour {
	private string name;
	private string description;
	private int fireCost;
	private int waterCost;
	private int airCost;
	private int earthCost;

	public BuildingInfo(string name, string description, int fireCost, int waterCost, int airCost, int earthCost){
		this.name = name;
		this.description = description;
		this.fireCost = fireCost;
		this.waterCost = waterCost;
		this.airCost = airCost;
		this.earthCost = earthCost;
	}

	public string Name { get{return name;} }
	public string Description { get{return description;} }
	public int FireCost { get{return fireCost;} }
	public int WaterCost { get{ return waterCost; } }
	public int AirCost { get{ return airCost; } }
	public int EarthCost { get{ return earthCost; } }
}
