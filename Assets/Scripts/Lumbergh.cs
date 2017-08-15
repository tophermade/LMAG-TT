using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class Lumbergh : MonoBehaviour {

	[Header ("Prefab References")]
	public GameObject[] cubes;


	[Header ("Scene Object References")]
	public GameObject baseCube;
	public GameObject camSpike;
	public GameObject mainCam;
	public GameObject indicator;
	public GameObject detector;
	public GameObject cubeParent;


	[Header ("Component References")]
	public AudioSource audioSource;


	[Header ("UI Object References")]
	public GameObject[] scenePanels;
	public GameObject bonusImage;
	public GameObject bonusImageCount;
	public Sprite bonusOf5Sprite;
	public Sprite bonusOf10Sprite;
	public Sprite bonusOf20Sprite;
	public Sprite bonusOf50Sprite;


	[Header ("UI Text References")]
	public GameObject mainScoreDisplay;
	public GameObject mainCoinDisplay;
	public GameObject buttonCoinDisplay;
	public GameObject shopCoinDisplay;


	[Header ("Audio Clips")]
	public AudioClip placeblock;
	public AudioClip clickButton;
	public AudioClip bonusSound;


	[Header ("Instantiated Object References")]
	public GameObject currentCube;


	[Header ("Booleans")]
	public bool playing = false;


	[Header ("Vectors")]
	public Vector3 trackerVector = new Vector3(0,3.96f,-7f);


	[Header ("Integers")]
	public int score = 0;
	public int coins;
	public int stackStreak = 0;
	public int roundcount = 0;


	[Header ("Floats")]
	public float indicatorMoveDelay = .75f;
	public float lastIndicatorMoveTime = 0f;
	public float indicatorMoveDelayModifier = 0;
	

	[Header ("Strings")]
	public string activePanelName = "MainMenu";
	public string lastActivePanelName;


	void Start(){
		EstablishCoinCount();
	}

	void EstablishCoinCount(){
		if(PlayerPrefs.HasKey("firstRun")){
			if(PlayerPrefs.HasKey("coins")){
				coins  = PlayerPrefs.GetInt("coins");
			} else {
				coins = 0;
			}
		} else {
			coins = 25;
			PlayerPrefs.SetInt("firstRun",0);
		}
		
		mainCoinDisplay.GetComponent<Text>().text = coins.ToString("D4");
		buttonCoinDisplay.GetComponent<Text>().text = coins.ToString("D4");
		shopCoinDisplay.GetComponent<Text>().text = coins.ToString("D4");
	}
	

	void ShowShop(){
		ManageCanvas("Shop");
	}

	void HideShop(){
		ManageCanvas(lastActivePanelName);
	}

	void UpdateCameraPosition(){
		if(playing){
			Vector3 tempVector = camSpike.transform.position;
			tempVector.y = Vector3.Lerp(camSpike.transform.position, trackerVector, Time.deltaTime * 2.5f).y;
			camSpike.transform.position = tempVector;			
			mainCam.transform.RotateAround(Vector3.zero, Vector3.up, 12f * Time.deltaTime);
			camSpike.transform.localScale = Vector3.Lerp(camSpike.transform.localScale, new Vector3(2,2,2), Time.deltaTime * 2.5f);
		} else {
			mainCam.transform.RotateAround(Vector3.zero, Vector3.up, 12f * Time.deltaTime);
			camSpike.transform.localScale = Vector3.Lerp(camSpike.transform.localScale, new Vector3(1,1,1), Time.deltaTime * 1.5f);
		}
	}

	void ManageCanvas(string makeActive){
		lastActivePanelName = activePanelName;
		activePanelName = makeActive;
		
		foreach(GameObject panel in scenePanels){
			if(panel.transform.name != makeActive){
				panel.SetActive(false);
			} else {
				panel.SetActive(true);
			}
		}
	}

	IEnumerator ManageCanvasWithDelay(string makeActive, float delay){
		yield return new WaitForSeconds(delay);
		lastActivePanelName = activePanelName;
		activePanelName = makeActive;

		foreach(GameObject panel in scenePanels){
			if(panel.transform.name != makeActive){
				panel.SetActive(false);
			} else {
				panel.SetActive(true);
			}
		}
	}

	IEnumerator RemoveAddedCubes(){
		yield return new WaitForSeconds(1.25f);
		foreach(Transform cube in cubeParent.transform){
			Destroy(cube.gameObject);
		}
		baseCube.SetActive(false);
	}

	IEnumerator DisableAfterTime(GameObject disableThis, float afterTime){
		yield return new WaitForSeconds(.6f);
		disableThis.SetActive(false);
	}

	void EndRound(){
		if(playing){
			Debug.Log("Round being ended");
			playing = false;
			stackStreak = 0;
			indicatorMoveDelayModifier = 0;

			currentCube.GetComponent<CubeBase>().cubeIsActive = false;
			indicator.SetActive(false);

			if(roundcount == 0){
				BroadcastMessage("ShowInterstertial");
			}

			StartCoroutine(ManageCanvasWithDelay("GameOver", 1.25f));
			StartCoroutine(RemoveAddedCubes());

			indicator.transform.parent = baseCube.transform;
			indicator.transform.position = new Vector3(0,1.2f,0);
		}
	}

	void PrepareForNewRound(){
		currentCube = baseCube;
		currentCube.GetComponent<CubeBase>().cubeIsActive = true;
		currentCube.transform.rotation = Quaternion.identity;
		currentCube.transform.position = new Vector3(0,0,0);
		currentCube.SetActive(true);
	}

	void StartNewRound(){
		playing = true;
		indicator.SetActive(true);
		ResetScore();
	}

	void PlaceNewCube(){
		Debug.Log("Placing new cube");
		float tempY = currentCube.transform.position.y + .4f;

		audioSource.PlayOneShot(placeblock, .6f);

		currentCube.GetComponent<CubeBase>().cubeIsActive = false;
		lastIndicatorMoveTime = 0;

		GameObject newCube = Instantiate(cubes[0], indicator.transform.position, Quaternion.identity);
		newCube.transform.parent = cubeParent.transform;
		newCube.transform.rotation = baseCube.transform.rotation;
		currentCube = newCube;

		Vector3 tempVector = trackerVector;
		tempVector.y = currentCube.transform.position.y + 2.5f;
		trackerVector = tempVector;

		if(currentCube.transform.position.y > tempY){
			stackStreak++;
		} else {
			stackStreak = 0;
		}

		indicatorMoveDelayModifier = indicatorMoveDelayModifier + .018f;
		if(indicatorMoveDelayModifier < .15f){
			indicatorMoveDelayModifier = .15f;
		}

		IncreaseScore(1);
		IncreaseCoins(1);
		CheckForBonus();
	}
	
	void IncreaseScore(int addAmount){
		score = score + addAmount;
		mainScoreDisplay.GetComponent<Text>().text = score.ToString("D3");
	}

	void ResetScore(){
		score = 0;
		mainScoreDisplay.GetComponent<Text>().text = score.ToString("D3");
	}

	void IncreaseCoins(int addAmount){
		coins = coins + addAmount;
		mainCoinDisplay.GetComponent<Text>().text = coins.ToString("D4");
		buttonCoinDisplay.GetComponent<Text>().text = coins.ToString("D3");
		shopCoinDisplay.GetComponent<Text>().text = coins.ToString("D4");
		PlayerPrefs.SetInt("coins", coins);
	}

	void SpendCoins(int spendAmount){
		coins = coins - spendAmount;
		mainCoinDisplay.GetComponent<Text>().text = coins.ToString("D4");
		buttonCoinDisplay.GetComponent<Text>().text = coins.ToString("D3");
		shopCoinDisplay.GetComponent<Text>().text = coins.ToString("D4");
		PlayerPrefs.SetInt("coins", coins);
	}

	void ApplyReward(){
		IncreaseCoins(50);
	}

	void CheckForBonus(){
		bool awardStreak = false;
		Sprite awardSprite = bonusOf5Sprite;

		if(stackStreak  == 2){
			awardStreak = true;
			IncreaseCoins(5);
		} else if(stackStreak  == 7){
			awardStreak = true;
			awardSprite = bonusOf10Sprite;
			IncreaseCoins(10);
		} if(stackStreak  == 15){
			awardStreak = true;
			awardSprite = bonusOf20Sprite;
			IncreaseCoins(20);
		} if(stackStreak  == 30){
			awardStreak = true;
			awardSprite = bonusOf50Sprite;
			IncreaseCoins(50);
		}

		if(awardStreak){
			bonusImage.SetActive(true);
			bonusImageCount.GetComponent<Image>().sprite = awardSprite;
			StartCoroutine(DisableAfterTime(bonusImage, .4f));
			audioSource.PlayOneShot(bonusSound, .5f);
		}
	}

	public void MakePurchase(GameObject buyButton){
		BuyButton buyingFrom = buyButton.GetComponent<BuyButton>();
		if(coins >= buyingFrom.itemCost){
			SpendCoins(buyingFrom.itemCost);
			PlayerPrefs.SetInt(buyingFrom.itemName, 1);
			buyingFrom.Unlock();
		}
	}
	


	/// 
	/// Called by button presses
	///
	public void StartRound(){
		Debug.Log("Starting round...");
		playing = true;
		ManageCanvas("Play");
	}


	public void RequestPlaceCube(){
		Debug.Log("Requsting cube to be placed");
		if(playing){
			PlaceNewCube();
		}
	}


	public void RequestRestart(){
		Debug.Log("Requesting a new round");
		PrepareForNewRound();
		StartNewRound();
		ManageCanvas("Play");
	}


	public void PlayButtonSound(){
		audioSource.PlayOneShot(clickButton, 1f);
	}

	void Update(){
		UpdateCameraPosition();
	}
	

}
