using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvincibilityEffects : MonoBehaviour
{
    Damageable damageable;

    bool soundPlayed;

    [SerializeField] TMP_Text invincibilityUI;

    [SerializeField] AudioSource invincibilitySound;

    Color temp;
    // Start is called before the first frame update
    void Start()
    {
        damageable = GetComponent<Damageable>();
        temp = invincibilityUI.color;
        temp.a = 0;
        invincibilityUI.color = temp;
    }

    // Update is called once per frame
    void Update()
    {
        if(!soundPlayed && damageable.isInvincibleCombo)
        {
            invincibilitySound.Play();
            temp.a = 1;
            invincibilityUI.color = temp;
            soundPlayed = true;
        }
        if(soundPlayed && !damageable.isInvincibleCombo)
        {
            soundPlayed = false;
        }
        if(damageable.isInvincibleCombo)
        {
            
            temp.a = temp.a - (1f*Time.deltaTime);
            invincibilityUI.color = temp;

        }
    }
}
