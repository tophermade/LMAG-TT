using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDetector : MonoBehaviour {

	public GameObject lumbergh;

	void OnTriggerEnter(){
		Debug.Log("Totem Fell");
		lumbergh.GetComponent<Lumbergh>().EndRound();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
