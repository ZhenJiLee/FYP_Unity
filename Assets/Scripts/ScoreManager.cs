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
    }
}
