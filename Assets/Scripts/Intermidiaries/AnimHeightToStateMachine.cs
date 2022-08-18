using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHeightToStateMachine : MonoBehaviour
{
    private SquashStretchController Controller;


    private void Start()
    {
        Controller = GetComponentInParent<SquashStretchController>();
    }

    public void StopJumpSquashStretch()
    {
        Controller.StopJumpSquashStretch();
    }

    public void StopLandingSquashStretch()
    {
        Controller.StopLandSquashStretch();
    }
}
