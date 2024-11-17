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
    static int comboScore;
    public int combo;
    Damageable playerDamageable;
    static bool activateInvincibility;
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDamageable = player.GetComponent<Damageable>();
    }
    void Start()
    {
        Instance = this;
        comboScore = 0;
        combo = 0;
    }

    public static void Hit()
    {
        comboScore += 1; 
        Instance.hitSFX.Play();
        if(comboScore>=5)
        {
            comboScore = 0;
            activateInvincibility = true;
        }
    }
    public static void Miss()
    {
        comboScore = 0;
        Instance.missSFX.Play();
    }
     

    private void Update()
    {
        scoreText.text = comboScore.ToString();
        combo = comboScore;
        if(activateInvincibility)
        {
            activateInvincibility = false;
            playerDamageable.isInvincibleCombo = true;
        }
    }
}
