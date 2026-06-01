using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;

    private int currentHealth;

    public Slider healthBar;

    private void Start()
    {
        currentHealth = PlayerPrefs.GetInt("PlayerHP", maxHealth);

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        PlayerPrefs.SetInt("PlayerHP", currentHealth);
        PlayerPrefs.Save();

        healthBar.value = currentHealth;

        Debug.Log("Player kena damage!");

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
