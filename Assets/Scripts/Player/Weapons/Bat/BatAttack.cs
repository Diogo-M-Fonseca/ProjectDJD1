using OkapiKit;
using UnityEditor.Callbacks;
using UnityEngine;

public class BatAttack : MonoBehaviour
{
    private GameObject attackArea = default;
    public bool attacking = false;
    public float attackDuration = 0.25f;     // How long the attack area stays active
    public float attackCooldown = 1f;         // How long you have to wait between attacks

    private float attackTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        // Update cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // When left click is pressed and cooldown finished
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f)
        {
            Attack();
        }

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

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(true);
        cooldownTimer = attackCooldown;  // Reset cooldown when you attack
    }

}
