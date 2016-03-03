using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SP_ClusterManager : MonoBehaviour {

	public SP_StarSystemManager starScript;

	private Cluster cluster;
	private int startingIndex;

	void Awake()
	{
		starScript = GetComponent<SP_StarSystemManager> ();
	}

	public Cluster BuildCluster(int index, int maxSystems)
	{
		cluster = new Cluster();
		startingIndex = index;
		//cluster.clusterSize = Random.Range (0, maxSystems);		//Use this to randomise the number of systems in each cluster.
		cluster.clusterSize = maxSystems;
		cluster.systemNameList = PopulateNameList<Enums.SystemNames> ();



		for (int i = 0; i < cluster.clusterSize; i++) 
		{
			cluster.starSystems.Add (new StarSystem ());
		}

		for (int j = 0; j < cluster.starSystems.Count; j++) 
		{
			cluster.starSystems [j] = starScript.BuildStarSystem(startingIndex, j, cluster.systemNameList);

			cluster.starSystems [j].systemIndex = startingIndex + j;
			cluster.starSystems [j].systemName = ChooseSystemName ();


			Debug.Log ("System Index: " + cluster.starSystems [j].systemIndex.ToString ());
			Debug.Log ("System Name: " + cluster.starSystems [j].systemName);
		}

		return cluster;

	}


	/// <summary>
	/// Randomly chooses the name of a system from a List of strings.
	/// Then removes that name from the List so as not to be chosen again.
	/// </summary>
	/// <returns>The system name.</returns>
	string ChooseSystemName()
	{
		int randomIndex = Random.Range (0, cluster.systemNameList.Count);
		string randomString = cluster.systemNameList [randomIndex];
		cluster.systemNameList.RemoveAt (randomIndex);
		return randomString;
	}


	/// <summary>
	/// Takes an enum of potential names, converts each constant into a string
	/// then adds it to a List of strings
	/// </summary>
	/// <returns>A List of strings containing all names from an enum</returns>
	/// <typeparam name="T">Enum Parameter</typeparam>
	static List<string> PopulateNameList<T>()
	{
		List <string> tempList = new List<string>();
		System.Array tempArray = System.Enum.GetValues (typeof(T));

		for (int i = 0; i < tempArray.Length; i++) 
		{
			tempList.Add (tempArray.GetValue (i).ToString());
		}

		return tempList;
	}


}
