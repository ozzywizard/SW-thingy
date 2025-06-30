using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Velocity Settings")]
    public float maxVelocity = 3f;
    public float maxRotationSpeed = 3f;

    [Header("Thrust Inertia")]
    public float thrustAcceleration = 5f;
    public float thrustDamping = 5f;

    [Header("Rotation Inertia")]
    public float rotationAcceleration = 200f;
    public float rotationDamping = 2f;

    private float rotationVelocity = 0f;
    private float thrustVelocity = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");

        ApplyThrustInertia(yAxis);
        ApplyTurningInertia(xAxis);
        ClampVelocity();
    }

    private void ApplyThrustInertia(float input)
    {
        // Update thrust velocity
        thrustVelocity += input * thrustAcceleration * Time.deltaTime;

        // Dampen when input is released
        if (Mathf.Approximately(input, 0f))
        {
            thrustVelocity = Mathf.Lerp(thrustVelocity, 0f, Time.deltaTime * thrustDamping);
        }

        //clamp thrust
        thrustVelocity = Mathf.Clamp(thrustVelocity, -maxVelocity, maxVelocity);

        // Apply force in the ship's forward (up) direction
        Vector2 thrustDirection = transform.up * thrustVelocity;
        rb.linearVelocity = new Vector2(thrustDirection.x, thrustDirection.y);
    }

    private void ApplyTurningInertia(float input)
    {
        // Update rotational velocity
        rotationVelocity += input * rotationAcceleration * Time.deltaTime;
        rotationVelocity = Mathf.Clamp(rotationVelocity, -maxRotationSpeed, maxRotationSpeed);

        // Dampen when input is released
        if (Mathf.Approximately(input, 0f))
        {
            rotationVelocity = Mathf.Lerp(rotationVelocity, 0f, Time.deltaTime * rotationDamping);
        }

        // Apply rotation (frame rate independent)
        transform.Rotate(0f, 0f, rotationVelocity * Time.deltaTime);
    }

    private void ClampVelocity()
    {
        // Clamp the actual Rigidbody2D velocity
        float x = Mathf.Clamp(rb.linearVelocity.x, -maxVelocity, maxVelocity);
        float y = Mathf.Clamp(rb.linearVelocity.y, -maxVelocity, maxVelocity);
        rb.linearVelocity = new Vector2(x, y);
    }
}
