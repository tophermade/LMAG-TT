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
			indicatorActual.transform.position = mountPoints[Random.Range(0, mountPoints.Length)].transform.position;
			lastIndicatorTime = Time.time;
		}
	}


	void SetupCube(){
		Transform tempMount = mountPoints[Random.Range(0, mountPoints.Length)].transform;
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
