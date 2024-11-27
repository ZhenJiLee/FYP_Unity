using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private const int comboForAttackBoost = 5; 
    private const int comboForInvincibility = 10; 

    Damageable playerDamageable;

    private bool activateInvincibility;
    static AudioSource powerUpSound;
    [SerializeField]AudioSource powerUp;
    private void Awake()
    {
        powerUpSound = powerUp;
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

        
        if (comboScore == comboForAttackBoost)
        {
            canCombo = true;
            powerUpSound.Play();
            Debug.Log("Combo Attack Boost Ready!");
        }

        
        if (comboScore >= comboForInvincibility)
        {
            comboScore = 0; 
            Instance.activateInvincibility = true;
            Debug.Log("Invincibility Activated!");
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
            int damage = healthPenalty;
            playerDamageable.TakeDamage(damage);
            Debug.Log($"Missed 12 times! Player takes {damage} damage.");

            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.CharacterTookDamage(playerDamageable.gameObject, damage);
            }
            else
            {
                Debug.LogWarning("UIManager not found. Cannot display damage text.");
            }
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
    public void EndGameAfterDeath()
    {
        StartCoroutine(WaitAndLoadMainMenu());
    }

    private IEnumerator WaitAndLoadMainMenu()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("MainMenu1");
    }

    private void OnEnable()
    {
        Damageable.playerDeath += EndGameAfterDeath;
    }
    private void OnDisable()
    {
        Damageable.playerDeath -= EndGameAfterDeath;
    }
}
