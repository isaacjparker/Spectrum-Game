using UnityEngine;
using System.Collections;

public class SP_CameraManager : MonoBehaviour {

	public float cameraSpeed;
	public float zoomMin = 10f;

	private float zoomMax;
	private float zoomStep;

	//private float leftBound;
	//private float rightBound;
	//private float topBound;
	//private float bottomBound;



	public void SetupCamera ()
	{
		zoomMax = 2f;
		zoomStep = 3f;

		//leftBound = mapSize * -1;
		//rightBound = mapSize;
		//topBound = mapSize;
		//bottomBound = mapSize * -1;

	}

	// Update is called once per frame
	void Update () {

		CameraMoveByKeys ();

		CameraScrollZoom ();

		//Debug.Log ("CamPos: " + Camera.main.transform.position.ToString ());


	}


	void CameraMoveByKeys()
	{

		Camera.main.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime, 0));

	}


	void CameraScrollZoom()
	{
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
		{
			if(Camera.main.orthographicSize < zoomMin)
			{
				Camera.main.orthographicSize += zoomStep;
			}



		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
		{
			if (Camera.main.orthographicSize > zoomMax)
			{
				Camera.main.orthographicSize -= zoomStep;
			}
		}
	}
}
