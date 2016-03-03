using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet {

	//instance holds the instantiated clone of a planet
	//mask holds the shadow mask and finds it's prefab from within the children
	//of instance through the SetupPlanetMasks function in SP_StarSystemManager
	public GameObject instance;
	public Transform mask;


	//Orbit variables
	public float radius = 40.0f;
	public float radiusPosition;
	public float radiusSpeed = 0.1f;
	public float initRotationSpeed = 4.0f;
	public float rotationSpeed;
	public Vector3 desiredPosition;
}
