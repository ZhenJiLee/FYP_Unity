using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    int defaultDamage;
    public Vector2 knockback = Vector2.zero;
    public bool isPlayer;
    public ScoreManager combo;
    public int currentCombo = 5;
    public TMP_Text combotext;

    public bool isComboAttack;

    public AudioSource attackPowerUpSound;
    public ParticleSystem attackPowerUpEffect;
    //public bool canComboAttack;
    private void Start()
    {
        defaultDamage = attackDamage;
        currentCombo = 5;
    }
    private void Update()
    {
        if (combo != null && isPlayer)
        {
            if (ScoreManager.canCombo)
            {
                combotext.text = "Attack Power Up Available" ;
            }
           else
            {
                combotext.text = "";
            }
/*            if(ScoreManager.canCombo)
            {
                canComboAttack = true;
            }
            else
            {
                canComboAttack = false;
            }*/
        }


    }

    private void OnTriggerEnter2D (Collider2D collision) 
    {
    
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback :new Vector2(-knockback.x, -knockback.y);
            if(isPlayer)
            {
                if(ScoreManager.isComboAttack)
                {
                    attackDamage = attackDamage + currentCombo;
                    attackPowerUpSound.Play();
                    attackPowerUpEffect.Play();
                    ScoreManager.canCombo = false;
                    ScoreManager.isComboAttack = false;
                }
            }
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if (gotHit) 
                Debug.Log(collision.name + " hit for " +  attackDamage);
            attackDamage = defaultDamage;
        }
}
}
