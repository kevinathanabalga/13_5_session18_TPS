using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float LifeTime = 3f;
    public int Damage = 1;

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyBehavior enemy =
            collision.gameObject.GetComponent<EnemyBehavior>();

        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
        }

        Destroy(gameObject);
    }
}