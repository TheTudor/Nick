//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using Tobii.EyeX.Client;
using UnityEngine;

/// <summary>
/// Base class for EyeX interactors using game object bounds and game object ID
/// </summary>
public abstract class EyeXInteractionBehaviour : MonoBehaviour
{
    // Use attribute to be able to use custom property drawer
    [BitMask(typeof(EyeXBehaviors))]
    public EyeXBehaviors eyeXBehaviors;

    public ProjectedRect location;
    public bool showProjectedBounds = false;

    private EyeXHost _eyeXHost;
    protected string interactorId;

    /// <summary>
    /// Locate the EyeX host game object and its EyeXHost component on Awake.
    /// </summary>
    public void Awake()
    {
        _eyeXHost = EyeXHost.GetInstance();

        interactorId = gameObject.GetInstanceID().ToString();
        location = ProjectedRect.GetProjectedRect(gameObject, Camera.main);
    }

    protected void Update()
    {
        // Update the interactor bounds.
        var interactor = _eyeXHost.GetInteractor(interactorId);
        interactor.Location = ProjectedRect.GetProjectedRect(gameObject, Camera.main);
    }

    /// <summary>
    /// Register the interactor when the game object is enabled.
    /// </summary>
    public void OnEnable()
    {
        var interactor = new EyeXInteractor(interactorId, EyeXHost.NoParent, eyeXBehaviors, OnEyeXEvent);
        interactor.Location = ProjectedRect.GetProjectedRect(gameObject, Camera.main);
        _eyeXHost.RegisterInteractor(interactor);
    }

    /// <summary>
    /// Unregister the interactor when the game object is disabled.
    /// </summary>
    public void OnDisable()
    {
        _eyeXHost.UnregisterInteractor(interactorId);
    }

    /// <summary>
    /// Draw the projected bounds if 'showProjectedBounds' is enabled.
    /// NOTE: Could be replaced by Gizmos
    /// </summary>
    public void OnGUI()
    {
        if (showProjectedBounds)
        {
            var face = ProjectedRect.GetProjectedRect(gameObject, Camera.main);
            if (face.isValid)
            {
                GUI.Box(face.rect, interactorId);
            }
        }
    }

    /// <summary>
    /// Override this event handler to handle the events
    /// </summary>
    /// <param name="interactorId">The id of the interactor</param>
    /// <param name="event">The interaction event</param>
    protected abstract void OnEyeXEvent(string interactorId, InteractionEvent @event);
}
