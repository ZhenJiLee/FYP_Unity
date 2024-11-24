using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    public static bool canCombo;
    public static bool isComboAttack;

    static int comboScore;
    private int missCount = 0; 
    private const int maxMissCount = 12; 
    private const int healthPenalty = 2;
    Damageable playerDamageable;
    static bool activateInvincibility;

    private void Awake()
    {
        isComboAttack = false;
        canCombo = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDamageable = player.GetComponent<Damageable>();
    }

    void Start()
    {
        Instance = this;
        comboScore = 0;
    }

    public static void Hit()
    {
        comboScore += 1;
        Instance.hitSFX.Play();
        Instance.ResetMissCount(); 

        if (comboScore >= 10)
        {
            comboScore = 0;
            activateInvincibility = true;
            canCombo = true;
        }
    }

    public static void Miss()
    {
        comboScore = 0;
        Instance.missSFX.Play();
        Instance.missCount += 1; 

        
        if (Instance.missCount >= ScoreManager.maxMissCount) 
        {
            Instance.ApplyMissPenalty(); 
            Instance.ResetMissCount(); 
        }
    }

    private void ApplyMissPenalty()
    {
        if (playerDamageable != null && !playerDamageable.isInvincibleCombo)
        {
            playerDamageable.TakeDamage(healthPenalty); 
            Debug.Log("Missed 6 times! Player takes 2 damage.");
        }
    }

    private void ResetMissCount()
    {
        missCount = 0; 
    }

    private void Update()
    {
        scoreText.text = comboScore.ToString();
        if (activateInvincibility)
        {
            activateInvincibility = false;
            playerDamageable.isInvincibleCombo = true;
        }
    }
}
