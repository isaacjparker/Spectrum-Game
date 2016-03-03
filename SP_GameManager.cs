using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class SP_GameManager : MonoBehaviour {

	public SP_ClusterManager clusterScript;
	//public SP_StarSystemManager starScript;
	public SP_CameraManager cameraScript;

	public Dropdown clusterDropDown;
	public Text text;

	public List <Cluster> cluster = new List<Cluster>();
	public int maxSystems;

	private int clusterIndex = 0;
	private int currentCluster = 0;
	private int systemIndex = 0;
	private bool clusterActive = false;


	void Awake ()
	{
		clusterScript = GetComponent<SP_ClusterManager> ();
		//starScript = GetComponent<SP_StarSystemManager>();
		cameraScript = GetComponent<SP_CameraManager>();

		cameraScript.SetupCamera();

		//clusterDropDown.ClearOptions ();


	}

	/// <summary>
	/// Main Update loop. Call ParallaxEffect and RotatePlanetMasks
	/// to update the planet shadows in realtime.
	/// </summary>
	void Update()
	{

		if (clusterActive == true) 
		{
			OrbitPlanets ();
		}

	}


	public void NewCluster()
	{
		//starSystems [systemIndex] = starScript.BuildStarSystem (systemIndex, cluster.systemNameList);

		cluster.Add (new Cluster ());

		cluster[clusterIndex] = clusterScript.BuildCluster (systemIndex, maxSystems);

		clusterActive = true;
		currentCluster = clusterIndex;

		clusterDropDown.options.Add (new Dropdown.OptionData() { text = clusterIndex.ToString () });

		systemIndex = systemIndex + cluster[clusterIndex].clusterSize;
		clusterIndex = clusterIndex + 1;

		RefreshDropDown ();

	}


	public void DestroyCluster()
	{
		if (clusterDropDown.value != null) 
		{
			clusterDropDown.options.RemoveAt (clusterDropDown.value);

			RefreshDropDown ();
		}

	}


	void RefreshDropDown()
	{
		if (clusterDropDown.value > 0) {
			int tempIndex = clusterDropDown.value - 1;
			clusterDropDown.value = 0;
			clusterDropDown.value = tempIndex;
		} 
		else 
		{
			clusterDropDown.value = 0;
		}

	}


	/// <summary>
	/// Carries out the function of orbiting the planets around the sun (usually position 0,0,0).
	/// The rotation speed array was filled in PositionOrbits function and can be negative or posiitve
	/// depending on the desired direction of orbit.
	/// </summary>
	void OrbitPlanets()
	{
		if (cluster [currentCluster] != null) {
			for (int i = 0; i < cluster [currentCluster].starSystems.Count; i++) 
			{
				if (cluster [currentCluster].starSystems [i] != null) 
				{
					for (int j = 0; j < cluster [currentCluster].starSystems [i].planets.Length; j++) 
					{
						if (cluster [currentCluster].starSystems [i].planets [j] != null) 
						{
							cluster [currentCluster].starSystems [i].planets [j].instance.transform.RotateAround (new Vector3 (0, 0, 0), Vector3.forward, cluster [currentCluster].starSystems [i].planets [j].rotationSpeed * Time.deltaTime);

							cluster [currentCluster].starSystems [i].planets [j].desiredPosition = (cluster [currentCluster].starSystems [i].planets [j].instance.transform.position - new Vector3 (0, 0, 0)).normalized * cluster [currentCluster].starSystems [i].planets [j].radius + cluster [currentCluster].starSystems [i].sunInstance.transform.position;

							cluster [currentCluster].starSystems [i].planets [j].instance.transform.position = Vector3.MoveTowards (cluster [currentCluster].starSystems [i].planets [j].instance.transform.position, cluster [currentCluster].starSystems [i].planets [j].desiredPosition, Time.deltaTime * cluster [currentCluster].starSystems [i].planets [j].radiusSpeed);
						}
					}
				}
			}
		}
	}


}
