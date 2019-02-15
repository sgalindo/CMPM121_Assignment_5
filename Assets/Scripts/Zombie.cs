using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int health = 3;
    private bool isDead = false;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            Die();
        }

        if (!isDead)
        {

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
    }
}
