using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float RotateSpeed = 75f;
    public float JumpForce = 7f;

    public Transform GroundCheck;
    public float GroundCheckRadius = 0.25f;
    public LayerMask GroundLayer;

    public GameObject Bullet;
    public Transform BulletSpawnPoint;

    public float BulletSpeed = 100f;

    private Rigidbody _rb;

    private float _vInput;
    private float _hInput;

    private bool _jumpPressed;
    private bool _shootPressed;

    private bool _isGrounded;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.freezeRotation = true;
    }

    void Update()
    {
       _vInput = Input.GetAxis("Vertical");
        _hInput = Input.GetAxis("Horizontal");

       _isGrounded = Physics.CheckSphere(
            GroundCheck.position,
            GroundCheckRadius,
            GroundLayer
        );

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _jumpPressed = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _shootPressed = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDirection =
            transform.forward *
            _vInput *
            MoveSpeed *
            Time.fixedDeltaTime;

        _rb.MovePosition(
            _rb.position + moveDirection
        );

        Quaternion turnRotation = Quaternion.Euler(
            0f,
            _hInput * RotateSpeed * Time.fixedDeltaTime,
            0f
        );

        _rb.MoveRotation(
            _rb.rotation * turnRotation
        );

        if (_jumpPressed)
        {
            _rb.linearVelocity = new Vector3(
                _rb.linearVelocity.x,
                0f,
                _rb.linearVelocity.z
            );

             _rb.AddForce(
                Vector3.up * JumpForce,
                ForceMode.Impulse
            );

            _jumpPressed = false;
        }

        if (_shootPressed)
        {
            GameObject newBullet = Instantiate(
                Bullet,
                BulletSpawnPoint.position,
                BulletSpawnPoint.rotation
            );

            Rigidbody bulletRB =
                newBullet.GetComponent<Rigidbody>();

            if (bulletRB != null)
            {
                bulletRB.useGravity = false;

                bulletRB.collisionDetectionMode =
                    CollisionDetectionMode.ContinuousDynamic;

                bulletRB.interpolation =
                    RigidbodyInterpolation.Interpolate;

                bulletRB.linearVelocity =
                    BulletSpawnPoint.forward *
                    BulletSpeed;
            }

            Collider playerCollider =
                GetComponent<Collider>();

            Collider bulletCollider =
                newBullet.GetComponent<Collider>();

            if (playerCollider != null &&
                bulletCollider != null)
            {
                Physics.IgnoreCollision(
                    playerCollider,
                    bulletCollider
                );
            }

            _shootPressed = false;
        }
    }

     void OnDrawGizmosSelected()
    {
        if (GroundCheck == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            GroundCheck.position,
            GroundCheckRadius
        );
    }
}