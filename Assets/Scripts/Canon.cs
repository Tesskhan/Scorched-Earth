using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform pivot;          // Pivot point for rotation
    public Transform muzzle;         // The cannon's muzzle where bullets spawn
    public GameObject bulletPrefab;  // Prefab of the bullet to shoot
    public float rotationSpeed = 100f; // Speed of rotation
    public float bulletSpeed = 10f;  // Default speed of the bullet
    public float maxBulletSpeed = 20f; // Max speed when spacebar is held down for a longer time
    private float chargeTime = 0f;   // Time the spacebar has been held down
    private GameManager gameManager; // Reference to the GameManager
    private bool currentTurn = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gameManager == null || !gameManager.IsPlayerTurn(gameObject))
        {
            return;
        }

        float input = 0f;

        // Get player input based on the tag
        if (gameObject.CompareTag("Player1"))
        {
            if (Input.GetKey(KeyCode.A))
            {
                input = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                input = 1f;
            }
        }
        else if (gameObject.CompareTag("Player2"))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                input = -1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                input = 1f;
            }
        }

        // Rotate the cannon if there's input
        if (Mathf.Abs(input) > 0.01f)
        {
            RotateCannon(input);
        }

        // Track spacebar hold time
        if (Input.GetKey(KeyCode.Space))
        {
            // Increase charge time while the spacebar is held
            chargeTime += Time.deltaTime;
        }

        // Shoot bullet when spacebar is released (so we wait for charging)
        if (Input.GetKeyUp(KeyCode.Space) && gameManager.currentPlayerTurn == gameObject.tag)
        {
            currentTurn = !currentTurn;
            Shoot();
            gameManager.IsPlayerTurn(currentTurn);

        }
    }

    void RotateCannon(float direction)
    {
        // Rotate the cannon around the pivot
        transform.RotateAround(pivot.position, Vector3.forward, -direction * rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if (bulletPrefab == null || muzzle == null)
        {
            Debug.LogWarning("BulletPrefab or Muzzle not assigned!");
            return;
        }

        // Adjust bullet speed based on charge time (longer press = faster bullet)
        float currentBulletSpeed = Mathf.Lerp(bulletSpeed, maxBulletSpeed, chargeTime);

        // Instantiate the bullet at the muzzle's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, transform.rotation);

        // Set the bullet's velocity to the cannon's forward direction, scaled by the charged speed
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * currentBulletSpeed; // Cannon's forward direction
            rb.bodyType = RigidbodyType2D.Dynamic; // Enable gravity and physics
        }

        // Ensure the bullet retains its original size
        bullet.transform.localScale = bulletPrefab.transform.localScale;

        // Reset charge time after shooting
        chargeTime = 0f;
    }
}
