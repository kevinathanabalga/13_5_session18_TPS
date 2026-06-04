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
    public Button LossButton;

    void Start()
    {
        // Update UI awal
        if (ItemText != null)
            ItemText.text = "Items: " + _itemsCollected;

        if (HealthText != null)
            HealthText.text = "Health: " + _playerHP;

        // Sembunyikan tombol
        if (WinButton != null)
            WinButton.gameObject.SetActive(false);

        if (LossButton != null)
            LossButton.gameObject.SetActive(false);

        // Sembunyikan cursor saat bermain
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;
    }

    public int Items
    {
        get
        {
            return _itemsCollected;
        }
        set
        {
            _itemsCollected = value;

            if (ItemText != null)
            {
                ItemText.text = "Items: " + _itemsCollected;
            }

            if (_itemsCollected >= MaxItems)
            {
                WinGame();
            }
            else
            {
                int remaining = MaxItems - _itemsCollected;

                if (ProgressText != null)
                {
                    ProgressText.text =
                        "Item found, only " +
                        remaining +
                        " more to go!";
                }
            }
        }
    }

    public int HP
    {
        get
        {
            return _playerHP;
        }
        set
        {
            _playerHP = value;

            if (HealthText != null)
            {
                HealthText.text =
                    "Health: " + _playerHP;
            }

            Debug.Log("Lives: " + _playerHP);

            if (_playerHP <= 0)
            {
                LoseGame();
            }
            else
            {
                if (ProgressText != null)
                {
                    ProgressText.text =
                        "Ouch... that's got hurt.";
                }
            }
        }
    }

    void WinGame()
    {
        if (ProgressText != null)
        {
            ProgressText.text =
                "You've found all the items!";
        }

        if (WinButton != null)
        {
            WinButton.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void LoseGame()
    {
        if (ProgressText != null)
        {
            ProgressText.text =
                "You want another life with that?";
        }

        if (LossButton != null)
        {
            LossButton.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}