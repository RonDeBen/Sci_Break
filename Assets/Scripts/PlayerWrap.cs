using UnityEngine;
using System.Collections;

public class PlayerWrap : MonoBehaviour {
	private GameObject[] ww;
	private GameObject wwObj;
	private float screenHeight, screenWidth;
	// Use this for initialization
	void Start () {
		wwObj = gameObject;
		screenHeight = 2f * Camera.main.orthographicSize;
		screenWidth = screenHeight * Camera.main.aspect;
		SetupWorldWrap();
	}
	
	// Update is called once per frame
	void Update () {
		SwapShips();
	}

	void SetupWorldWrap(){
		ww = new GameObject[2];
		for(int k = 0; k < 2; k++){
			ww[k] = Instantiate(wwObj, Vector3.zero, Quaternion.identity) as GameObject;
			ww[k].GetComponent<PlayerWrap>().enabled = false;
		}
		PositionShips();
	}

	void PositionShips(){
		Vector3 ghostPos = transform.position;

		//right ghost
		ghostPos.x = transform.position.x + screenWidth;
		ww[0].transform.position = ghostPos;
		
		//left ghost
		ghostPos.x = transform.position.x - screenWidth;
		ww[1].transform.position = ghostPos;
	}

	void SwapShips(){
		foreach(GameObject ghost in ww){
			if(ghost.transform.position.x < screenWidth && ghost.transform.position.x > 0){
				transform.position = ghost.transform.position;
				PositionShips();
			}
		}
	}
}
