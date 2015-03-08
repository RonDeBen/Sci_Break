using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public float speed;
	private Vector2 ballVelocity, ballPos;
	private float radius, screenHeight, screenWidth;
	private CircleCollider2D c2d;
	public GameObject paddle, MMObj, brickMaker;
	private MusicMiddleware mm;
	private CreateBricks cb;
	private bool stasis = true;
	private int numberOfBricks;
	// Use this for initialization
	void Start () {
		//rigidbody2D.velocity = Vector2.up * -speed;
		c2d = gameObject.collider2D as CircleCollider2D;
		radius = c2d.radius;
		screenHeight = 2f * Camera.main.orthographicSize;
		screenWidth = screenHeight * Camera.main.aspect;
		transform.position = new Vector3(screenWidth / 2f, transform.position.y, transform.position.z);
		mm = MMObj.GetComponent<MusicMiddleware>();
		cb = brickMaker.GetComponent<CreateBricks>();
		numberOfBricks = cb.getBrickNumber();

		//mm.loopSound("Rorschach Game Jam Jam", true);
		//mm.loopSound("Very_Hedgie", true);
	}
	
	// Update is called once per frame
	void Update () {
		if(!stasis){
			ballVelocity = rigidbody2D.velocity;

			ballPos = transform.position;
			// ball hits the edges of the windows
	            if (ballPos.y + radius > screenHeight)//top of the screen
		            {
		                rigidbody2D.velocity = new Vector3(ballVelocity.x, -Mathf.Abs(ballVelocity.y));
		                mm.playSound("paddlehit");
		            }
		            else if (ballPos.x + radius > screenWidth)//right side of the screen
		            {
		                rigidbody2D.velocity = new Vector3(-Mathf.Abs(ballVelocity.x), ballVelocity.y);
		                mm.playSound("paddlehit");
		            }
		            else if (ballPos.x - radius < 0)//left side of the screen
		            {
		                rigidbody2D.velocity = new Vector3(Mathf.Abs(ballVelocity.x), ballVelocity.y);
		                mm.playSound("paddlehit");
		            }
		            else if(ballPos.y - radius < 0){//ball falls through bottom of screen
		            	mm.playSound("balldie");
		            	stasis = true;
	            }
	        }else{
	        	Vector3 paddlePos = paddle.transform.position;
	        	transform.position = new Vector3(paddlePos.x, paddlePos.y + 1.5f, paddlePos.z);
	        	if(Input.GetKeyDown("space")){
	        		stasis = false;
	        		rigidbody2D.velocity = Vector2.up * -speed;
	       	}
	    }
	}

	void OnCollisionExit2D(Collision2D collision){
		if(collision.gameObject.tag.Equals("Player")){
			float x = (transform.position.x - collision.transform.position.x);
			float y = ballVelocity.y;
			Vector2 newVelocity = new Vector2(x,y);
			newVelocity.Normalize();
			rigidbody2D.velocity = newVelocity * speed;
			mm.playSound("paddlehit");
		}else{
			Destroy(collision.gameObject);
			mm.playSound("brickhit");
			numberOfBricks--;
			if(numberOfBricks < 1){
				StartCoroutine(WinGame());
			}
		}
	}

	IEnumerator WinGame(){
		mm.pauseAllSounds();
		mm.playSound("win");
		yield return new WaitForSeconds(1);
		Application.LoadLevel("Sci_Break");
	}
}
