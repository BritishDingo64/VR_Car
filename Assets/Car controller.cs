using UnityEngine;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.Content.Interaction;

public class CarController : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public InputActionReference trigger;
    public XRKnob knob;
    public bool isPressed = false;
    public float motorTorque;
    public float breakTorque;
    public float steeringMax;

    void Start()
    {
        // Initialization if needed
    }

    private void Update()
    {
        if (trigger.action.WasPressedThisFrame())
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = 0;
                wheels[i].motorTorque = motorTorque;
            }
        }

        if (trigger.action.WasReleasedThisFrame())
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = breakTorque;
            }
        }

        for (int i = 0; i < wheels.Length - 2; i++)
        {
            wheels[i].steerAngle = (knob.value - 0.5f) * 120;
        }
    }
}
