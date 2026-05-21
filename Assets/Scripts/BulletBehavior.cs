using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float LifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}