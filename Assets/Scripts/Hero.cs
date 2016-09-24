using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{
	#region Fields
	public static Hero S; //Singleton

	[Header("Ship movement")]
	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;

	[Header("Ship status")]
	public float shieldLevel = 1;

	[Header("Other")]
	public Bounds bounds;

	#endregion

	#region Methods

	void Awake()
	{
		S = this; //Set the Singleton
		bounds = Utils.CombineBoundsOfChildren(this.gameObject);
	}

	void Start()
	{
	
	}
	
	// Update is called once per frame
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
	}

	void OnTriggerEnter(Collider other)
	{
		//Find the tag of other.gameObject or its parent GameObjects
		GameObject go = Utils.FindTaggedParent(other.gameObject);

		//If there is a parent with a tag
		if (go != null)
		{
			//Announce it
			print("Triggered: " + go.name);
		}
		else
		{
			//Otherwise, announce the original other.gameObject
			print("Triggered: " + other.gameObject.name);
		}
	}
	#endregion
}
