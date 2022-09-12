using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public PlayerInputHandler InputHandler { get; private set; }

    public event EventHandler OnSpacePressed;

    private bool JumpInput;


    // Start is called before the first frame update
    void Start()
    {
        InputHandler = GetComponent<PlayerInputHandler>();

    }

   

    private void Update()
    {

        JumpInput = InputHandler.JumpInput;

        if(JumpInput)
        {
            //Space pressed!
            OnSpacePressed?.Invoke(this, EventArgs.Empty);
            
        }
    }

}
