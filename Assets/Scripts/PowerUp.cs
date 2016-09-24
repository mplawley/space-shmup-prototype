using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
	//This is an unusual but handy use of Vector2s. x holds a min value
	//and y a max value for a Random.Range() that will be called later
	public Vector2 rotMinMax = new Vector2(15, 90);
	public Vector2 driftMinMax = new Vector2(.25f, 2);
	public float lifeTime = 6f; //Seconds the PowerUp exists
	public float fadeTime = 4f; //Seconds it will then fade

	[Header("________________")]
	public WeaponType type; //The type of the PowerUp
	public GameObject cube; //Reference to the Cube child
	public TextMesh letter; //Reference to the TextMesh
	public Vector3 rotPerSecond; //Euler rotation speed
	public float birthTime;

	void Awake()
	{
		//Find the cube reference
		cube = transform.Find("Cube").gameObject;

		//Find the TextMesh
		letter = GetComponent<TextMesh>();

		//Set a random velocity
		Vector3 vel = Random.onUnitSphere; //Get RandomXYZ velocity

		//Random.onUnitSphere gives you a vector point that is somewhere on the surface of the sphere
		//with a radius of 1 M around the origin
		vel.z = 0; //Flatten the vel to the XY plane
		vel.Normalize(); //Make the length of the vel 1

		//Reminder: Normalizing is finding the unit vector
		vel *= Random.Range(driftMinMax.x, driftMinMax.y);
		GetComponent<Rigidbody>().velocity = vel;

		//Set the rotation of this GameObject to R:[0, 0, 0]. Quaternion.identity is equal to no rotation
		transform.rotation = Quaternion.identity;

		//Set up the rotPerSecond for the Cube child using rotMinMax x and y
		rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
			Random.Range(rotMinMax.x, rotMinMax.y),
			Random.Range(rotMinMax.x, rotMinMax.y));

		//CheckOffscreen() every 2 seconds
		InvokeRepeating("CheckOffscreen", 2f, 2f);

		birthTime = Time.time;
	}

	void Update()
	{
		//Manually rotate the Cube child every Update()
		//Multiplying it by Time.time causes the rotation to be time-based
		cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

		//Fade out the PowerUP over time
		//Give the default values, a PowerUp will exist for 10 seconds and then fade out over 4 seconds
		//TODO
	}
}
