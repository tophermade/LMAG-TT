using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBase : MonoBehaviour {

	[Header ("Scene Object References")]
	public GameObject lumbergh;
	public GameObject baseCube;
	public GameObject[] mountPoints;

	[Header ("Component References")]
	public Lumbergh manager;


	[Header ("Booleans")]
	public bool cubeIsActive = false;


	[Header ("integers")]
	public int currentMount = -1;


	void Awake(){
		if(gameObject.name != "StarterCube"){
			lumbergh = GameObject.Find("Lumbergh");
			baseCube = GameObject.Find("StarterCube");
		}

		manager = lumbergh.GetComponent<Lumbergh>();
	}

	void Start(){
		cubeIsActive = true;
		CheckAvailablePositions();
		PruneLowHangingFruit();
	}

	void CheckAvailablePositions(){
		GameObject[] tempMountPoints = new GameObject[mountPoints.Length];
		int tempMountCount = 0;

		foreach(GameObject mountPoint in mountPoints){
			if (!Physics.Linecast(transform.position, mountPoint.transform.position)){
				tempMountPoints[tempMountCount] = mountPoint;
				tempMountCount++;
			}            
		}

		Array.Resize(ref tempMountPoints, tempMountCount);
		mountPoints = tempMountPoints;
	}


	void PruneLowHangingFruit(){
		GameObject[] tempMountPoints = new GameObject[mountPoints.Length];
		int tempMountCount = 0;

		foreach(GameObject mountPoint in mountPoints){
			if(mountPoint.transform.position.y > -.02f){
				tempMountPoints[tempMountCount] = mountPoint;
				tempMountCount++;
			}
		}
		Array.Resize(ref tempMountPoints, tempMountCount);
		mountPoints = tempMountPoints;
	}


	int GetNewMount(int notThis){
		int newDir;
		newDir = UnityEngine.Random.Range(0,mountPoints.Length);
		while (newDir == notThis){
			newDir = UnityEngine.Random.Range(0,mountPoints.Length);
		}
		return newDir;
	}


	void MoveIndicator(){
		if(cubeIsActive && Time.time >  manager.lastIndicatorMoveTime + (manager.indicatorMoveDelay - manager.indicatorMoveDelayModifier) && mountPoints.Length > 0){
			currentMount = GetNewMount(currentMount);
			manager.indicator.transform.position = mountPoints[currentMount].transform.position;
			manager.indicator.transform.parent = mountPoints[currentMount].transform;
			manager.lastIndicatorMoveTime = Time.time;
		}
	}


	void OnCollisionEnter(Collision other){
		if(other.transform.name == "Island"){
			Debug.Log("Emmit island collision sound");
		}
	}

	
	void Update(){
		MoveIndicator();
	}



}
