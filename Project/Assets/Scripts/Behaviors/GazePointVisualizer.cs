//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;

/// <summary>
/// Visualizes the gaze point in the game window using a tiny GUI.Box.
/// </summary>
public class GazePointVisualizer : MonoBehaviour
{
    private EyeXGazePointProvider _gazePointProvider;
    
    /// <summary>
    /// The choice of gaze point data stream to be visualized.
    /// Changing this value will not take effect until the next OnEnable().
    /// </summary>
    public EyeXGazePointType gazePointType = EyeXGazePointType.GazeLightlyFiltered;

    /// <summary>
    /// Indicates whether to show the visualizer point or not
    /// </summary>
    public bool showPoint = true;

    /// <summary>
    /// The size of the visualizer point
    /// </summary>
    public float pointSize = 5;
    
    /// <summary>
    /// The color of the visualizer point
    /// </summary>
    public Color pointColor = Color.red;
    
    public void Awake()
    {
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
    
    /// <summary>
    /// Draw a GUI.Box at the user's gaze point.
    /// </summary>
    public void OnGUI()
    {
        if (!showPoint) return;

        UpdateGUIStyle();

        var title = "";

        // Show fixation index for fixation types
        if (gazePointType == EyeXGazePointType.FixationSensitive || gazePointType == EyeXGazePointType.FixationSlow)
        {
            var fixationIndex = _gazePointProvider.GetLastFixationCount(gazePointType);
            title = fixationIndex.ToString();
        }

        var gazePoint = _gazePointProvider.GetLastGazePoint(gazePointType);
        if (gazePoint.IsWithinScreenBounds)
        {
            GUI.Box(new UnityEngine.Rect(gazePoint.GUI.x - pointSize/2.0f, gazePoint.GUI.y - pointSize/2.0f, pointSize, pointSize), title);
        }
    }
    
    private void UpdateGUIStyle()
    {
        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, pointColor);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.skin.box.border = new RectOffset(0, 0, 0, 0);
    }
}