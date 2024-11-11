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
    public static int currentCombo = 0;
    public TMP_Text combotext;
    private void Start()
    {
        defaultDamage = attackDamage;
        currentCombo = 0;
    }
    private void Update()
    {
        if(combo!=null)
        {
            if(currentCombo<combo.combo)
            {
                currentCombo = combo.combo;
                
            }
            combotext.text = "Combo " + currentCombo;
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
                attackDamage = attackDamage + currentCombo;
                currentCombo = 0;
            }
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if (gotHit) 
                Debug.Log(collision.name + " hit for " +  attackDamage);
            attackDamage = defaultDamage;
        }
}
}
