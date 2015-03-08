using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour {
	private float height, width;
	// Use this for initialization
	void Start () {
		height = Camera.main.orthographicSize * 2.0f;
		width = height * Screen.width / Screen.height;
		transform.position = new Vector3(width/2, height/2, 1.0f);

		Sprite spr = gameObject.GetComponent<SpriteRenderer>().sprite;
		width /= spr.bounds.size.x;
		height /= spr.bounds.size.y;
		transform.localScale = new Vector3(width, height, 1f);
	}
}
