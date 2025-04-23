using UnityEngine;
using System.Collections;

public class ShotgunAttack : MonoBehaviour
{
    private GameObject attackArea = default;
    public bool attacking = false;
    public float attackDuration = 0.25f;
    public float attackCooldown = 1f;

    private float attackTimer = 0f;
    private float cooldownTimer = 0f;
    public bool shotgunAttacking;

    [SerializeField]
    private float bullets = 2f;
    [SerializeField]
    private float maxBullets = 2f;
    [SerializeField]
    private float reloadTime = 1f;
    [SerializeField]
    private float recoilForce = 5f;

    private bool reloading = false;
    private Rigidbody2D rb;

    // Reference to PlayerMovement (make sure this is set in Inspector or GetComponent)
    public PlayerMovement playerMovement;
    public BulletDisplay bulletDisplay;  // This is the public field where you assign the BulletDisplay script


    void Start()
{
    attackArea = transform.GetChild(0).gameObject;
    rb = GetComponent<Rigidbody2D>();

    // Automatically find the BulletDisplay script in the scene
    if (bulletDisplay == null)
    {
        bulletDisplay = FindObjectOfType<BulletDisplay>();
    }

    // Check if the BulletDisplay script was found
    if (bulletDisplay == null)
    {
        Debug.LogError("BulletDisplay script not found in the scene!");
    }

    // Update bullet count UI initially
    bulletDisplay.UpdateBulletCount(bullets);
}


    void Update()
    {
        // Update cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Handle reload timer
        if (reloading)
        {
            reloadTime -= Time.deltaTime;
            if (reloadTime <= 0)
            {
                Reload();
            }
        }

        // Rotate the attack area towards the mouse position
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
                attackArea.SetActive(false);
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
            attackArea.SetActive(true);
            bullets -= 1;
            bulletDisplay.UpdateBulletCount(bullets);
            if (bullets < 0) bullets = 0;

            StartCoroutine(ApplyRecoil()); // Start recoil as a coroutine

            cooldownTimer = attackCooldown;
        }

        if (bullets == 0)
        {
            reloading = true;
            reloadTime = 1f;
        }
    }

    private IEnumerator ApplyRecoil()
    {
        // Get the world position of the mouse cursor
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Invert the direction (opposite to where the mouse is pointing)
        Vector2 recoilDirection = new Vector2(-direction.x, -direction.y);

        // Apply different recoil based on whether the player is grounded or airborne
        if (playerMovement.isGrounded)
        {
            // Apply recoil with reduced strength when grounded (in the opposite direction of mouse)
            rb.AddForce(recoilDirection * recoilForce * 1f, ForceMode2D.Impulse);  // Full recoil on ground
        }
        else
        {
            // Apply full recoil when airborne
            rb.AddForce(recoilDirection * recoilForce * 1.5f, ForceMode2D.Impulse); // More recoil in the air
        }

        // Wait for a short time to simulate recoil effect
        yield return new WaitForSeconds(0.1f); // Adjust the time to control the recoil duration
    }

    private void Reload()
    {
        bullets = maxBullets;
        reloading = false;
        bulletDisplay.UpdateBulletCount(bullets);
    }
}
