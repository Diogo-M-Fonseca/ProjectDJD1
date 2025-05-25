using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class KnifeAttack : MonoBehaviour
{
    private GameObject attackArea = default;  
    public bool attacking = false; 
    public float timeToAttack = 0.25f;  
    private float lastAttackTime = 0f;
    [SerializeField] private Animator animator;

   
    // Start is called before the first frame update
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if(attacking)
        {
            lastAttackTime += Time.deltaTime;


            if (lastAttackTime >= timeToAttack)
            {
                lastAttackTime = 0;
                attacking = false;
                animator.SetBool("Attack", attacking);
                attackArea.SetActive(attacking);
            }
        }
    }
    private void Attack()
    {
        attacking = true;
        animator.SetBool("Attack", attacking);
        attackArea.SetActive(attacking);
    }
    
}
