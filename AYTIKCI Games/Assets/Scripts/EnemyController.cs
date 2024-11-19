using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType
{
    Melee,
    Caster,
    Boss
}

public class EnemyController : MonoBehaviour
{
    public GameObject WinPanel;
    public GameObject OpenMenu;
    public GameObject TurretSelectionMenu;
    public EnemyType enemyType;
    public float health = 100f;
    public float damage = 10f;
    public float attackRange = 1.5f;
    public GameObject targetCPU; // Target to attack (the CPU)
    public GameObject targetTurret; // Target for casters (turrets)
    public int goldReward = 10; // Gold awarded when this enemy is killed
    public float attackSpeed = 1f;    // Number of seconds between attacks
    private float attackCooldown = 0f;   // Timer to manage attack intervals
    public GameObject attackEffect; // Reference to the particle effect prefab
    public Transform attackEffectSpawnPoint; // Where the effect spawns (e.g., enemy hand or weapon)


    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {

        GameObject canvas = GameObject.Find("UI"); // Ensure the parent is active
        if (canvas != null)
        {
            WinPanel = canvas.transform.Find("WinPanel").gameObject;
        }
        OpenMenu = GameObject.Find("OpenMenu");
        if (OpenMenu == null)
        {
            Debug.LogError("OpenMenu not found! Ensure the name matches in the Hierarchy.");
        }
        TurretSelectionMenu = GameObject.Find("TurretSelection");
        if (TurretSelectionMenu == null)
        {
            Debug.LogError("TurretSelection Menu not found! Ensure the name matches in the Hierarchy.");
        }

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

    void FindNearestTurret()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        float closestDistance = Mathf.Infinity;
        GameObject closestTurret = null;

        foreach (GameObject turret in turrets)
        {
            float distance = Vector3.Distance(transform.position, turret.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTurret = turret;
            }
        }

        if (closestTurret != null)
        {
            targetTurret = closestTurret;
            //agent.SetDestination(targetTurret.transform.position);
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

        attackCooldown -= Time.deltaTime;

        FindNearestTurret();

        //if (enemyType == EnemyType.Melee)
        //{
        // Melee enemies attack the CPU directly
        if (Vector3.Distance(transform.position, targetCPU.transform.position) <= attackRange && attackCooldown <= 0f)
            {
                    Debug.LogWarning("Attack CPU");
                    AttackCPU();
                    attackCooldown = attackSpeed; // Reset the cooldown timer
            }
        //}
        //else if (enemyType == EnemyType.Caster)
        //{
            // Casters should attack turrets if they are within range
            if (targetTurret != null)
            {
                //agent.SetDestination(targetTurret.transform.position);

                if (Vector3.Distance(transform.position, targetTurret.transform.position) <= attackRange && attackCooldown <= 0f)
                {
                    Debug.Log("Attacked Turret: " + damage);
                    if (attackCooldown <= 0f)
                    {
                        AttackTurret();
                        attackCooldown = attackSpeed; // Reset the cooldown timer
                    }
                    
                }
            }
        //}
        if (targetCPU != null)
        {
            Debug.DrawLine(transform.position, targetCPU.transform.position, Color.red); // Visualize the path
        }
    }

    void TriggerAttackEffect()
    {
        if (attackEffect != null && attackEffectSpawnPoint != null)
        {
            // Instantiate the particle effect at the specified spawn point
            GameObject effectInstance = Instantiate(attackEffect, attackEffectSpawnPoint.position, Quaternion.Euler(-90, 0, 0));

            // Destroy the particle effect after its duration
            ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(effectInstance, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                // If the effect has no ParticleSystem, destroy it after a default time (e.g., 2 seconds)
                Destroy(effectInstance, 2f);
            }
        }
    }

    void AttackCPU()
    {
        // Logic for CPU damage (e.g., reduce CPU health)
        CPUHealth cpuHealth = targetCPU.GetComponent<CPUHealth>();
        if (cpuHealth != null)
        {
            Debug.LogWarning("Attacked CPU");
            cpuHealth.TakeDamage(damage);
            TriggerAttackEffect();
        }
    }

    void AttackTurret()
    {
        // Logic for turret damage (e.g., destroy turret)
        Turret turret = targetTurret.GetComponent<Turret>();
        if (targetCPU.GetComponent<CPUHealth>() == null)
        {
            Debug.LogError("CPUHealth component is missing on the targetCPU GameObject!");
        }
        if (turret != null)
        {
            Debug.LogWarning("Attack Turret");
            turret.TakeDamage(damage);
            TriggerAttackEffect();
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
            if (enemyType == EnemyType.Boss) // Check if this enemy is the boss
            {
                ShowWinMenu();
                Time.timeScale = 0; // Pause the game
            }
            Die();
        }
    }

    private void ShowWinMenu()
    {
        if (WinPanel != null)
        {
            WinPanel.SetActive(true); // Activate the win panel
            TurretSelectionMenu.SetActive(false);
            OpenMenu.SetActive(false);
            Time.timeScale = 0; // Pause the game
        }
        else
        {
            Debug.LogWarning("Win Panel is not assigned in the Inspector.");
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