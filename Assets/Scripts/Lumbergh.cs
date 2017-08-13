using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Lumbergh : MonoBehaviour {

	[Header ("Prefab References")]
	public GameObject[] cubes;


	[Header ("Scene Object References")]
	public GameObject baseCube;
	public GameObject camSpike;
	public GameObject mainCam;
	public GameObject indicator;
	public GameObject detector;

	[Header ("Instantiated Object References")]
	public GameObject currentCube;

	[Header ("UI Object References")]
	public GameObject[] scenePanels;
	public GameObject mainPanel;
	public GameObject playPanel;
	public GameObject gameOverPanel;
	public GameObject shopPanel;


	[Header ("Booleans")]
	public bool playing = false;


	[Header ("Vectors")]
	public Vector3 trackerVector = new Vector3(0,3.96f,-7f);


	[Header ("Floats")]
	public float indicatorMoveDelay = .75f;
	public float lastIndicatorMoveTime = 0f;


	void Start(){

	}

	void UpdateCameraPosition(){
		if(playing){
			Vector3 tempVector = camSpike.transform.position;
			tempVector.y = Vector3.Lerp(camSpike.transform.position, trackerVector, Time.deltaTime * 2.5f).y;
			camSpike.transform.position = tempVector;			
			mainCam.transform.RotateAround(Vector3.zero, Vector3.up, 12f * Time.deltaTime);
			camSpike.transform.localScale = Vector3.Lerp(camSpike.transform.localScale, new Vector3(2,2,2), Time.deltaTime * 2.5f);
		} else {
			mainCam.transform.RotateAround(Vector3.zero, Vector3.up, 12f * Time.deltaTime);
			camSpike.transform.localScale = Vector3.Lerp(camSpike.transform.localScale, new Vector3(1,1,1), Time.deltaTime * 1.5f);
		}
	}

	void ManageCanvas(string makeActive){
		foreach(GameObject panel in scenePanels){
			if(panel.transform.name != makeActive){
				panel.SetActive(false);
			} else {
				panel.SetActive(true);
			}
		}
	}


	/// 
	/// Called by button presses
	///
	public void StartRound(){
		Debug.Log("Starting round...");
		playing = true;
		ManageCanvas("Play");
	}

	public void StartNewRound(){
		Debug.Log("Starting new round...");
	}

	public void RequestPlaceCube(){
		Debug.Log("Requsting cube to be placed");
	}



	void Update(){
		UpdateCameraPosition();
	}


}
