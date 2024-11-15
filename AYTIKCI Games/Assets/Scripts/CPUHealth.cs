using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUHealth : MonoBehaviour
{
    public float health = 500f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // Destroy CPU when health is 0
            // Implement game over logic here
        }
    }
}
