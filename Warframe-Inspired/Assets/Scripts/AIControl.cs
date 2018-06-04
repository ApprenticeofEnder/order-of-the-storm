using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour {

    public bool isAlive = true;

	void Start () {
        isAlive = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (isAlive)
        {

        }
        else
        {
            Death();
        }
	}

    private void Death()
    {

    }
}
