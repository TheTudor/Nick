using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject ball;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
	
		offset = transform.position - ball.transform.position;

	}
	
	// Update is called once per frame
	void LateUpdate () {

		transform.position = ball.transform.position + offset;
		if (Input.GetKey (KeyCode.Q)) {
			transform.Rotate(Vector3.up,Time.deltaTime * -100,Space.World);	
		}
		if (Input.GetKey (KeyCode.E)) {
		
			transform.Rotate (Vector3.up, Time.deltaTime * 100, Space.World);
		}
	}
}
