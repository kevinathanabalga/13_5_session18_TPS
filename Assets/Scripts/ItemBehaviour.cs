using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    private GameBehavior gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameBehavior>();

        if (gameManager == null)
        {
            Debug.LogError(
                "GameBehavior tidak ditemukan di scene!"
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tambah item counter
            if (gameManager != null)
            {
                gameManager.Items++;
            }

            Debug.Log("Item collected!");

            Destroy(gameObject);
        }
    }
}