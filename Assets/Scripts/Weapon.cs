using UnityEngine;
using System.Collections;

//This is an enum of the various possible weapon types. It also includes a "shield" type to allow a shield power-up
//Items marked Not Implemented are kept here for future possible development but have no functionality in build 1.0

public enum WeaponType
{
	none, //The default, no weapon
	blaster, //A simple blaster
	spread, //Two shots simultaoneously
	phaser, //Shots that move in weaves. NOT IMPLEMENTED
	missile, //Homing missiles. NOT IMPLEMENTED
	laser, //Damage over time. NOT IMPLEMENTED
	shield //Raise shieldLevel
}

//The WeaponDefinition class allows you to set the properties of a specific weapon in the Inspector
//Main has an array of WeaponDefinitions that makes this possible.
//Note: Weapon prefabs, colors, and so on are set in the class Main.

[System.Serializable]
public class WeaponDefinition
{
	public WeaponType type = WeaponType.none;
	public string letter; //The letter to show on the power-up
	public Color color = Color.white; //Color of Collar and power-up
	public GameObject projectilePrefab; //Prefab for projectiles
	public Color projectileColor = Color.white;
	public float damageOnHit = 0; //Amount of damage caused
	public float continuousDamage = 0; //Damage per second (Laser)
	public float delayBetweenShots = 0;
	public float velocity = 20; //Speed of projectiles
}

public class Weapon : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}
