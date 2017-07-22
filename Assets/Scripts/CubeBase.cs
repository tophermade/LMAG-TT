using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBase : MonoBehaviour {

	public GameObject indicator;
	private GameObject indicatorActual;
	public GameObject baseObject;
	public GameObject[] mountPoints;


	public float jumpTime = .8f;



	void SetupCube(){
		Transform tempMount = mountPoints[Random.Range(0, mountPoints.Length)].transform;
		indicatorActual = Instantiate(indicator, tempMount.position,Quaternion.identity);
		indicatorActual.transform.parent = tempMount;
	}


	// Use this for initialization
	void Start () {
		SetupCube();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
