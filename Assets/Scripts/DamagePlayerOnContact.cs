using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public float damageAmount = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.ChangeHealth(-damageAmount);
        }
    }
}
