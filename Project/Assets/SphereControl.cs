using UnityEngine;
using System.Collections;
using Leap;
using System;

public class SampleListener : Leap.Listener
{
	public Leap.Controller controller;
	
	public string TextDisplay { get; set; }
	
	//hand states 
	bool grounded_left;
	bool grounded_right;
	
	const int GROUND = 100;
	
	private bool toPause = false;
	public bool ToPause {
		get { return toPause; }
		set { toPause = value; }
	}
	
	private float movement;
	public float Distance{
		get { return movement; }
		set { movement = value; }
	}
	
	public float theta = 0;
	
	//checks if user is always using two hands only
	private bool checkTwoHands(Frame frame)
	{
		if (frame.Hands.Count != 2) {
			TextDisplay = "PUT HANDS BACK";
			Debug.Log("PUT HANDS BACK");
			toPause = true;
			// PRINT MESSAGE TUDOR'S RESEARCH JOB
			return false;
		} else {
			toPause = false; 
		}
		return true;
	}
	
	//checks if any hand is on the ground
	private int checkGrounding(Frame frame)
	{
		if (checkTwoHands(frame))
		{
			Hand left = frame.Hands.Leftmost;
			Hand right = frame.Hands.Rightmost;
			if (left.PalmPosition.y < GROUND) {
				grounded_left = true;
				TextDisplay = "left hand grounded moffuga";
				Debug.Log("left hand grounded moffuga");
			}
			else {
				grounded_left = false;
			}
			if (right.PalmPosition.y < GROUND) {
				grounded_right = true;		
				TextDisplay = "right hand grounded fommuga";
				Debug.Log("right hand grounded fommuga");
			}
			else {
				grounded_right = false;
			}
		}
		
		if (!grounded_left && !grounded_right) {
			return 0;
		}
		if (grounded_left ^ grounded_right) {
			return 1;
		}
		if (grounded_left && grounded_right) {
			return 2;
		}
		return -1;
	}
	
	//move forth and back
	public void move(Frame frame, Frame previousFrame) {
		Debug.Log ("LOOK AT ME I IZ MOVING FUCKEN EY");
		TextDisplay = "LOOK AT ME I IZ MOVING FUCKEN EY";
		Hand groundedHand = new Hand ();
		Hand previousHand;
		
		if (previousFrame != null) {
			if (grounded_left) {
				groundedHand = frame.Hands.Leftmost;
				previousHand = previousFrame.Hands.Leftmost;
			} else {
				groundedHand = frame.Hands.Rightmost;
				previousHand = previousFrame.Hands.Rightmost;
			}
			movement = (groundedHand.PalmPosition.z - 
			            previousHand.PalmPosition.z)/10;
			
			TextDisplay = "Movement is" + movement;
			Debug.Log ("Movement is" + movement);
		}
	}
	
	//rotate
	private Vector2 previousLtr;
	
	public int Dir { get; set; }
	
	public void turn(Frame frame, Frame previousFrame) {
		Debug.Log ("LOOK AT ME I IZ TERRNING FUCKEN EY");
		TextDisplay = "LOOK AT ME I IZ TERRNING FUCKEN EY";
		
		GestureList gestures = frame.Gestures (previousFrame);
		if(!gestures.IsEmpty) {
			Gesture gesture = gestures[0];
			if(gesture.Type == Gesture.GestureType.TYPESWIPE) {
				SwipeGesture swipe = new SwipeGesture (gesture);
				
				Dir = Math.Sign(swipe.Direction.x);
			}
		}
	}
	
	// Use this for initialization
	public void Start()
	{
		controller = new Leap.Controller(this);
		TextDisplay = "HELLO I AM AN AMAZING PROGRAM";
		Debug.Log ("HELLO I AM AN AMAZING PROGRAM");
	}
	
	public override void OnConnect (Controller controller)
	{
		controller.EnableGesture (Gesture.GestureType.TYPESWIPE);
	}

	public bool StopMoving { get; set; }

	// called once per frame
	public override void OnFrame(Controller controller)
	{
		Frame frame = controller.Frame ();
		Frame previousFrame = controller.Frame (1);
		
		if (checkTwoHands (frame)) {
			switch(checkGrounding (frame)) {
			case 2: { StopMoving = true; break; }
			case 1: { move(frame, previousFrame); break; }
			case 0: { turn(frame, previousFrame); break; }
			}
		}
	}
}

public class SphereControl: MonoBehaviour 
{

	public float speed;
	public string cameraId = "Camera";
	
	private GameObject c;

	SampleListener listener;
	Controller controller;

	//private float _scaleFactor = 0;
	// Use this for initialization
	void Start () {
		c = GameObject.Find(cameraId);
	
		listener = new SampleListener();
		controller = new Controller();
		controller.AddListener(listener);
	}
	

	/*void FixedUpdate(){
		
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical =   Input.GetAxis ("Vertical");
		
		Vector3 movement =  new Vector3 (moveHorizontal, 0.0f, moveVertical);
		//Vector3 m = c.transform.TransformDirection (c.transform.forward) ;//
		movement = c.transform.rotation * movement ;//new Vector3 (movement.x, m.y, movement.z);
		//Debug.Log (movement.ToString ());
		//Vector3 m = Vector3.Normalize(c.transform.forward);

		//Vector3 movement = new Vector3 (c.transform.position.x*moveHorizontal, 0.0f, c.transform.position.z*moveVertical);
		
		rigidbody.AddForce (movement * speed * Time.deltaTime);
	}*/

	void Update() {
		/*DummyLeapController targetScript = this.gameObject.GetComponent<DummyLeapController>();
		float distance = targetScript.distance;
		float theta = targetScript.theta;*/

		Vector3 a = gameObject.transform.position;
		if (listener.Distance > 0 && !listener.StopMoving) {
			a.z += listener.Distance;
			gameObject.transform.position = a;
		}
		
		//if(listener.Dir != null)
		//	c.transform.Rotate (Vector3.up, listener.Dir * 0.5f);



		//Debug.Log (listener.Distance);
		if (listener.Distance > 0 && !listener.StopMoving)// && c.transform.rotation.y > 0.9f) 
			transform.Translate ((c.transform.rotation * Vector3.forward)* listener.Distance / 100);

		if (Input.GetKey(KeyCode.W)) 
		{
			transform.Translate((new Vector3 (0.0f, 0.0f, 1.0f))*Time.deltaTime*speed,Space.World);
		}
		
		if (Input.GetKey(KeyCode.S)) 
		{
			transform.Translate((new Vector3 (0.0f, 0.0f, -1.0f))*Time.deltaTime*speed,Space.World);
		}
		
		if (Input.GetKey(KeyCode.A)) 
		{
				transform.Translate((new Vector3 (-1.0f, 0.0f, 0.0f))*Time.deltaTime*speed,Space.World);
		}
		
		if (Input.GetKey(KeyCode.D)) 
		{
					transform.Translate((new Vector3 (1.0f, 0.0f, 0.0f))*Time.deltaTime*speed,Space.World);
		}

	}
}