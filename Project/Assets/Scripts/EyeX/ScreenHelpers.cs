//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------
using System;
using UnityEngine;

/// <summary>
/// Provides utility functions related to screen and window handling.
/// </summary>
public class ScreenHelpers
{
    private static ScreenHelpers _instance;
    private string _windowId;
    private IntPtr _hwnd;

    /// <summary>
    /// Singleton instance.
    /// </summary>
    public static ScreenHelpers Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScreenHelpers();
            }

            return _instance;
        }
    }

    private ScreenHelpers()
    {
        _hwnd = Win32Helpers.GetForegroundWindow();
        _windowId = _hwnd.ToString();
    }

    /// <summary>
    /// Window ID for the game window.
    /// </summary>
    public string GameWindowId
    {
        get
        {
            return _windowId;
        }
    }

    /// <summary>
    /// Returns the position of the game window in screen coordinates.
    /// </summary>
    public Vector2 GetGameWindowPosition()
    {
#if UNITY_EDITOR
        var gameWindow = GetMainGameView();
        var heightOffset = gameWindow.position.height - Screen.height;
        return new Vector2(gameWindow.position.x, gameWindow.position.y + heightOffset);
#else
        var windowClientPosition = new Win32Helpers.POINT();
        Win32Helpers.ClientToScreen(_hwnd, ref windowClientPosition);
        return new Vector2(windowClientPosition.x, windowClientPosition.y);
#endif
    }

#if UNITY_EDITOR
    private static UnityEditor.EditorWindow GetMainGameView()
    {
        var unityEditorType = Type.GetType("UnityEditor.GameView,UnityEditor");
        var getMainGameViewMethod = unityEditorType.GetMethod("GetMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = getMainGameViewMethod.Invoke(null, null);
        return (UnityEditor.EditorWindow)result;
    }
#endif
}