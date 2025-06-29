using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float maxVelocity = 3;
    public float rotationSpeed = 0.5f;
    float rotationVelocity = 0;
    float thrustVelocity = 0;
    private float thrustAcceleration = 0.5f;
    private float thrustDamping = 2f;
    private float rotationAcceleration = 0.5f;
    private float rotationDamping = 2f; // Higher = more damping
    public float maxRotationSpeed = 3;

    #region PlayerMovement API

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");
        
        ThrustForward(yAxis);
        Rotate(transform, xAxis* rotationSpeed);
        ClampVelocity(); // Call this to limit velocity
        ApplyTurningInertia(xAxis);
        ApplyThrustInertia(yAxis);
        ClampVelocity();

    }

    #endregion

    #region Maneuvering API

    private void ClampVelocity()
    {
        float x = Mathf.Clamp(rb.linearVelocity.x, -maxVelocity, maxVelocity);
        float y = Mathf.Clamp(rb.linearVelocity.y, -maxVelocity, maxVelocity);

        rb.linearVelocity = new Vector2(x, y);
    }

    private void ThrustForward(float amount)
    {
        Vector2 force = transform.up * amount;
        rb.AddForce(force);
    }

    private void Rotate(Transform t, float amount)
    {
        t.Rotate(0, 0, amount);
    }

    private void ApplyTurningInertia(float input)
    {
        // Accelerate rotation velocity based on input
        rotationVelocity += input * rotationAcceleration * Time.deltaTime;

        //Cap the rotation
        rotationVelocity = Mathf.Clamp(rotationVelocity, -maxRotationSpeed, maxRotationSpeed);

        // Apply damping to simulate rotational resistance
        if (Mathf.Approximately(input, 0))
        {
            rotationVelocity = Mathf.Lerp(rotationVelocity, 0, Time.deltaTime * rotationDamping);
        }

        // Apply rotation
        transform.Rotate(0, 0, rotationVelocity);
    }

    private void ApplyThrustInertia(float input)
    {
        thrustVelocity += input * thrustAcceleration * Time.deltaTime;

        if (Mathf.Approximately(input, 0))
        {
            thrustVelocity = Mathf.Lerp(thrustVelocity, 0, Time.deltaTime * thrustDamping);
        }


    }

    #endregion
}