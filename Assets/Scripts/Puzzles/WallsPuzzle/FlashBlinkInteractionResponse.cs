using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBlinkInteractionResponse : MonoBehaviour
{
    public Flash flash;
    public InteractionMediator mediator;
    public MeshCollider collider;
    public WallsPuzzleController puzzleController;
    public int index = 0;

    public void Start()
    {
        mediator.OnEyesStatusChanged += OnBlink;
    }

    public void OnDestroy()
    {
        mediator.OnEyesStatusChanged -= OnBlink;
    }

    public void OnBlink(bool blink)
    {
        if (blink)
        {
            AttemptFlash();
        }
    }

    private void AttemptFlash()
    {
        if (!puzzleController.CanActivate(index))
        {
            return;
        }

        Bounds bounds = collider.bounds;
        Plane[] cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
        {
            flash.DoFlash();
            puzzleController.IncrementIndex(index);
        }
    }
}
