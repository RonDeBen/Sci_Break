using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed;

	void Awake(){
		float screenHeight = 2f * Camera.main.orthographicSize;
		float screenWidth = screenHeight * Camera.main.aspect;
		transform.position = new Vector3(screenWidth / 2f, transform.position.y, transform.position.z);
		
		if(gameObject.tag.Equals("Player")){
			SpriteRenderer spr = gameObject.GetComponentInChildren<SpriteRenderer>();
			BoxCollider2D bc2d = gameObject.GetComponent<BoxCollider2D>();

			float newWidth, newHeight;
			
			if(spr.bounds.size.y > spr.bounds.size.x){
				Transform tr = gameObject.GetComponentInChildren<Transform>();
				tr.transform.Rotate(0,0,-90);
				bc2d.size = new Vector3(spr.bounds.size.y, spr.bounds.size.x, 0f);
				newWidth = (screenWidth * 0.33f) / spr.bounds.size.x;
				newHeight = (screenHeight * 0.2f) / spr.bounds.size.y;
				transform.position = new Vector3(transform.position.x, spr.bounds.size.y / 2, transform.position.z);
				transform.localScale = new Vector3(newWidth, newHeight, 1f);
				//bc2d.center = spr.bounds.center;
			}else{
				//bc2d.center = spr.bounds.center;
				bc2d.size = spr.bounds.size;
				newWidth = (screenWidth * 0.33f) / spr.bounds.size.x;
				newHeight = (screenHeight * 0.2f) / spr.bounds.size.y;
				transform.position = new Vector3(transform.position.x, spr.bounds.size.y / 2, transform.position.z);
				transform.localScale = new Vector3(newWidth, newHeight, 1f);
			}
		}
	}

	void Update () {
		bool left = Input.GetKey("left");
		bool right = Input.GetKey("right");

		if(left){
			rigidbody2D.velocity = Vector2.right * -speed;
		}
		if(right){
			rigidbody2D.velocity = Vector2.right * speed;
		}
		if(!(left || right)){
			rigidbody2D.velocity = Vector2.zero;
		}
	}
}
