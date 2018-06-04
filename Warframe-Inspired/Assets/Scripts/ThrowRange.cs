using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRange : MonoBehaviour {

    public Sword swordScript;
    private int tDist;
    void Awake () {
        tDist = swordScript.throwDistance;
        transform.position = new Vector3(0, 0,tDist);
        Debug.Log(tDist);
	}
	
}
