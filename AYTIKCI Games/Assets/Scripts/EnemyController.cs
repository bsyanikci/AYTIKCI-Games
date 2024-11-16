using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Melee,
    Caster
}

public class EnemyController : MonoBehaviour
{
    public EnemyType enemyType;
    public float health = 100f;
    public float damage = 10f;
    public float attackRange = 1.5f;
    public GameObject targetCPU; // Target to attack (the CPU)
    public GameObject targetTurret; // Target for casters (turrets)
    public int goldReward = 10; // Gold awarded when this enemy is killed


    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Make sure the NavMeshAgent is properly initialized and can move.
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing on " + gameObject.name);
            return;
        }

        if (targetCPU != null)
        {
            // Move towards the CPU as the target
            agent.SetDestination(targetCPU.transform.position);
        }
    }

    void Update()
    {
        // Ensure the enemy moves toward the CPU or turret based on its type
        // Check if the CPU is destroyed or inactive (null)
        if (targetCPU == null)
        {
            // If the CPU is destroyed, stop moving or do something else
            agent.isStopped = true; // Stop the NavMeshAgent from moving
            return; // Exit the Update method to avoid trying to attack the CPU
        }

        if (enemyType == EnemyType.Melee)
        {
            // Melee enemies attack the CPU directly
            if (Vector3.Distance(transform.position, targetCPU.transform.position) <= attackRange)
            {
                AttackCPU();
            }
        }
        else if (enemyType == EnemyType.Caster)
        {
            // Casters should attack turrets if they are within range
            if (targetTurret != null)
            {
                agent.SetDestination(targetTurret.transform.position);

                if (Vector3.Distance(transform.position, targetTurret.transform.position) <= attackRange)
                {
                    AttackTurret();
                }
            }
        }
        if (targetCPU != null)
        {
            Debug.DrawLine(transform.position, targetCPU.transform.position, Color.red); // Visualize the path
        }
    }

    void AttackCPU()
    {
        // Logic for CPU damage (e.g., reduce CPU health)
        CPUHealth cpuHealth = targetCPU.GetComponent<CPUHealth>();
        if (cpuHealth != null)
        {
            cpuHealth.TakeDamage(damage);
        }
    }

    void AttackTurret()
    {
        // Logic for turret damage (e.g., destroy turret)
        Turret turret = targetTurret.GetComponent<Turret>();
        if (turret != null)
        {
            turret.TakeDamage(damage);
        }
    }
    public void PlayDeathAnimation()
    {
        // Implement death animation or effect
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Altýn kazandýr
        GoldManager.Instance.AddGold(goldReward);
        GoldManager.Instance.UpdateGoldDisplay();

        // Düþmaný yok et
        Destroy(gameObject);

        // Örneðin bir animasyon veya ses çalma burada yapýlabilir.
        // Örnek: AudioManager.Instance.PlaySound("EnemyDeath");
    }
}