using UnityEngine;
using System.Collections;

public class CenterScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float screenHeight = 2f * camera.orthographicSize;
		float screenWidth = screenHeight * camera.aspect;

		transform.position = new Vector3(screenWidth / 2f, screenHeight / 2f, transform.position.z);
	}
}
