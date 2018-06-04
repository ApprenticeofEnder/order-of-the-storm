using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private int health = 10;
    private Damage damage;

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
