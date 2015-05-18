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

		if(wwObj.tag.Equals("Player")){
			SpriteRenderer spr = wwObj.GetComponentInChildren<SpriteRenderer>();

			BoxCollider2D bc2d = wwObj.GetComponent<BoxCollider2D>();

			float newWidth, newHeight;
			
			if(spr.bounds.size.y > spr.bounds.size.x){
				Transform tr = wwObj.GetComponentInChildren<Transform>();
				tr.transform.Rotate(0,0,-90);
				bc2d.size = new Vector3(spr.bounds.size.y, spr.bounds.size.x, 0f);
				newWidth = (screenWidth * 0.33f) / spr.bounds.size.x;
				newHeight = (screenHeight * 0.2f) / spr.bounds.size.y;
				transform.localScale = new Vector3(newWidth, newHeight, 1f);
				transform.position = new Vector3(transform.position.x, spr.bounds.size.y / 2, transform.position.z);
			}else{
				bc2d.size = spr.bounds.size;
				newWidth = (screenWidth * 0.33f) / spr.bounds.size.x;
				newHeight = (screenHeight * 0.2f) / spr.bounds.size.y;
				transform.localScale = new Vector3(newWidth, newHeight, 1f);
				transform.position = new Vector3(transform.position.x, spr.bounds.size.y / 2, transform.position.z);
			}
		}

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
			ww[k].GetComponent<Transform>().rotation = wwObj.GetComponentInChildren<Transform>().rotation;
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
