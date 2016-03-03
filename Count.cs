using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Count {

	public int minimum;
	public int maximum;

	//This is a constructor, basically saying how the
	//class should be laid out when called and written in code
	//like a template.
	public Count (int min, int max)
	{
		minimum = min;
		maximum = max;
	}

}
