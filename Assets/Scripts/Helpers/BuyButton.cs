using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyButton : MonoBehaviour {
	public Button button;
	public GameObject lumbergh;
	public GameObject text;
	public GameObject unlockThisBlock;
	public string itemName;
	public int itemCost;
	public bool isUnlocked;



	void Start(){
		button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);

		lumbergh = GameObject.Find("Lumbergh");
		itemName = unlockThisBlock.name;
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
