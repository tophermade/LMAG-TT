using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Lumbergh : MonoBehaviour {

	[Header ("Scene Object Refernces")]
	public GameObject[] cubes;
	public GameObject cubeParent;
	public GameObject starterCube;
	public GameObject mainCam;
	public GameObject indicator;
	public GameObject cameraSpike;


	[Header ("Instantiated References")]
	public GameObject currentActiveCube;


	[Header ("Component References")]
	public AudioSource audioSource;
	public PostProcessingProfile roundOverProfile;
	public PostProcessingProfile roundActiveProfile;


	[Header ("Sound Effects")]
	public AudioClip placeBlock;


	[Header ("Vectors")]
	public Vector3 trackerVectorStart = new Vector3(0,0,0);
	public Vector3 trackerVector = new Vector3(0,3.96f,-7f);


	[Header ("Booleans")]
	public bool playing = false;
	

	public void InitiateNewRound(){
	}

	public void InitiateRoundRestart(){
	}


	void UpdateCameraPosition(){
		if(playing){
			Vector3 tempVector = cameraSpike.transform.position;
			tempVector.y = Vector3.Lerp(cameraSpike.transform.position, trackerVector, Time.deltaTime * 2.5f).y;
			cameraSpike.transform.position = tempVector;			
			mainCam.transform.RotateAround(Vector3.zero, Vector3.up, 12f * Time.deltaTime);

			cameraSpike.transform.localScale = Vector3.Lerp(cameraSpike.transform.localScale, new Vector3(2,2,2), Time.deltaTime * 2.5f);
		} else {
			mainCam.transform.RotateAround(Vector3.zero, Vector3.up, 12f * Time.deltaTime);
			cameraSpike.transform.localScale = Vector3.Lerp(cameraSpike.transform.localScale, new Vector3(1,1,1), Time.deltaTime * 1.5f);
		}		
	}


	void BlockSpawn(){
		currentActiveCube.GetComponent<CubeBase>().canHaveIndicator = false;
		GameObject newCube = Instantiate(cubes[Random.Range(0, cubes.Length)], indicator.transform.position, Quaternion.identity);
		newCube.transform.parent = cubeParent.transform;
		currentActiveCube = newCube;
		newCube.transform.rotation = cubeParent.transform.rotation;
		if(newCube.transform.position.y > trackerVector.y){
			trackerVector = cameraSpike.transform.position;
			trackerVector.y = newCube.transform.position.y + 1f;
		}
		indicator.SetActive(false);
		audioSource.PlayOneShot(placeBlock, 1F);
		BroadcastMessage("TinyShake");
		//Debug.Log(tempIndicator.transform.position);
	}
	

	void SetupWorld(){
		currentActiveCube = GameObject.Find("StarterCube");
		indicator.SetActive(false);
	}


	public void EndRound(){
		Debug.Log("Round Ended");
		indicator.SetActive(false);
	}



	// button triggered functions
	public void RequestFirstStart(){
		starterCube.SendMessage("SetupCube");
		StartRound();
	}

	public void StartRound(){
		playing = true;
	}
	


	// Use this for initialization
	void Start () {
		SetupWorld();
	}
	
	// Update is called once per frame
	void Update () {
		if(playing){
			if(Input.GetMouseButtonDown(0)){
				BlockSpawn();
			}
		}

		UpdateCameraPosition();
	}	
}
