using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
	#region Fields
	public static Main S; //Singleton
	public static Dictionary<WeaponType, WeaponDefinition> W_DEFS;

	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f; //# of enemies per second
	public float enemySpawnPadding = 1.5f; //Padding for position
	public WeaponDefinition[] weaponDefinitions;

	[Header("___________________")]
	public WeaponType[] activeWeaponTypes;
	public float enemySpawnRate = 1.5f; //Delay between enemy spawns
	#endregion

	#region Methods
	void Awake()
	{
		S = this; //Singleton

		//Set Utils.camBounds
		Utils.SetCameraBounds(this.GetComponent<Camera>());

		//0.5 enemies/second = enemySpawnRate of 2
		enemySpawnRate = 1f/enemySpawnPerSecond;

		//Invoke call SpawnEnemy() once after a 2-second delay
		Invoke("SpawnEnemy", enemySpawnRate);

		//A generic dictionary with WeaponType as the key
		W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
		foreach (WeaponDefinition def in weaponDefinitions)
		{
			W_DEFS[def.type] = def;
		}
	}

	public static WeaponDefinition GetWeaponDefinition(WeaponType wt)
	{
		//Check to make sure that the key exists in the Dictionary
		//Attemping to retrieve a key that didn't exist would throw an error here:
		if (W_DEFS.ContainsKey(wt))
		{
			return (W_DEFS[wt]);
		}

		//This will return a definition for WeaponType.none, which means it has failed to find the WeaponDefinition
		return (new WeaponDefinition());
	}

	void Start()
	{
		activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
		for (int i = 0; i < weaponDefinitions.Length; i++)
		{
			activeWeaponTypes[i] = weaponDefinitions[i].type;
		}
	}

	public void SpawnEnemy()
	{
		//Pick a random Enemy prefab to instantiate
		int ndx = Random.Range(0, prefabEnemies.Length);
		GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;

		//Position the enemy above the sreen with a random x position
		Vector3 pos = Vector3.zero;
		float xMin = Utils.camBounds.min.x + enemySpawnPadding;
		float xMax = Utils.camBounds.max.x - enemySpawnPadding;
		pos.x = Random.Range(xMin, xMax);
		pos.y = Utils.camBounds.max.y + enemySpawnPadding;
		go.transform.position = pos;

		//Call SpawnEnemy() again in a couple of seconds
		Invoke("SpawnEnemy", enemySpawnRate);
	}

	public void DelayedRestart(float delay)
	{
		//Invoke the Restart() method in delay seconds
		Invoke("Restart", delay);
	}

	public void Restart()
	{
		//Reload the Scene_0 to restart the game
		SceneManager.LoadScene("Scene_0");
	}
	#endregion
}
