using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet {

	public GameObject instance = new GameObject();
	public Transform mask;


	//Orbit variables
	public float radius = 40.0f;
	public float radiusPosition;
	public float radiusSpeed = 0.1f;
	public float initRotationSpeed = 4.0f;
	public float rotationSpeed;
	public Vector3 desiredPosition;
}
