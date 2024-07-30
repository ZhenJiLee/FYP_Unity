using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable playerDamageable;

    private Quaternion originalRotation;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("No Player found in the scene. Make sure it has tag 'Player'");
        }

        playerDamageable = player.GetComponent<Damageable>();
        originalRotation = transform.rotation;
    }

    private void Start()
    {
        healthSlider.value = CalculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);
        healthBarText.text = "HP" + playerDamageable.Health + "/" + playerDamageable.MaxHealth;
    }

    private void OnEnable()
    {
        playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int MaxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, MaxHealth);
        healthBarText.text = "HP" + newHealth + "/" + MaxHealth;
    }

    private void LateUpdate()
    {
        transform.rotation = originalRotation;
    }
}
