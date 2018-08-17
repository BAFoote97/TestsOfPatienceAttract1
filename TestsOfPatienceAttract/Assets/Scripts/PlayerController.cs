using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public GameObject gunRotate;
    public GameObject emitter;
    public GameObject bullet;
    public float bulletForce;
    public float bulletLifetime;

    public Rigidbody myRigidbody;

    public float thrust;

    public bool lightMode;
    public bool darkMode;

    public Camera mainCamera;

    public Material currentMat;
    public Material lightMat;
    public GameObject lightParticles;
    public Material darkMat;
    public GameObject darkParticles;
    public Renderer rend;

    public float playerScore;
    public Text scoreText;
    public GameObject gameOverText;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        gameOverText.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = playerScore.ToString();

        rend.material = currentMat;

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        RaycastHit hit;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {

            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);


            //            gunRotate.transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            //mainCamera.transform.LookAt (new Vector3(pointToLook.x,transform.position.y, pointToLook.z));
            gunRotate.transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            gunRotate.transform.rotation = Quaternion.Euler(new Vector3(0, gunRotate.transform.rotation.eulerAngles.y, 0));

        }

        if (Input.GetMouseButtonDown(0))
        {
            //The Bullet Instantiation happens here.
            GameObject Temporary_Bullet_handler;

            Temporary_Bullet_handler = Instantiate(bullet, emitter.transform.position, emitter.transform.rotation) as GameObject;
            //Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
            //This is EASILY corrected here, you might have to rotate it from a different axis and/or angle based on your particular mesh.
            Temporary_Bullet_handler.transform.Rotate(Vector3.left);

            //Retrieve the Rigidbody component from the instantiated Bullet and control it.
            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_handler.GetComponent<Rigidbody>();

            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
            Temporary_RigidBody.AddForce(gunRotate.transform.forward * bulletForce, ForceMode.Impulse);

            //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            Destroy(Temporary_Bullet_handler, bulletLifetime);
            myRigidbody.AddForce(gunRotate.transform.forward * -thrust);
        }

        //        if (Input.GetMouseButtonDown(1) && lightMode == true && darkMode == false)
        //        {
        //            lightMode = false;
        //            darkMode = true;
        //        }
        //        if (Input.GetMouseButtonDown(1) && darkMode == true && lightMode == false)
        //        {
        //            darkMode = false;
        //            lightMode = true;
        //        }

        //        if (lightMode == false)
        //        {
        //            darkMode = true;
        //        }
        //        if (darkMode == false)
        //        {
        //            lightMode = true;
        //        }

        if (Input.GetMouseButtonDown(1))
        {
            lightMode = !lightMode;
            darkMode = !darkMode;
        }

        if (lightMode == true)
        {
            gameObject.layer = 11;
            currentMat = lightMat;
            lightParticles.SetActive(true);
            darkParticles.SetActive(false);
        }
        if (darkMode == true)
        {
            gameObject.layer = 12;
            currentMat = darkMat;
            darkParticles.SetActive(true);
            lightParticles.SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "DarkObstacle" && lightMode == true)
        {
            //Debug.Log("Player died");
            gameOverText.SetActive(true);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "LightObstacle" && darkMode == true)
        {
            //Debug.Log("Player died");
            gameOverText.SetActive(true);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "LightObstacle" && lightMode == true)
        {
            if (other.gameObject.GetComponent<ObstacleScript>().lightPickup == true)
            {
                playerScore += other.gameObject.GetComponent<ObstacleScript>().pickupScore;
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "DarkObstacle" && darkMode == true)
        {
            if (other.gameObject.GetComponent<ObstacleScript>().darkPickup == true)
            {
                playerScore += other.gameObject.GetComponent<ObstacleScript>().pickupScore;
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "Border")
        {
            Debug.Log("Player died");
            gameOverText.SetActive(true);

            Destroy(this.gameObject);
        }

    }

    
}
