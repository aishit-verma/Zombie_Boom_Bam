using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public const string PLAYER_TAG = "Player";
    public static PlayerMovement Instance { get; private set; }

    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;

    public Vector2 MoveInput { get; private set; }
    public bool IsMovementEnabled { get; set; } = true;
    private Vector2 recoilVelocity = Vector2.zero;

    [SerializeField] private float recoilRecoverySpeed = 10f;

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        Vector3 mouseWorldPos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction =
            (mouseWorldPos - transform.position).normalized;
        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void ApplyRecoil(Vector2 recoil)
    {
        recoilVelocity = recoil;
    }

    void FixedUpdate()
    {
        if (!IsMovementEnabled) return;

        // recover from recoil
        recoilVelocity = Vector2.Lerp(
            recoilVelocity, Vector2.zero,
            recoilRecoverySpeed * Time.fixedDeltaTime);

        rb.linearVelocity = MoveInput * moveSpeed + recoilVelocity;
    }
}