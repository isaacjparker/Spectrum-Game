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







	/// <summary>
	/// Initialises the grid lists for planet and marker placements.
	/// Also adds the available positions to those lists. Takes the map size and divides by the map scale
	/// to give grid positions. Adds those to list.
	/// </summary>
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


	/// <summary>
	/// Place the markers for debugging. Grid can be turned on from Unity editor
	/// otheriwse, will only show corner markers (to demonstrate size of map).
	/// </summary>
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
		starSystem.sunInstance = Instantiate (sunTile, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
	}


	/// <summary>
	/// Lays out the planet instances using the RandomPosition function. This is the initial layout
	/// based on a grid system and is modified by PositionOrbits().
	/// The tile Array is manually created using prefab planet tiles in Unity.
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

			//Instantiate that choice at the randomPosition defined and store it in an instanced object.
			starSystem.planets[i].instance = Instantiate (tileChoice, randomPosition, Quaternion.identity) as GameObject;
		}
	}


	/// <summary>
	/// Firstly, initialises the arrays for storing the different radii each planet will rotate around
	/// and the speeds of each planet.
	/// Then cycle through each planet (the number of which is defined by planets.Length)
	/// and attach the next radius (in increments of 10) to each planet and a random speed as
	/// created in RandomRotationSpeed function. Finally, position each planet on its radius. 
	/// The arrays are then used each frame in the OrbitPlanets
	/// function called from Update() in the GameManager.
	/// </summary>
	void PositionOrbits()
	{

		for (int x = 0; x < starSystem.planets.Length; x++) 
		{
			starSystem.planets[x].radiusPosition = new float();
			starSystem.planets[x].rotationSpeed = new float();
		}

		for (int j = 0; j < starSystem.planets.Length; j++) 
		{
			starSystem.planets[j].radiusPosition = starSystem.planets[j].radius + (j * 10);
			starSystem.planets[j].rotationSpeed = RandomRotationSpeed();
		}

		for (int i = 0; i < starSystem.planets.Length; i++) 
		{
			starSystem.planets [i].instance.transform.position = (starSystem.planets [i].instance.transform.position - new Vector3 (0, 0, 0)).normalized * starSystem.planets[i].radiusPosition + starSystem.sunInstance.transform.position;
		}
	}

	/// <summary>
	/// Randomises the rotation speed.
	/// </summary>
	/// <returns>A random rotation speed.</returns>
	float RandomRotationSpeed()
	{
		float randRotation = (Random.Range (-initRotationSpeed, initRotationSpeed)) + 0.1f;
		return randRotation;
	}


	/// <summary>
	/// Finds the transforms attached to the child object masks of each instance of a planet.
	/// Assigns that Transform to the mask variable within the same Planet class.
	/// This keeps each instance and it's mask in the same Planet clone.
	/// </summary>
	void SetupPlanetMasks()
	{

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


}
