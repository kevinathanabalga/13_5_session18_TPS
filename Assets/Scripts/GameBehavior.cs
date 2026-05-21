using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    // Variables untuk menyimpan data game
    private int _itemsCollected = 0;
    private int _playerHP = 10;
    public int MaxItems = 4;

    // Referensi UI
    public TMP_Text HealthText;
    public TMP_Text ItemText;
    public TMP_Text ProgressText;
    public Button WinButton;

    void Start()
    {
        // Initialize UI display
        ItemText.text = "Items: " + _itemsCollected;
        HealthText.text = "Health: " + _playerHP;

        // Sembunyikan win button di awal
        if (WinButton != null)
            WinButton.gameObject.SetActive(false);
    }

    // Property untuk Items dengan getter/setter
    public int Items
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            ItemText.text = "Items: " + _itemsCollected;

            // Cek apakah semua item sudah terkumpul
            if (_itemsCollected >= MaxItems)
            {
                ProgressText.text = "You've found all the items!";
                if (WinButton != null)
                    WinButton.gameObject.SetActive(true);
                Time.timeScale = 0f; // Pause game
            }
            else
            {
                int remaining = MaxItems - _itemsCollected;
                ProgressText.text = "Item found, only " + remaining + " more to go!";
            }
        }
    }

    // Property untuk HP dengan getter/setter
    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            HealthText.text = "Health: " + _playerHP;
            Debug.LogFormat("Lives: {0}", _playerHP);
        }
    }

    // Method untuk restart scene
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}