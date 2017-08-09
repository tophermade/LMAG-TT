using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumbergh : MonoBehaviour {

	public GameObject[] cubes;
	public GameObject cubeParent;
	public GameObject mainCam;
	public GameObject indicator;


	public GameObject currentActiveCube;


	public Vector3 trackerVector = new Vector3(0,3,-11.5f);


	public void InitiateNewRound(){
	}

	public void InitiateRoundRestart(){
	}


	void UpdateCameraPosition(){
		//mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, trackerVector, Time.deltaTime * 2.5f);
		mainCam.transform.RotateAround(Vector3.zero, Vector3.up, 12f * Time.deltaTime);
	}


	void BlockSpawn(){
		currentActiveCube.GetComponent<CubeBase>().canHaveIndicator = false;
		GameObject newCube = Instantiate(cubes[Random.Range(0, cubes.Length)], indicator.transform.position, Quaternion.identity);
		newCube.transform.parent = cubeParent.transform;
		currentActiveCube = newCube;
		newCube.transform.rotation = cubeParent.transform.rotation;
		if(newCube.transform.position.y > trackerVector.y){
			trackerVector = mainCam.transform.position;
			trackerVector.y = newCube.transform.position.y + 1f;
		}
		indicator.SetActive(false);
		//Debug.Log(tempIndicator.transform.position);
	}
	

	void SetupWorld(){
		currentActiveCube = GameObject.Find("StarterCube");
	}


	public void EndRound(){
		Debug.Log("Round Ended");
		indicator.SetActive(false);
	}
	


	// Use this for initialization
	void Start () {
		SetupWorld();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			BlockSpawn();
		}

		UpdateCameraPosition();
	}	
}
