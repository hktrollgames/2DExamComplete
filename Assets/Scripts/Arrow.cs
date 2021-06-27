using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Monster monster = collision.transform.GetComponent<Monster>();
        if (monster == null)
            return;

        monster.OnDamage(damage);
    }
}
