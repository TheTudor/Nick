using UnityEngine;
using System.Collections;

public class SphereControl: MonoBehaviour 
{
	public float speed;
	public string cameraId = "Camera";
	
	private GameObject c;
	//private float _scaleFactor = 0;
	// Use this for initialization
	void Start () {
		c = GameObject.Find(cameraId);
	}
	

	void FixedUpdate(){
		
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical =   Input.GetAxis ("Vertical");
		
		Vector3 movement =  new Vector3 (moveHorizontal, 0.0f, moveVertical);
		//Vector3 m = c.transform.TransformDirection (c.transform.forward) ;//
		movement = c.transform.rotation * movement ;//new Vector3 (movement.x, m.y, movement.z);
		//Debug.Log (movement.ToString ());
		//Vector3 m = Vector3.Normalize(c.transform.forward);

		//Vector3 movement = new Vector3 (c.transform.position.x*moveHorizontal, 0.0f, c.transform.position.z*moveVertical);
		
		rigidbody.AddForce (movement * speed * Time.deltaTime);
	}
}