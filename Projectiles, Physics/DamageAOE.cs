using UnityEngine;
using System.Collections;

public class DamageAOE : MonoBehaviour {

    public int damageValue = 5;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Health>() != null && !other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Health>().Damage(damageValue);
        }
    }
}
