using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyButton : MonoBehaviour {
	public GameObject lumbergh;
	public GameObject text;
	public string itemName;
	public int itemCost;
	public bool isUnlocked;



	void Start(){
		lumbergh = GameObject.Find("Lumbergh");
		if(PlayerPrefs.GetInt(itemName) == 1){
			Unlock();
		}
	}

	public void OnButtonClick(){
		Debug.Log("Requesting to purchase new block");
		lumbergh.GetComponent<Lumbergh>().MakePurchase(gameObject);
    }

	public void Unlock(){
		text.GetComponent<Text>().text = "owned";
	}
}
