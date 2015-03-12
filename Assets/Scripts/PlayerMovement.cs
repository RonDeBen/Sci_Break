using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed;

	void Awake(){
		float screenHeight = 2f * Camera.main.orthographicSize;
		float screenWidth = screenHeight * Camera.main.aspect;
		transform.position = new Vector3(screenWidth / 2f, transform.position.y, transform.position.z);
	}

	void Update () {
		bool left = Input.GetKey("left");
		bool right = Input.GetKey("right");

		if(left){
			GetComponent<Rigidbody2D>().velocity = Vector2.right * -speed;
		}
		if(right){
			GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
		}
		if(!(left || right)){
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}
}
