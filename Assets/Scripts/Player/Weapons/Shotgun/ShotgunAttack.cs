using System.Collections;
using UnityEngine;

public class ShotgunAttack : MonoBehaviour
{
    public Animator animator;
    private GameObject attackArea = default;
    public bool attacking = false;
    public float attackDuration = 0.25f;
    public float attackCooldown = 1f;
    private float attackTimer = 0f;
    private float cooldownTimer = 0f;
    public bool shotgunAttacking;
    [SerializeField]
    private float recoilXMultiplier = 0f;
    [SerializeField]
    private float recoilYMultiplier = 0f;
    private int bullets = 2;
    [SerializeField]
    private int maxBullets = 2;
    [SerializeField]
    private float reloadTime = 1f;
    [SerializeField]
    private float recoilForce = 0f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private bool reloading = false;
    private Rigidbody2D rb;

    // Reference to PlayerMovement (make sure this is set in Inspector or GetComponent)
    public PlayerMovement playerMovement;
    public BulletUI bulletDisplay;  // This is the public field where you assign the BulletDisplay script

    // Add an AudioSource component and AudioClip for shooting sound
    public AudioSource audioSource;  // Reference to the AudioSource component
    public AudioClip shootSound;     // Reference to the shooting sound clip

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();

        // Automatically find the BulletDisplay script in the scene
        if (bulletDisplay == null)
        {
            bulletDisplay = FindFirstObjectByType<BulletUI>();
        }

        // Check if the BulletDisplay script was found
        if (bulletDisplay == null)
        {
            Debug.LogError("BulletDisplay script not found in the scene!");
        }

        // Ensure AudioSource is assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        animator.SetInteger("Bullets", bullets);

        // Update cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Handle reload timer
        if (reloading)
        {
            animator.SetBool("IsReloading", true);
            reloadTime -= Time.deltaTime;

            if (reloadTime <= 0)
            {
                Reload();
                animator.SetBool("IsReloading", false);
            }
        }

        // Rotate the attack area towards the mouse
        RotateAttackAreaTowardsMouse();

        // When left-click is pressed and cooldown finished
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f && !reloading)
        {
            Attack();
        }

        // Handle attack animation and recoil return
        if (attacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                attackTimer = 0f;
                attacking = false;
            }
        }
    }

    private void RotateAttackAreaTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackArea.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Attack()
    {
        if (bullets > 0)
        {
            attacking = true;
            bullets -= 1;
            if (bullets < 0) bullets = 0;

            ShootConeBullets(); // Fire 3 bullets in a cone

            StartCoroutine(ApplyRecoil()); // Start recoil as a coroutine
            cooldownTimer = attackCooldown;

            // Play the shooting sound
            PlayShootSound();
        }

        if (bullets == 0)
        {
            reloading = true;
            reloadTime = 1f;
        }
    }

    // This function plays the shooting sound
    private void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or ShootSound not assigned!");
        }
    }

    private IEnumerator ApplyRecoil()
    {
        // Get the world position of the mouse cursor
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Calculate direction from player to mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Invert direction for recoil (this is the main recoil direction)
        Vector2 recoilDirection = -direction;

        // Apply different multipliers to X and Y without normalizing
        Vector2 customRecoil = new Vector2(
            recoilDirection.x * recoilXMultiplier,
            recoilDirection.y * recoilYMultiplier
        );

        // Final recoil vector, scaled by recoil force and grounded state
        float recoilMultiplier = playerMovement.isGrounded ? 1f : 1.5f;
        Vector2 finalRecoil = customRecoil * recoilForce * recoilMultiplier;

        // Apply the force as an impulse
        rb.AddForce(finalRecoil, ForceMode2D.Impulse);  // Use Impulse for sudden recoil

        // Lock player movement briefly after recoil (optional)
        playerMovement.LockMovementTemporarily();

        // Optional delay for recoil effect
        yield return new WaitForSeconds(0.1f);  // Allow some time for recoil to take effect
    }

    private void Reload()
    {
        bullets = maxBullets;
        reloading = false;
    }

    private void ShootConeBullets()
    {
        int bulletCount = 3;
        float spreadAngle = 30f;
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);

        // Get base aim angle from firePoint (this ensures the bullet follows firePoint's rotation)
        float baseAngle = firePoint.transform.eulerAngles.z;

        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset = startAngle + i * angleStep;
            float finalAngle = baseAngle + angleOffset;

            // Convert angle to radians and calculate direction
            float rad = finalAngle * Mathf.Deg2Rad;
            Vector2 shootDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
            Debug.Log($"Bullet {i} Direction: {shootDir}"); // 👈 Log direction

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetDirection(shootDir);

            // Set the bullet's direction for movement logic in Bullet script
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(shootDir);
            }
        }
    }
}
