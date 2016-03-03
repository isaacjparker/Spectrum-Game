using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SP_StarSystemManager : MonoBehaviour {

	private StarSystem starSystem;
	private List <string> systemNameList;

	//Map settings
	public float systemSize = 136;
	public float systemScale = 27;
	public float initRotationSpeed = 0.7f;

	//Tiles to use to populate map
	public GameObject[] planetTiles;	//Create an array to store the planet sprites to load.
	public GameObject sunTile;

	//Map marker objects
	public GameObject mapMarker;
	public bool turnOnGrid;
	public GameObject gridMarker;

	//Lists to store possible spawn posiitions for planets and markers.
	private List <Vector3> gridPositions = new List<Vector3> ();	//use this to track all the possible positions for planets on the map. And keep track if object has been spawned in that position.
	//private float[] radiusPositions;		//use this to track the different radii a planet can orbit around.
	private List <Vector3> markerPositions = new List<Vector3> ();

	//Stores how many grid positions there are on a map
	//by dividing the mapSize by the mapScale (eg. Size 135 by Scale 27
	//would create 5 by 5 grid posiitons on the map.
	private float gridSize;




	public StarSystem BuildStarSystem (int startingIndex, int index, List<string> nameList)
	{
		starSystem = new StarSystem();

		InitialiseList ();
		MarkerSetup ();
		PlaceSun ();
		LayoutObjectAtRandom (planetTiles, starSystem.planetCount.minimum, starSystem.planetCount.maximum);
		PositionOrbits ();
		SetupPlanetMasks ();
		RotatePlanetMasks ();


		return starSystem;
	}

	void DestroyStarSystem (StarSystem starSystem)
	{

	}

	void StoreStarSystem (StarSystem starSystem)
	{

	}

	StarSystem LoadStarSystem (int index)
	{
		starSystem = new StarSystem();

		return starSystem;
	}








	void InitialiseList()
	{
		gridPositions.Clear ();
		markerPositions.Clear ();
		gridSize = systemSize / systemScale;

		for (int x = 0; x < gridSize; x++)
		{
			for (int y = 0; y < gridSize; y++) 
			{
				gridPositions.Add (new Vector3 (x * systemScale, y * systemScale, 0f));
			}
		}



		for (int a = 0; a < 2; a++) 
		{
			for (int b = 0; b < 2; b++) 
			{
				markerPositions.Add (new Vector3 (a*systemSize - (systemSize/2), b*systemSize - (systemSize/2), 0f));
			}
		}
	}


	void MarkerSetup()
	{

		//Instaniate a central marker and a marker for each corner of the map.
		//Instantiate (mapMarker, new Vector3 (0, 0, 0), Quaternion.identity);

		for (int x = 0; x < 4; x++) 
		{
			Instantiate (mapMarker, markerPositions[x], Quaternion.identity);
		}

		//Instantiate Grid markers.
		if (turnOnGrid == true) 
		{
			for (int y = 0; y < gridPositions.Count; y++) 
			{
				Instantiate (gridMarker, gridPositions [y], Quaternion.identity);
			}
		}

	}

	/// <summary>
	/// Return a random position within the available grid positions.
	/// Called by further functions looking to randomly place their instances.
	/// </summary>
	/// <returns>A random position from provided grid</returns>
	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	/// <summary>
	/// Places the sun in a random position. Should be called
	/// before planet placement so that their shadow masks can be rotated
	/// away from the sun.
	/// </summary>
	void PlaceSun()
	{
		//Vector3 randomPosition = RandomPosition ();
		//randomPosition.x = randomPosition.x - (systemSize/2);
		//randomPosition.y = randomPosition.y - (systemSize/2);

		starSystem.sunInstance = Instantiate (sunTile, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
	}


	/// <summary>
	/// Lays out the planet instances using the RandomPosition function.
	/// </summary>
	/// <param name="tileArray">Tile array.</param>
	/// <param name="minimum">Minimum.</param>
	/// <param name="maximum">Maximum.</param>
	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);
		Debug.Log ("Object Count: " + objectCount.ToString ());

		starSystem.planets = new Planet[objectCount];

		for (int j = 0; j < objectCount; j++) 
		{
			starSystem.planets [j] = new Planet ();
		}

		for (int i = 0; i < objectCount; i++) 
		{
			Vector3 randomPosition = RandomPosition ();
			randomPosition.x = randomPosition.x - (systemSize/2);
			randomPosition.y = randomPosition.y - (systemSize/2);
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];		//Get a random planet and assign it to tileChoice

			//starSystem.planets [i].instance = new GameObject ();

			//Instantiate that choice at the randomPosition defined and store it in an instanced object.
			starSystem.planets[i].instance = Instantiate (tileChoice, randomPosition, Quaternion.identity) as GameObject;
		}
	}


	/// <summary>
	/// Firstly, initialises the arrays for storing the different radii each planet will rotate around
	/// and the speeds of each planet.
	/// Then cycle through each planet (the number of which is defined by planetInstances.Length)
	/// and attach the next radius (in increments of 10) to each planet and a random speed as
	/// created in RandomRotationSpeed function. Finally, position each planet on its radius. 
	/// The arrays are then used each frame in the OrbitPlanets
	/// function called from Update().
	/// </summary>
	void PositionOrbits()
	{
		//radiusPositions = new float[starSystem.planets.Length];
		//rotationSpeed = new float[starSystem.planets.Length];

		for (int x = 0; x < starSystem.planets.Length; x++) 
		{
			starSystem.planets[x].radiusPosition = new float();
			starSystem.planets[x].rotationSpeed = new float();
		}

		for (int j = 0; j < starSystem.planets.Length; j++) 
		{
			starSystem.planets[j].radiusPosition = starSystem.planets[j].radius + (j * 10);
			starSystem.planets[j].rotationSpeed = RandomRotationSpeed();
			//Debug.Log ("Rad Pos: " + radiusPositions [j].ToString ());
			//Debug.Log ("Rand Rot (" + j.ToString() + ") " + rotationSpeed[j].ToString ());
		}

		for (int i = 0; i < starSystem.planets.Length; i++) 
		{
			starSystem.planets [i].instance.transform.position = (starSystem.planets [i].instance.transform.position - new Vector3 (0, 0, 0)).normalized * starSystem.planets[i].radiusPosition + starSystem.sunInstance.transform.position;
		}
	}

	float RandomRotationSpeed()
	{
		float randRotation = (Random.Range (-initRotationSpeed, initRotationSpeed)) + 0.1f;
		return randRotation;
	}


	/// <summary>
	/// Simply finds all child objects attached to the planet instances
	/// and adds them to an array for modification in RotatePLanetMasks.
	/// </summary>
	void SetupPlanetMasks()
	{
		//planetMaskArray = GameObject.FindGameObjectsWithTag("PlanetMask");

		for (int i = 0; i < starSystem.planets.Length; i++) 
		{
			starSystem.planets [i].mask = starSystem.planets [i].instance.GetComponentInChildren<Transform> ().Find ("Mask");
		}

	}

	/// <summary>
	/// Rotates all the shadow masks away from the sun.
	/// </summary>
	void RotatePlanetMasks()
	{

		for (int i = 0; i < starSystem.planets.Length; i++) 
		{
			if (starSystem.sunInstance != null && starSystem.planets [i].mask != null) 
			{
				Vector3 targetPosition = starSystem.sunInstance.transform.position - starSystem.planets [i].mask.transform.position;		//used to use sunTile but didn't work. Cast to new object and we're good.
				float angle = (Mathf.Atan2 (targetPosition.y, targetPosition.x) * Mathf.Rad2Deg) - 180;
				starSystem.planets [i].mask.transform.localRotation = Quaternion.AngleAxis (angle, Vector3.forward);		//very important to make it localrotation

			}
		}

	}




























	/*
	void ChooseSystemName()
	{
		//starSystem.nameList = new Enums.SystemNames();
		//starSystem.nameList = GetRandomEnum<Enums.SystemNames> ();
		//starSystem.systemName = starSystem.nameList.ToString ();

		//systemNameList = PopulateNameList<Enums.SystemNames> ();
		starSystem.systemName = RandomString (systemNameList);
	}



	static T GetRandomEnum<T>()
	{
		System.Array tempArray = System.Enum.GetValues (typeof(T));
		T tempValue = (T)tempArray.GetValue (Random.Range (0, tempArray.Length));
		return tempValue;
	}


	string RandomString(List<string> stringList)
	{
		int randomIndex = Random.Range (0, stringList.Count);
		string randomString = stringList [randomIndex];
		stringList.RemoveAt (randomIndex);
		return randomString;
	}
	*/


}
