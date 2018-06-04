using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int health;
    private Damage damage;

    public void Damage(int damage) {
        health -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Damage>() != null && other.gameObject.GetComponent<Damage>().attacking && other.gameObject.tag != gameObject.tag)
        {
            damage = other.gameObject.GetComponent<Damage>();
            health -= damage.dmg;
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
