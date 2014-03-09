using UnityEngine;

using Tobii.EyeX.Client;
using Tobii.EyeX.Client.Interop;
using Tobii.EyeX.Framework;
using System.Collections;

public class ForceOnGoaze : EyeXInteractionBehaviour {


    private bool _hasFocus;
    public float shakeFactor = 10f;
    public int shakeLength = 10;
    private int shakeStep = 0;
    public string cameraId = "Camera";

    public float destroyDistance = 10;

    public int forcePower = 0;

    private GameObject c;
    //private float _scaleFactor = 0;
	// Use this for initialization
	void Start () {
        c = GameObject.Find(cameraId);
	}
	
	// Update is called once per frame
	public new void Update () 
    {
        base.Update();
        
        // Update the scale factor depending on whether the eye gaze is on the object or not.
      //  Debug.Log(_hasFocus.ToString());
		Debug.Log(_hasFocus.ToString());

		if (_hasFocus)
        {
            if(shakeStep < shakeLength/2)
            {
                transform.Translate(Vector3.back * Time.deltaTime*shakeFactor);
            }
            else
            {
                transform.Translate(Vector3.forward * Time.deltaTime*shakeFactor);
            }

            if(shakeStep > 10)
            {
                shakeStep = -1;
            }

            if(forcePower > 60)
            {
                var heading = c.transform.position - transform.position;
                transform.Translate(heading * Time.deltaTime*(forcePower/60)*2);

                if(heading.magnitude < destroyDistance)
                {
                    Destroy(this.gameObject);
                }
            }
            shakeStep++;
            forcePower++;
        }
        else
        {
            forcePower = 0;
        //    _scaleFactor = Mathf.Clamp01(_scaleFactor - speed * Time.deltaTime);
        }	
	}

    protected override void OnEyeXEvent(string interactorId, InteractionEvent @event)
    {
        // NOTE: this method is called from a worker thread, so it must not access any game objects.
        // Therefore, we store the state in a variable and handle the state change in the Update() method.
		Debug.Log("gaze");
		foreach (var behavior in @event.Behaviors)
        {
            GazeAwareEventParams eventData;
            if (behavior.TryGetGazeAwareEventParams(out eventData))
            {
                _hasFocus = eventData.HasGaze != EyeXBoolean.False;
            }
        }
    }
}
