using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform pivot;          // Pivot point for rotation
    public Transform muzzle;         // The cannon's muzzle where bullets spawn
    public GameObject bulletPrefab;  // Prefab of the bullet to shoot
    public float rotationSpeed = 100f; // Speed of rotation
    public float bulletSpeed = 10f;  // Speed of the bullet

    void Update()
    {
        // Get player input
        float input = Input.GetAxis("Horizontal"); // -1 for left, +1 for right

        // Rotate the cannon if there's input
        if (Mathf.Abs(input) > 0.01f)
        {
            RotateCannon(input);
        }

        // Shoot bullet on space key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
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

        // Instantiate bullet at the muzzle position and rotation
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, transform.rotation);

        // Apply force to the bullet in the cannon's forward direction
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * bulletSpeed; // `transform.up` points in the cannon's local forward direction
        }
    }
}
