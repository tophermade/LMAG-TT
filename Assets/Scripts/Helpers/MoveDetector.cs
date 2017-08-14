using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDetector : MonoBehaviour {

	public GameObject lumbergh;

	void OnTriggerEnter(){
		Debug.Log("Totem Falling");
		lumbergh.SendMessage("EndRound");
	}

	// Use this for initialization
	void Start () {
		lumbergh = GameObject.Find("Lumbergh");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
