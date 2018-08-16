using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

	public GameObject playerObject;

    public GameObject pointObject;
    public Rigidbody myRigidBody;

    public float moveMin;
    public float moveMax;

	public float rotSpeed;

    public float moveSpeed;
    public float delayToPush;

    public bool moveIndependent;

    public bool lightPickup;
    public bool darkPickup;

	public float pickupScore;

	// Use this for initialization
	void Start () {
		playerObject = GameObject.FindGameObjectWithTag ("Player");
        //moveSpeed = (Random.Range(moveMin, moveMax));
        //myRigidBody.AddForce(whatever.transform.forward * moveSpeed);
        StartCoroutine(PushObject());
        
    }

    // Update is called once per frame
    void Update () {
        //if (moveIndependent == false)
        //{
        //transform.Translate(0, -moveSpeed, 0);
        //myRigidBody.AddForce(whatever.transform.forward * moveSpeed);
        //}
        moveSpeed = (Random.Range(moveMin, moveMax));

		if (moveIndependent == true) 
		{
			pointObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerObject.transform.position - transform.position), rotSpeed * Time.deltaTime);
		}

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Border")
        {
            //Debug.Log("Hit border");
            Destroy(gameObject);
        }
    }

    IEnumerator PushObject()
    {
        myRigidBody.AddForce(pointObject.transform.forward * moveSpeed);
        yield return new WaitForSeconds(delayToPush);
        StartCoroutine(PushObject());
    }

}
