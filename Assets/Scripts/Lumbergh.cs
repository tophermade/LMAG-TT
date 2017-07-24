using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumbergh : MonoBehaviour {

	public GameObject[] cubes;
	public GameObject cubeParent;


	public GameObject currentActiveCube;



	void BlockSpawn(){
		currentActiveCube.GetComponent<CubeBase>().canHaveIndicator = false;

		GameObject tempIndicator = GameObject.Find("Indicator");
		GameObject newCube = Instantiate(cubes[Random.Range(0, cubes.Length)], tempIndicator.transform.position, Quaternion.identity);
		newCube.transform.parent = cubeParent.transform;
		currentActiveCube = newCube;
		//Debug.Log(tempIndicator.transform.position);
	}
	

	void SetupWorld(){
		currentActiveCube = GameObject.Find("StarterCube");
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
