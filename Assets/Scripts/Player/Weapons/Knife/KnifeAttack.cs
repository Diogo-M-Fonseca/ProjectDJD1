using Unity.VisualScripting;
using UnityEngine;

public class KnifeAttack : MonoBehaviour
{
    private GameObject attackArea = default;  
    public bool attacking = false; 
    public float timeToAttack = 0.25f;  
    public float timer = 0f; 

    private float lastAttackTime = 0f;
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
            timer += Time.deltaTime;

            if(timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }
    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
    }
    
}
