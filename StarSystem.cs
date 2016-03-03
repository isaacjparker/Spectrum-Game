using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarSystem {

	public int systemIndex;
	public string systemName;

	public float mapSize;
	public float mapScale;
	public Count  planetCount = new Count (5,5);

	public Vector3 spawnOffset = new Vector3(0, 0, 0);

	//Spawned Instances of planets and sun
	public Planet[] planets;
	public GameObject sunInstance;

	public float[] radiusPositions;			//use this to track the different radii a planet can orbit around.

	//Planet Shadow Masks
	public GameObject[] planetMaskArray;

}
