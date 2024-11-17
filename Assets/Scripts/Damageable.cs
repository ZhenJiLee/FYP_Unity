using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;

    Animator animator;

    SpriteRenderer playerSprite;

    [SerializeField] Slider healthBar;

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);
            if (_health <= 0)
            {
                IsAlive = false;
                damageableDeath?.Invoke(); 
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible = false;
    private float timeSinceHit = 0;
    public bool isInvincibleCombo = false;
    private float timeSinceCombo = 0;
    public float comboInvincibilityTime = 1f;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            if (animator != null)
            {
                animator.SetBool(AnimationStrings.isAlive, value);

                if (value == false)
                {
                    damageableDeath.Invoke();
                }
            }
        }
    }

    public bool LockVelocity
    {
        get { return animator != null && animator.GetBool(AnimationStrings.lockVelocity); }
        set
        {
            if (animator != null)
            {
                animator.SetBool(AnimationStrings.lockVelocity, value);
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on " + gameObject.name);
        }
        if(GetComponent<SpriteRenderer>())
        {
            playerSprite = GetComponent<SpriteRenderer>();
        }
    }

    public void Update()
    {
        if (isInvincible)//Invincibility after being hit
        {

            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;

            }

            timeSinceHit += Time.deltaTime;
        }
        else if(isInvincibleCombo)//Invinsibility after getting high combo
        {
            //Make player transparent
            Color temp = playerSprite.color;
            temp.a = .25f;
            playerSprite.color = temp;
            ColorBlock temp2 = healthBar.colors;
            temp2.disabledColor= new Vector4(1f, 0.98f, 0.75f, 1f);
            healthBar.colors = temp2;
            if (timeSinceCombo > comboInvincibilityTime)
            {
                isInvincibleCombo = false;
                timeSinceCombo = 0;

                //Make player opaque again
                temp.a = 1f;
                playerSprite.color = temp;
                temp2.disabledColor = new Vector4(0.08f, 0.82f, 0.067f, 1f);
                healthBar.colors = temp2;
            }

            timeSinceCombo += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if ((IsAlive && !isInvincible) && (IsAlive && !isInvincibleCombo))
        {
            Health -= damage;
            isInvincible = true;

            if (animator != null)
            {
                animator.SetTrigger(AnimationStrings.hitTrigger);
            }
            else
            {
                Debug.LogWarning("Animator is null on " + gameObject.name);
            }

            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged?.Invoke(gameObject, damage);

            return true;
        }

        return false;
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHealth = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHealth, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;
    }
}
