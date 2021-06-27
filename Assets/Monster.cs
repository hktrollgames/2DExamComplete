using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject destroyEffect;
    public int hp = 1;

    public void OnDamage(int damage)
    {
        hp -= damage;
        if( hp <= 0)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
