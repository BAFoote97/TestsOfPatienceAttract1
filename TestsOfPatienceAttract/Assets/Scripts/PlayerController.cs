using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public GameObject gunRotate;
    public GameObject emitter;
    public GameObject bullet;
    public GameObject lightBullet;
    public GameObject darkBullet;
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
    public float highScore;
    public float baseHighScore;
    public Text highScoreText;
    public GameObject gameOverText;

    public GameObject emitterSide1;
    public GameObject emitterSide2;
    public GameObject emitterSide3;
    public GameObject emitterSide4;

    public bool paused;
    public GameObject pauseMenu;

    public GameObject newHighScore;

    public bool wallKillsPlayer;
    public GameObject wallWarnings;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        gameOverText.SetActive(false);

        newHighScore.SetActive(false);
        baseHighScore = PlayerPrefs.GetFloat("High Score");
        highScore = baseHighScore;

        //emitterSide1.SetActive(false);
        //emitterSide2.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        //if (wallKillsPlayer == true)
        //{
        //    wallWarnings.SetActive(true);
        //}
        //if (wallKillsPlayer == false)
        //{
        //    wallWarnings.SetActive(false);
        //}
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetHighScore();
        }

        if (paused == true)
        {
            pauseMenu.SetActive(true);
        }
        if (paused == false)
        {
            pauseMenu.SetActive(false);
        }

        scoreText.text = playerScore.ToString();
        highScoreText.text = highScore.ToString();

        if (playerScore > baseHighScore)
        {
            highScore = playerScore;
            newHighScore.SetActive(true);
        }

        rend.material = currentMat;

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        RaycastHit hit;

        if (groundPlane.Raycast(cameraRay, out rayLength) && paused == false)
        {

            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);


            //            gunRotate.transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            //mainCamera.transform.LookAt (new Vector3(pointToLook.x,transform.position.y, pointToLook.z));
            gunRotate.transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            gunRotate.transform.rotation = Quaternion.Euler(new Vector3(0, gunRotate.transform.rotation.eulerAngles.y, 0));

        }

        if (Input.GetMouseButtonDown(0) && paused == false)
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
            bullet = lightBullet;
            gameObject.layer = 11;
            currentMat = lightMat;
            lightParticles.SetActive(true);
            darkParticles.SetActive(false);
        }
        if (darkMode == true)
        {
            bullet = darkBullet;
            gameObject.layer = 12;
            currentMat = darkMat;
            darkParticles.SetActive(true);
            lightParticles.SetActive(false);
        }

        if (playerScore <= 100)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 50;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 50;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 50;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 50;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 20;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 20;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 20;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 20;
        }

        if (playerScore > 100 && playerScore <= 200)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 45;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 45;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 45;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 45;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 19;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 19;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 19;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 19;
        }

        if (playerScore > 200 && playerScore <= 300)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 45;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 45;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 45;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 45;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 18;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 18;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 18;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 18;
        }

        if (playerScore > 300 && playerScore <= 400)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 40;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 40;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 40;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 40;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 17;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 17;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 17;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 17;
        }

        if (playerScore > 400 && playerScore <= 500)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 35;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 35;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 35;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 35;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 16;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 16;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 16;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 16;
        }

        if (playerScore > 500 && playerScore <= 600)
        {
            //emitterSide1.SetActive(true);
            //emitterSide2.SetActive(true);
            emitterSide1.GetComponent<EmitterScript>().delayMax = 35;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 35;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 35;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 35;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 15;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 15;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 15;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 15;
        }

        if (playerScore > 600 && playerScore <= 700)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 30;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 30;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 30;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 30;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 14;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 14;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 14;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 14;
        }

        if (playerScore > 700 && playerScore <= 800)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 27;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 27;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 27;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 27;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 13;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 13;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 13;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 13;
        }

        if (playerScore > 800 && playerScore <= 900)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 25;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 25;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 25;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 25;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 13;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 13;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 13;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 13;
        }

        if (playerScore > 900 && playerScore <= 1000)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 25;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 25;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 25;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 25;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 12;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 12;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 12;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 12;
        }

        if (playerScore > 1000 && playerScore <= 1100)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 22;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 22;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 22;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 22;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 12;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 12;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 12;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 12;
        }

        if (playerScore > 1100 && playerScore <= 1200)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 20;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 20;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 20;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 20;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 12;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 12;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 12;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 12;
        }

        if (playerScore > 1200 && playerScore <= 1300)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 20;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 20;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 20;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 20;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 11;
        }

        if (playerScore > 1300 && playerScore <= 1400)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 18;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 18;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 18;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 18;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 11;
        }

        if (playerScore > 1400 && playerScore <= 1500)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 17;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 17;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 17;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 17;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 11;
        }

        if (playerScore > 1500 && playerScore <= 1600)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 15;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 15;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 15;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 15;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 11;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 11;
        }

        if (playerScore > 1600 && playerScore <= 1700)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 15;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 15;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 15;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 15;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 10;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 10;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 10;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 10;
        }

        if (playerScore > 1700 && playerScore <= 1800)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 13;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 13;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 13;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 13;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 9;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 9;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 9;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 9;
        }

        if (playerScore > 1800 && playerScore <= 1900)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 12;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 12;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 12;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 12;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 8;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 8;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 8;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 8;
        }

        if (playerScore > 1900 && playerScore <= 2000)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 10;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 7;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 7;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 7;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 7;
        }

        if (playerScore > 2000 && playerScore <= 2100)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 10;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 6;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 6;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 6;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 6;
        }

        if (playerScore > 2100 && playerScore <= 2200)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 10;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 5;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 5;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 5;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 5;
        }

        if (playerScore > 2200 && playerScore <= 2300)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 10;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 4;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 4;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 4;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 4;
        }

        if (playerScore > 2300 && playerScore <= 2400)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 10;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 3;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 3;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 3;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 3;
        }

        if (playerScore > 2400 && playerScore <= 2500)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 10;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 10;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 2;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 2;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 2;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 2;
        }

        if (playerScore > 2500 && playerScore <= 2600)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 9;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 9;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 9;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 9;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 2;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 2;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 2;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 2;
        }

        if (playerScore > 2600 && playerScore <= 2700)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 8;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 8;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 8;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 8;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 2;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 2;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 2;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 2;
        }

        if (playerScore > 2700 && playerScore <= 2800)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 7;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 7;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 7;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 7;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 1;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 1;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 1;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 1;
        }

        if (playerScore > 2800 && playerScore <= 2900)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 6;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 6;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 6;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 6;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 1;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 1;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 1;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 1;
        }

        if (playerScore > 2900 && playerScore <= 3000)
        {
            emitterSide1.GetComponent<EmitterScript>().delayMax = 5;
            emitterSide2.GetComponent<EmitterScript>().delayMax = 5;
            emitterSide3.GetComponent<EmitterScript>().delayMax = 5;
            emitterSide4.GetComponent<EmitterScript>().delayMax = 5;

            emitterSide1.GetComponent<EmitterScript>().delayMin = 0.5f;
            emitterSide2.GetComponent<EmitterScript>().delayMin = 0.5f;
            emitterSide3.GetComponent<EmitterScript>().delayMin = 0.5f;
            emitterSide4.GetComponent<EmitterScript>().delayMin = 0.5f;
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
        if (other.gameObject.tag == "Border" && wallKillsPlayer == true)
        {
            Debug.Log("Player died");
            gameOverText.SetActive(true);

            Destroy(this.gameObject);
        }

    }

    public void OnDestroy()
    {
        //mainCamera.GetComponent<AudioSource>().enabled = false;
        PlayerPrefs.SetFloat("High Score", highScore);
    }

    public void PauseGame()
    {
        paused = !paused;
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            paused = true;
        }
        else
        {
            Time.timeScale = 1;
            paused = false;
        }
    }

    public void ResetHighScore()
    {
        baseHighScore = 0;

        highScore = 0;

        PlayerPrefs.SetFloat("High Score", 0);
    }
    
}
