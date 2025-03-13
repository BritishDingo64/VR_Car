using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Content.Interaction;
using TMPro; // Add this for TextMeshPro

public class CarController : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public InputActionReference rtrigger; // Forward acceleration
    public InputActionReference ltrigger; // Brake & reverse
    public XRKnob knob; // Steering input

    public float motorTorque; // Forward torque
    public float brakeTorque; // Manual brake force
    public float reverseTorque; // Reverse torque
    public float autoBrakeTorque = 50f; // Auto-brake when rtrigger is released
    public float downforceMultiplier = 1000f;

    public TextMeshPro speedText; // Reference to the TextMeshPro component in the 3D world

    private Rigidbody rb;
    private bool isBraking = false;
    private bool isReversing = false;
    private float reverseHoldTime = 0f;
    private float reverseThreshold = 1f; // 1-second delay before reversing

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float steerAngle = (knob.value - 0.5f) * 120;
        for (int i = 0; i < wheels.Length - 2; i++) // Only front wheels steer
            wheels[i].steerAngle = steerAngle;

        if (rtrigger.action.IsPressed())
        {
            ApplyTorque(motorTorque);
            ResetBrakingAndReverse();
        }
        else if (ltrigger.action.IsPressed()) // Manual brake
        {
            ApplyBrakeOrReverse();
        }
        else // Auto-brake when rtrigger is released
        {
            ApplyAutoBrake();
        }

        // Update the speed text with the car's current speed in MPH
        UpdateSpeedText();
    }

    private void FixedUpdate()
    {
        ApplyDownforce();
    }

    private void ApplyTorque(float torque)
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].brakeTorque = 0;
            wheels[i].motorTorque = torque;
        }
    }

    private void ApplyBrakeOrReverse()
    {
        if (!isReversing)
        {
            ApplyBrake();
        }

        if (rb.linearVelocity.magnitude < 0.1f) // Check if stopped
        {
            ReleaseBrake(); // Release the brake once stopped
            reverseHoldTime += Time.deltaTime;

            if (reverseHoldTime >= reverseThreshold)
            {
                isReversing = true;
                ApplyTorque(-reverseTorque);
            }
        }
        else
        {
            reverseHoldTime = 0f; // Reset counter if moving
        }
    }

    private void ApplyBrake()
    {
        isBraking = true;
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].brakeTorque = brakeTorque;
            wheels[i].motorTorque = 0;
        }
    }

    private void ApplyAutoBrake()
    {
        if (!isBraking && !isReversing) // Auto-brake only if not manually braking or reversing
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = autoBrakeTorque;
                wheels[i].motorTorque = 0;
            }
        }
    }

    private void ReleaseBrake()
    {
        isBraking = false;
        for (int i = 0; i < wheels.Length; i++)
            wheels[i].brakeTorque = 0;
    }

    private void ResetBrakingAndReverse()
    {
        isBraking = false;
        isReversing = false;
        reverseHoldTime = 0f;
    }

    private void ApplyDownforce()
    {
        float downforce = rb.linearVelocity.magnitude * downforceMultiplier;
        rb.AddForce(-transform.up * downforce);
    }

    private void UpdateSpeedText()
    {
        // Convert the velocity magnitude to MPH
        float speedInMPH = rb.linearVelocity.magnitude * 2.23694f; // 1 m/s = 2.23694 MPH
        speedText.text = $"Speed: {speedInMPH:F1} MPH"; // Update the text with the speed value
    }
}
