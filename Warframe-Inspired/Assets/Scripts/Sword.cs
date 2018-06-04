using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    public GameObject sword;
    public Rigidbody swordRb;
    public GameObject swordCollBase;
    GameObject swordColl;
    public Transform swordRef;
    public GameObject owner;
    public Transform cameraHolder;
    public int throwDistance;
    public Transform pathEnd;
    public Camera cameraM;
    public Damage damage;
    Vector3 standardPos;
    Quaternion standardRot;
    Vector3 standardScale;
    private bool atEnd = false;
    public bool isThrown = false;
    private bool isDropped = false;
    public bool isSwung = false;
    public float throwSpeed;

	void Awake () {
        standardPos = sword.transform.localPosition;
        standardRot = sword.transform.localRotation;
        standardScale = sword.transform.localScale;
        damage.attacking = false;
    }

    public void SetThrowTrajectory()
    {
        isThrown = true;
        damage.attacking = true;
        atEnd = false;
        Ray ray = cameraM.ScreenPointToRay(new Vector3(cameraM.pixelWidth/2,cameraM.pixelHeight/2,0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, throwDistance))
        {
            pathEnd.position = hit.point;
        }
        else
        {
            pathEnd.position = swordRef.position;
        }
        swordColl = Instantiate(swordCollBase, sword.transform.position, cameraHolder.transform.rotation);
        swordColl.tag = owner.tag;
        sword.transform.parent = swordColl.transform;
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.Euler(0, -90, 0);
        swordRb.isKinematic = false;
    }

    public void Attack()
    {
        Ray ray = new Ray(cameraM.transform.position, cameraM.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5f))
        {
            Debug.Log("Boop.");
            if (hit.collider.gameObject.GetComponent<Health>() != null)
            {
                hit.collider.gameObject.GetComponent<Health>().Damage(damage.dmg);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (isThrown)
        {
            swordColl.transform.position = Vector3.MoveTowards(swordColl.transform.position, pathEnd.position, throwSpeed * Time.deltaTime);
            swordRb.AddTorque(-Vector3.up * 10000);
            if (Vector3.Magnitude(swordColl.transform.position - owner.transform.position) < 0.5f && atEnd)
            {
                sword.transform.parent = cameraHolder;
                isThrown = false;
                sword.transform.localPosition = standardPos;
                sword.transform.localScale = standardScale;
                sword.transform.localRotation = standardRot;
                Destroy(swordColl);
                swordRb.isKinematic = true;
                damage.attacking = false;
            }
            else if (!atEnd)
            {
                if (Vector3.Magnitude(swordColl.transform.position - pathEnd.position) < 0.5)
                {
                    atEnd = true;
                    var reboundDir = swordColl.transform.position - pathEnd.transform.position;
                    Ray ray = new Ray(swordColl.transform.position,reboundDir);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag != "Environment")
                    {
                        pathEnd.position = owner.transform.position;
                    }
                }
            }
            else
            {
                var reboundDir = swordColl.transform.position - pathEnd.transform.position;
                Ray ray = new Ray(swordColl.transform.position, reboundDir);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag != "Environment")
                {
                    pathEnd.position = owner.transform.position;
                }
                else if (Physics.Raycast(ray, out hit, 1f))
                {
                    isThrown = false;
                    isDropped = true;
                }
                else
                {
                    pathEnd.position = owner.transform.position;
                }
            }
        }
		
	}
}
