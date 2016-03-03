using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cluster {

	public Enums.SystemNames nameList;
	public List<string> systemNameList;

	public List<StarSystem> starSystems = new List<StarSystem>();

	public int clusterSize;

	public bool isActive = false;

	public int clusterIndex;
}
