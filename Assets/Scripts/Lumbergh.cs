﻿using System.Collections;
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


	[Header ("Menu Panels")]
	public GameObject mainMenuPanel;
	public GameObject playPanel;
	public GameObject gameOverPanel;
	public GameObject shopPanel;


	[Header ("Instantiated References")]
	public GameObject currentActiveCube;


	[Header ("Component References")]
	public AudioSource audioSource;
	public PostProcessingProfile roundOverProfile;
	public PostProcessingProfile roundActiveProfile;

	public Animator mainMenuPanelAnimator;
	public Animator playPanelAnimator;
	public Animator gameOverPanelAnimator;
	public Animator shopPanelAnimator;


	[Header ("Sound Effects")]
	public AudioClip placeBlock;
	public AudioClip buttonTap;


	[Header ("Vectors")]
	public Vector3 initialBasePosition;
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
		if(currentActiveCube != null){
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
	}
	

	void SetupWorld(){
		currentActiveCube = GameObject.Find("StarterCube");
		indicator.SetActive(false);
	}


	public void EndRound(){
		Debug.Log("Round Ended");
		BroadcastMessage("DoShake");

		indicator.SetActive(false);
		playing = false;
		
		//StartCoroutine(ShowGameOverScreen());
		playPanel.SetActive(false);
		gameOverPanel.SetActive(true);

		//StartCoroutine(SetupForNextRound());		
		starterCube.transform.position = initialBasePosition;
		starterCube.transform.eulerAngles = new Vector3(0,0,0);
		starterCube.GetComponent<CubeBase>().Reset();
		currentActiveCube = starterCube;		
		indicator.transform.parent = null;
		indicator.transform.position = new Vector3(0,1.24f,0);

		//StartCoroutine(DestroyCubes());
		foreach(Transform cube in cubeParent.transform){
			Destroy(cube.gameObject);
		}

		//gameOverPanelAnimator.SetTrigger("In");
	}

	IEnumerator ShowGameOverScreen(){
		yield return new WaitForSeconds(.7f);
		playPanel.SetActive(false);
		gameOverPanel.SetActive(true);
		//gameOverPanelAnimator.SetTrigger("In");
	}

	IEnumerator SetupForNextRound(){
		yield return new WaitForSeconds(.7f);
		
		starterCube.transform.position = initialBasePosition;
		starterCube.transform.eulerAngles = new Vector3(0,0,0);
		starterCube.GetComponent<CubeBase>().Reset();
		currentActiveCube = starterCube;
		
		indicator.transform.parent = null;
		indicator.transform.position = new Vector3(0,1.24f,0);		
	}

	IEnumerator DestroyCubes(){
		yield return new WaitForSeconds(.7f);
		foreach(Transform cube in cubeParent.transform){
			Destroy(cube.gameObject);
		}
	}


	public void RestartRound(){
		mainMenuPanel.SetActive(false);
		indicator.SetActive(true);
		StartRound();
	}



	// button triggered functions
	public void RequestFirstStart(){
		starterCube.SendMessage("SetupCube");
		StartRound();
	}

	public void StartRound(){
		//gameOverPanelAnimator.SetTrigger("Out");
		gameOverPanel.SetActive(false);

		if(mainMenuPanel.activeSelf){
			mainMenuPanelAnimator.SetTrigger("Out");
		}
		
		playPanel.SetActive(true);
		playPanelAnimator.SetTrigger("In");

		playing = true;
	}

	public void ShowShop(){
		shopPanel.SetActive(true);
		
		if(mainMenuPanel.activeSelf){
			mainMenuPanelAnimator.SetTrigger("Out");
			StartCoroutine(DisablePanel(mainMenuPanel, .5f));
		}
		if(playPanel.activeSelf){
			playPanelAnimator.SetTrigger("Out");
			StartCoroutine(DisablePanel(playPanel, .5f));
		}
		if(gameOverPanel.activeSelf){
			gameOverPanelAnimator.SetTrigger("Out");
			StartCoroutine(DisablePanel(gameOverPanel, .5f));			
		}

		shopPanelAnimator.SetTrigger("In");
	}

	public void HideShop(){
		mainMenuPanel.SetActive(true);
		DisablePanel(shopPanel, .5f);		
		
		shopPanelAnimator.SetTrigger("Out");
		mainMenuPanelAnimator.SetTrigger("In");
	}

	IEnumerator DisablePanel(GameObject panel, float delay){
		yield return new WaitForSeconds(delay);
		panel.SetActive(false);
	}

	public void PlaceBlock(){
		if(playing){
			BlockSpawn();
		}
	}

	

	// sound functions
	public void PlayButtonSound(){
		audioSource.PlayOneShot(placeBlock, 1F);
	}
	


	// Use this for initialization
	void Awake(){
		initialBasePosition = starterCube.transform.position;
	}

	void Start () {
		SetupWorld();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateCameraPosition();
	}	
}
