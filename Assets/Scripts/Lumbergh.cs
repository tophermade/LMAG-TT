using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumbergh : MonoBehaviour {


	public GameObject cube;
	public GameObject cubeParent;



	void BlockSpawn(){
		GameObject tempIndicator = GameObject.Find("Indicator(Clone)");
		GameObject newCube = Instantiate(cube, tempIndicator.transform.position, Quaternion.identity);
		newCube.transform.parent = cubeParent.transform;
		//Debug.Log(tempIndicator.transform.position);
	}
	

	void SetupWorld(){
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
	}	
}
