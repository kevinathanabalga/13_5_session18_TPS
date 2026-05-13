using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // Movement vars
    public float MoveSpeed = 10f;
    public float RotateSpeed = 75f;
    public float JumpVelocity = 5f;

    // Ground check vars
    public bool IsOnGround = true;
    public float GroundCheckRadius = 0.3f;
    public LayerMask GroundLayer;

    // Shooting vars
    public GameObject Bullet;
    public float BulletSpeed = 100f;

    // Private vars
    private float _vInput;
    private float _hInput;

    private bool _isJumping;
    private bool _isShooting;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movement input
        _vInput = Input.GetAxis("Vertical") * MoveSpeed;
        _hInput = Input.GetAxis("Horizontal") * RotateSpeed;

        // Ground check
        Vector3 groundCheckPos = transform.position + Vector3.down * 1f;
        IsOnGround = Physics.CheckSphere(
            groundCheckPos,
            GroundCheckRadius,
            GroundLayer
        );

        // Jump input
        if (Input.GetKeyDown(KeyCode.J) && IsOnGround)
        {
            _isJumping = true;
        }

        // Shooting input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isShooting = true;
        }
    }

    void FixedUpdate()
    {
        // Move forward/backward
        _rb.MovePosition(
            transform.position +
            transform.forward * _vInput * Time.fixedDeltaTime
        );

        // Rotate left/right
        Quaternion angleRot = Quaternion.Euler(
            Vector3.up * _hInput * Time.fixedDeltaTime
        );

        _rb.MoveRotation(_rb.rotation * angleRot);

        // Jump
        if (_isJumping)
        {
            _rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
            _isJumping = false;
        }

        // Shoot
        if (_isShooting)
        {
            Vector3 spawnPos = transform.position + transform.forward * 1f;

            GameObject newBullet = Instantiate(
                Bullet,
                spawnPos,
                transform.rotation
            );

            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();

            bulletRB.linearVelocity = transform.forward * BulletSpeed;

            _isShooting = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 groundCheckPos = transform.position + Vector3.down * 1f;

        Gizmos.DrawWireSphere(
            groundCheckPos,
            GroundCheckRadius
        );
    }
}