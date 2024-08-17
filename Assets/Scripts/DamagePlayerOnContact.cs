using System.Collections;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public float damageAmount = 10f;
    private Coroutine delayCoroutine;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (delayCoroutine == null)
                delayCoroutine = StartCoroutine(DelayCoroutine());
        }
    }

    IEnumerator DelayCoroutine()
    {
        PlayerController.instance.LostHealth(-damageAmount);
        yield return new WaitForSeconds(1.1f);
        delayCoroutine = null;
    }
}
