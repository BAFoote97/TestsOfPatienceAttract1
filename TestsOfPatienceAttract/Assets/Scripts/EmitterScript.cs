using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterScript : MonoBehaviour {

    public List<GameObject> possibleProjectile = new List<GameObject>();

    public GameObject projectile;
    public int projectileNumber;
    public float projectileLifetime;
    public float projectileForce;

    public bool canShoot;

    public float delayTime;
    public float delayMin;
    public float delayMax;

    public bool startBool;

    public GameObject parent;
    public bool isParent;


	// Use this for initialization
	void Start () {
        startBool = true;
        canShoot = false;

	}
	
	// Update is called once per frame
	void Update () {
        projectileNumber = (Random.Range(0, possibleProjectile.Count));

        projectile = possibleProjectile[projectileNumber];

        delayTime = (Random.Range(delayMin, delayMax));
        if (startBool == true)
        {
            StartCoroutine(ShootDelay());
            startBool = false;
        }

        if (isParent == false)
        {
            delayMin = parent.GetComponent<EmitterScript>().delayMin;
            delayMax = parent.GetComponent<EmitterScript>().delayMax;
        }


        if (canShoot == true && isParent == false)
        {
            //The Bullet Instantiation happens here.
            GameObject Temporary_Bullet_handler;

            Temporary_Bullet_handler = Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            //Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
            //This is EASILY corrected here, you might have to rotate it from a different axis and/or angle based on your particular mesh.
            Temporary_Bullet_handler.transform.Rotate(Vector3.left);

            //Retrieve the Rigidbody component from the instantiated Bullet and control it.
            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_handler.GetComponent<Rigidbody>();

            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
            Temporary_RigidBody.AddForce(transform.forward * projectileForce, ForceMode.Impulse);

            //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            Destroy(Temporary_Bullet_handler, projectileLifetime);

            canShoot = false;
            StartCoroutine(ShootDelay());
        }

        

    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(delayTime);
        canShoot = true;
    }

    

}
