using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBase : MonoBehaviour {

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
		//Debug.Log(tempMountPoints.Length);
	}


	void SetupCube(){
		PruneLowHangingFruit();
		Transform tempMount = mountPoints[UnityEngine.Random.Range(0, mountPoints.Length)].transform;
		indicatorActual = GameObject.Find("Indicator");
		indicatorActual.transform.parent = tempMount;
		lastIndicatorTime = Time.time;
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
