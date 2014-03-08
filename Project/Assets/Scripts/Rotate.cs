using UnityEngine;
using System.Collections;
using System;

public class Rotate : MonoBehaviour {

    private double speedX = 0;
    private double speedY = 0;
    public double speed = 100;
    public double speedTreeshold = 0.13;

    public double xViewRestriction = 30;
    public double yViewRestriction = 30;

    private EyeXGazePointProvider _gazePointProvider;

    private float centerX = 0f;
    private float centerY = 0f;
//    private int counter = 0;
    /// <summary>
    /// The choice of gaze point data stream to be visualized.
    /// Changing this value will not take effect until the next OnEnable().
    /// </summary>
    public EyeXGazePointType gazePointType = EyeXGazePointType.GazeLightlyFiltered;
    public Vector3 initRot;

   
    public void Awake()
    {
        centerX = Screen.width / 2;
        centerY = Screen.height / 2;
        _gazePointProvider = EyeXGazePointProvider.GetInstance();
    }
    
    public void OnEnable()
    {
        print("GazePointVisualizer enabled.");
        _gazePointProvider.StartStreaming(gazePointType);
    }
    
    public void OnDisable()
    {
        print("GazePointVisualizer disabled.");
        _gazePointProvider.StopStreaming(gazePointType);
    }

	// Use this for initialization
	void Start () 
    {
        initRot = transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        var gazePoint = _gazePointProvider.GetLastGazePoint(gazePointType);
        float gazeX = gazePoint.GUI.x;
        float gazeY = gazePoint.GUI.y;



        if (gazePoint.IsWithinScreenBounds)
        {
            //Debug.Log("x: " + gazePoint.GUI.x + " y: " + gazePoint.GUI.y);
            //Debug.Log("delta" + (prevX-gazeX).ToString());
            double deltaX = ((gazeX - centerX) * 6) / (centerX);
            double deltaY = ((gazeY - centerY) * 6) / (centerY);
            //speedX = ((1/(1+ Math.Pow( Math.E,delta*(-1)))) - 0.5)*100;

           
            speedX = convertToSpeed(deltaX) * speed;
            speedY = convertToSpeed(deltaY) * speed * (-1);
        //    Debug.Log("speed: " + (speedX).ToString());


            //  transform.Rotate(Vector3.up, Time.deltaTime * (float)speedX);
            //   GUI.Box(new UnityEngine.Rect(gazePoint.GUI.x - pointSize/2.0f, gazePoint.GUI.y - pointSize/2.0f, pointSize, pointSize), title);

            //  prevX = gazeX;
            //  prevY = gazeY;
        } 
        else
        {
            speedX = 0;
            speedY = 0;
        }
        var angle = normalizeAngle(transform.eulerAngles.y - initRot.y);
        if (angle < xViewRestriction && speedX > 0 || angle > xViewRestriction*(-1) && speedX < 0)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * (float)speedX);
        }
        angle = normalizeAngle(transform.eulerAngles.x - initRot.x);
        if (angle > yViewRestriction*(-1) && speedY > 0 || angle < yViewRestriction && speedY < 0)
        {
            Debug.Log("delta: " + (angle).ToString());

            transform.Rotate(Vector3.left, Time.deltaTime * (float)speedY);
        }


        //counter++;

	}
    private double normalizeAngle(double angle)
    {
        if(angle > 180) angle -= 360;
        if(angle < -180) angle += 360;
        return angle;
    }
    private double convertToSpeed(double delta)
    {
        if (delta >= 0)
        {
            return normalizeSpeed((1 / (1 + Math.Pow(Math.E,
                                                       (delta - 6) * (-1)
                                                       ))));
        } else
        {
            return normalizeSpeed((1 / (1 + Math.Pow(Math.E,
                                                       (delta + 6)
                                                       ))))*(-1);
        }
    }
    private double normalizeSpeed(double d)
    {
        if (Math.Abs(d) > speedTreeshold)
        {
            return d;
        } else
            return 0.0;

    }

}
