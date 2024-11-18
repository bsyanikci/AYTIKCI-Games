//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Turret : MonoBehaviour
//{
//    public float health = 500f;
//    public float damage = 10f;
//    public float attackRange = 50f;
//    public int cost = 50;
//    public GameObject targetEnemy; // Enemy to attack

//    void Update()
//    {
//        if (targetEnemy != null && Vector3.Distance(transform.position, targetEnemy.transform.position) <= attackRange)
//        {
//            Attack();
//        }
//    }

//    //void Attack()
//    //{
//    //    // Logic for turret attack on enemies (damage)
//    //    EnemyController enemyController = targetEnemy.GetComponent<EnemyController>();
//    //    if (enemyController != null)
//    //    {
//    //        enemyController.health -= damage;
//    //        if (enemyController.health <= 0)
//    //        {
//    //            GoldManager.Instance.AddGold(enemyController.goldReward); 
//    //            Destroy(targetEnemy);
//    //        }
//    //    }
//    //}
//    public void TakeDamage(float damage)
//    {
//        health -= damage;
//        if (health <= 0)
//        {
//            Destroy(gameObject); // Destroy CPU when health is 0
//            // Implement game over logic here
//        }
//    }

//    void Attack()
//    {
//        // Ensure targetEnemy is not null
//        if (targetEnemy != null)
//        {
//            // Get the EnemyController component of the target enemy
//            EnemyController enemyController = targetEnemy.GetComponent<EnemyController>();

//            if (enemyController != null)
//            {
//                // Apply damage to the enemy's health
//                enemyController.health -= damage;

//                // Log current health for debugging
//                Debug.Log("Enemy's health after attack: " + enemyController.health);

//                // Check if the enemy's health is less than or equal to 0 (i.e., the enemy is dead)
//                if (enemyController.health <= 0)
//                {
//                    // Reward gold for killing the enemy
//                    GoldManager.Instance.AddGold(enemyController.goldReward);
//                    GoldManager.Instance.UpdateGoldDisplay();
//                    Debug.Log("Enemy killed! Gold rewarded.");

//                    // Destroy the enemy instance from the scene
//                    Destroy(targetEnemy); // This will destroy the actual instance of the enemy in the game

//                    // Optionally, you could also play some death animation or effect before destroying the enemy
//                    // Example: Play death animation, sound, or spawn particle effects here before destroying
//                }
//            }
//            else
//            {
//                Debug.LogError("EnemyController component is missing on the target enemy.");
//            }
//        }
//        else
//        {
//            Debug.LogError("Target enemy is null, cannot attack.");
//        }
//    }
//}

using UnityEngine;

public class Turret : MonoBehaviour
{
    public float health = 500f;
    public float attackRange = 5f;       // Range within which the turret will detect enemies
    public float attackCooldown = 1f;    // Time between attacks
    public float damage = 10f;           // Damage dealt per attack
    public int cost = 50;
    public float attackSpeed = 1f;       // Time between attacks (per second)

    private GameObject targetEnemy;      // Reference to the enemy the turret is attacking
    private float attackTimer = 0f;      // Timer to control attack cooldown

    public Transform turretHead;         // If you want the turret to rotate to face the enemy (optional)

    public GameObject projectilePrefab; // Prefab ayarlanacak
    public Transform firePoint; // Projectile'nin fýrlatýlacaðý nokta

    public AudioClip attackSound; // Sound effect for the turret attack
    //void Update()
    //{
    //    // Update attack timer
    //    attackTimer -= Time.deltaTime;

    //    // Detect and find the nearest enemy
    //    FindEnemyInRange();

    //    // If a target is found, attack it
    //    if (targetEnemy != null)
    //    {
    //        // Rotate turret head towards the enemy
    //        RotateTowardsEnemy();

    //        // Attack if the cooldown is over
    //        if (attackTimer <= 0f)
    //        {
    //            Attack();
    //            attackTimer = attackCooldown;  // Reset the attack cooldown timer
    //        }
    //    }


    //}

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (targetEnemy == null || !IsTargetInRange())
        {
            FindEnemyInRange();
        }

        if (targetEnemy != null)
        {
            RotateTowardsEnemy();

            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    bool IsTargetInRange()
    {
        if (targetEnemy == null) return false;
        return Vector3.Distance(transform.position, targetEnemy.transform.position) <= attackRange;
    }

    // Find the closest enemy within the attack range
    void FindEnemyInRange()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange);
        float closestDistance = Mathf.Infinity;

        targetEnemy = null;

        foreach (Collider col in enemiesInRange)
        {
            if (col.CompareTag("Enemy")) // Make sure we're only detecting enemies
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = col.gameObject; // Set the closest enemy as the target
                }
            }
        }
    }

    // Attack the target enemy by applying damage
    //void Attack()
    //{
    //    if (targetEnemy != null)
    //    {
    //        EnemyController enemyController = targetEnemy.GetComponent<EnemyController>();
    //        if (enemyController != null)
    //        {
    //            // Apply damage to the enemy
    //            enemyController.health -= damage;

    //            // If the enemy's health is 0 or below, destroy the enemy
    //            if (enemyController.health <= 0)
    //            {
    //                // Reward gold for killing the enemy
    //                GoldManager.Instance.AddGold(enemyController.goldReward);
    //                GoldManager.Instance.UpdateGoldDisplay();
    //                Debug.LogError("Money : " + GoldManager.Instance.currentGold);
    //                // Optionally, play death animation, sound, or spawn effects here
    //                // Example: enemyController.PlayDeathAnimation();
    //                // AudioManager.Instance.PlaySound("EnemyDeath");

    //                // Destroy the enemy from the scene
    //                Destroy(targetEnemy);  // Destroy the target enemy instance
    //            }
    //        }
    //    }
    //}


    void Attack()
    {
        if (targetEnemy != null)
        {
            GameObject projectileInstance = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Projectile projectileScript = projectileInstance.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                // Play attack sound
                if (attackSound != null)
                {
                    Debug.Log("AttackSound");
                    SoundManager.Instance.PlaySound(attackSound);
                }
                projectileScript.Initialize(targetEnemy.transform);
                projectileScript.damage = damage; // Projectile'ye hasar deðeri aktarýlýr
                
                
            }
        }
    }

    // Rotate the turret to face the enemy
    void RotateTowardsEnemy()
    {
        if (turretHead != null && targetEnemy != null)
        {
            Vector3 direction = targetEnemy.transform.position - turretHead.position;
            direction.y = 0; // Ignore vertical component for horizontal-only rotation
            Quaternion rotation = Quaternion.LookRotation(direction);
            turretHead.rotation = Quaternion.Slerp(turretHead.rotation, rotation, Time.deltaTime * attackSpeed);
        }
    }

    // Draw the range sphere in the editor for visualization (optional)
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //}

    void OnDrawGizmos()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(firePoint.position, 0.1f); // FirePoint pozisyonunu görselleþtirir
            Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.forward * 2); // Ateþ yönünü çizer
        }
    }

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