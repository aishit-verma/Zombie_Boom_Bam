using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.15f;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private Vector2 dashDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
                StopDash();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
            rb.linearVelocity = dashDirection * dashSpeed;
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;

        dashDirection = PlayerMovement.Instance.MoveInput;
        if (dashDirection == Vector2.zero)
            dashDirection = transform.up;

        PlayerMovement.Instance.IsMovementEnabled = false;
    }

    private void StopDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector2.zero;
        PlayerMovement.Instance.IsMovementEnabled = true;
    }
}