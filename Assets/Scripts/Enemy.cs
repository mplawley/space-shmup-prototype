using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	#region Fields
	[Header("Enemy stats")]
	public float speed = 10f; //The speed in m/s
	public float fireRate = 0.3f; //The seconds/shot (Unused)
	public float health = 10;
	public int score = 100; //Pointed earned for destroying this

	[Header("Enemy bounds")]
	public Bounds bounds; //The bounds of this and its children
	public Vector3 boundsCenterOffset; //Distance of Bounds.center from position
	#endregion

	#region Methods

	void Awake()
	{
		InvokeRepeating("CheckOffscreen", 0f, 2f);
	}

	void Update()
	{
		Move();	
	}

	public virtual void Move()
	{
		Vector3 tempPos = pos;
		tempPos.y -= speed * Time.deltaTime;
		pos = tempPos;
	}

	//This is a Property: a method that acts like a field
	public Vector3 pos
	{
		get
		{
			return (this.transform.position);
		}
		set
		{
			this.transform.position = value;
		}
	}

	void CheckOffscreen()
	{
		//If bounds are still their default value...
		if (bounds.size == Vector3.zero)
		{
			//then set them
			bounds = Utils.CombineBoundsOfChildren(this.gameObject);

			//Also find the diff between bounds.center and transform.position
			boundsCenterOffset = bounds.center - transform.position;
		}

		//Every time, update the bounds to the current position
		bounds.center = transform.position + boundsCenterOffset;

		//Check to see whether the bounds are completely offscreen
		Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
		if (off != Vector3.zero)
		{
			//if this enemy has gone off the bottom edge of the screen
			if (off.y < 0)
			{
				//then destroy it
				Destroy(this.gameObject);
			}
		}
	}
	#endregion
}
