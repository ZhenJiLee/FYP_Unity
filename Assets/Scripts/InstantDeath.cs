using UnityEngine;

public class InstantDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            Damageable playerDamageable = collision.GetComponent<Damageable>();
            if (playerDamageable != null)
            {
                playerDamageable.Health = 0; 
            }
        }
    }
}
