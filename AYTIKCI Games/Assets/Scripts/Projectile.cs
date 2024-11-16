using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Projectile hýzýný belirler
    public float damage = 10f; // Projectile'nin verdiði hasar

    private Transform target;

    public void Initialize(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Eðer hedef yoksa projectile yok edilir
            return;
        }

        // Hedefe doðru hareket
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Hedefle çarpýþma kontrolü
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        EnemyController enemy = target.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Düþmana hasar uygula
        }

        Destroy(gameObject); // Projectile yok edilir
    }
}