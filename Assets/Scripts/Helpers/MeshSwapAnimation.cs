using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSwapAnimation : MonoBehaviour {

	public GameObject[] states;
	public float timeBetweenStates = .25f;
	
	private int currentState = 0;
	private float lastStateChangeTime = 0f;

	public void UpdateState(){
		if(Time.time > lastStateChangeTime + timeBetweenStates){
			states[currentState].SetActive(false);
			if(currentState == states.Length - 1){
				currentState = 0;
			} else {
				currentState++;
			}
			states[currentState].SetActive(true);
			lastStateChangeTime = Time.time;
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		UpdateState();		
	}
}
