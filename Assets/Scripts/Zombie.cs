using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Salvador Galindo
 * sagalind
 * CMPM 121 - Assignment 5
 * Zombie.cs - Basic AI script for zombie along with its animations
 */
public class Zombie : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [HideInInspector] public bool isDead = false;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float minAttackDistance = 0.5f;

    private Animator anim;

    private GameObject player;

    private bool isWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= maxDistance)
            {
                ChasePlayer();
                if (distance <= minAttackDistance)
                {
                    anim.SetTrigger("IsAttacking");
                }
            }
            else
            {
                if (isWalking)
                {
                    isWalking = false;
                }
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void DecreaseHealth(int damage)
    {
        health -= damage;
    }

    private void Die()
    {
        isDead = true;
        anim.SetBool("IsDead", isDead);
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void ChasePlayer()
    {
        if (!isWalking)
        {
            isWalking = true;
            anim.SetBool("IsWalking", isWalking);
        }
        transform.LookAt(player.transform);
        GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
    }
}
