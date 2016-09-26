using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{
	#region Fields
	[Header("Script maintenance")]
	public static Hero S; //Singleton
	public float gameRestartDelay = 2f;

	[Header("Ship movement")]
	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;

	[Header("Ship status and weapons")]
	[SerializeField]
	private float _shieldLevel = 1; //We'll watch the value of the _shieldLevel field via the shieldLevel property to put bounds on it
	public Weapon[] weapons;

	[Header("Other")]
	public Bounds bounds;

	//Declare a new delegate type WeaponFireDelegate
	public delegate void WeaponFireDelegate();

	//Create a WeaponFireDelegate field named fireDelegate
	public WeaponFireDelegate fireDelegate;

	#endregion

	#region Methods

	void Awake()
	{
		S = this; //Set the Singleton
		bounds = Utils.CombineBoundsOfChildren(this.gameObject);
	}

	void Start()
	{
		//Reset the weapons to start Hero with 1 blaster
		ClearWeapons();
		weapons[0].SetType(WeaponType.blaster);
	}

	void Update()
	{
		//Pull in information from the Input class
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");

		//Change transform.position based on the axes
		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;

		bounds.center = transform.position;

		//Keep the ship constrained to the screen bounds
		Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
		if (off != Vector3.zero)
		{
			pos -= off;
			transform.position = pos;
		}

		//Rotate the ship to make it feel more dynamic
		transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

		//Use the fireDelegate to fire Weapons.
		//First, make sure the Axis("Jump") button is pressed.
		//Then ensure that fireDelgate isn't null to avoid an error
		if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
		{
			fireDelegate();
		}
	}

	//This variable holds a reference to the last triggering GameObject
	public GameObject lastTriggerGo = null;

	void OnTriggerEnter(Collider other)
	{
		//Find the tag of other.gameObject or its parent GameObjects
		GameObject go = Utils.FindTaggedParent(other.gameObject);

		//If there is a parent with a tag
		if (go != null)
		{
			//Make sure it's not the same triggering go as last time
			if (go == lastTriggerGo)
			{
				return;
			}
			lastTriggerGo = go;

			if (go.tag == "Enemy")
			{
				//If the shield was triggered by an enemy, decrease the level of the shield by 1
				shieldLevel--;

				//Destroy the enemy
				Destroy(go);
			}
			else if (go.tag == "PowerUp")
			{
				//If the shield was triggered by a PowerUp
				AbsorbPowerUp(go);
			}
			else
			{
				print("Triggered: " + go.name);
			}
		}
		else
		{
			//Otherwise, announce the original other.gameObject
			print("Triggered: " + other.gameObject.name);
		}
	}

	public float shieldLevel
	{
		get
		{
			return(_shieldLevel);
		}
		set
		{
			//Upper bound to the shield
			_shieldLevel = Mathf.Min(value, 4);
			//Lower bound to the shield--AKA, player dies if the shield is going to be set to less than 0
			if (value < 0)
			{
				Destroy(this.gameObject);

				//Tell Main.S to restart the game after a delay
				Main.S.DelayedRestart(gameRestartDelay);
			}
		}
	}

	public void AbsorbPowerUp(GameObject go)
	{
		PowerUp pu = go.GetComponent<PowerUp>();
		switch (pu.type)
		{
		case WeaponType.shield:
			shieldLevel++;
			break;
		default: //If it's any Weapon PowerUp
			//Check the current weapon type
			if (pu.type == weapons[0].type)
			{
				//then increase the number of weapons of this type
				Weapon w = GetEmptyWeaponSlot(); //Find an available weapon
				if (w != null)
				{
					//Set it to pu.type
					w.SetType(pu.type);
				}
			}
			else
			{
				//If this is a different weapon
				ClearWeapons();
				weapons[0].SetType(pu.type);
			}
			break;
		}
		pu.AbsorbedBy(this.gameObject);
	}

	Weapon GetEmptyWeaponSlot()
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons[i].type == WeaponType.none)
			{
				return (weapons[i]);
			}
		}
		return (null);
	}

	void ClearWeapons()
	{
		foreach (Weapon w in weapons)
		{
			w.SetType(WeaponType.none);
		}
	}
	#endregion
}
