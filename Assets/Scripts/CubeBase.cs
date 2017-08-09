using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBase : MonoBehaviour {
	
	public GameObject lumbergh;
	private GameObject indicatorActual;
	public GameObject baseObject;
	public GameObject[] mountPoints;


	public float jumpTime = .8f;
	public float lastIndicatorTime = 0f;


	public bool canHaveIndicator = true;


	void MoveIndicator(){
		if(Time.time > lastIndicatorTime + jumpTime){
			indicatorActual.transform.position = mountPoints[UnityEngine.Random.Range(0, mountPoints.Length)].transform.position;
			lastIndicatorTime = Time.time;
		}
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


	void SetupCube(){
		PruneLowHangingFruit();
		CheckAvailablePositions();
		if(mountPoints.Length > 0){
			Transform tempMount = mountPoints[UnityEngine.Random.Range(0, mountPoints.Length)].transform;
			lumbergh = GameObject.Find("Lumbergh");
			indicatorActual = lumbergh.GetComponent<Lumbergh>().indicator;
			indicatorActual.transform.position = tempMount.position;
			indicatorActual.transform.parent = tempMount;
			//StartCoroutine(EnableIndicator());
			indicatorActual.SetActive(true);
			lastIndicatorTime = Time.time;
		} else {
			canHaveIndicator = false;
		}
	}


	IEnumerator EnableIndicator(){
		yield return new WaitForSeconds(.25f);
		indicatorActual.SetActive(true);
	}


	// Use this for initialization
	void Start () {
		SetupCube();
	}

	
	// Update is called once per frame
	void Update () {
		if(canHaveIndicator){
			MoveIndicator();
		}
	}
}
