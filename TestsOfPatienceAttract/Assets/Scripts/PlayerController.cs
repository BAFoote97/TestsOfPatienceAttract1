using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public float topScore;
    public float baseHighScore;
    public Text highScoreText;
    public GameObject gameOverText;

    public List<float> Top10Scores = new List<float>();

    public bool menuRef;
    private float defaultScore1 = 100;
    public Text score1Text;

    private float defaultScore2 = 90;
    public Text score2Text;

    private float defaultScore3 = 80;
    public Text score3Text;

    private float defaultScore4 = 70;
    public Text score4Text;

    private float defaultScore5 = 60;
    public Text score5Text;

    private float defaultScore6 = 50;
    public Text score6Text;

    private float defaultScore7 = 40;
    public Text score7Text;

    private float defaultScore8 = 30;
    public Text score8Text;

    private float defaultScore9 = 20;
    public Text score9Text;

    private float defaultScore10 = 10;
    public Text score10Text;


    public GameObject emitterSide1;
    public GameObject emitterSide2;
    public GameObject emitterSide3;
    public GameObject emitterSide4;

    public bool paused;
    public GameObject pauseMenu;

    public GameObject newHighScore;

    public bool wallKillsPlayer;
    public GameObject wallWarnings;
    public GameObject fogFX;
    public int gameModeInt;

    // Use this for initialization
    void Start()
    {
        gameModeInt = PlayerPrefs.GetInt("Game Mode Int");
        if (gameModeInt == 1)
        {
            wallKillsPlayer = true;
        }
        if (gameModeInt == 2)
        {
            wallKillsPlayer = false;
        }

        if (menuRef == false)
        {
            rend = GetComponent<Renderer>();
            gameOverText.SetActive(false);
			newHighScore.SetActive(false);

            
        }
        baseHighScore = PlayerPrefs.GetFloat("High Score");
		topScore = baseHighScore;
//            Top10Scores[0] = PlayerPrefs.GetFloat("TopScore1");
//        Debug.Log(Top10Scores[0]);
//        if (Top10Scores[0] < defaultScore1)
//        {
//            Top10Scores[0] = defaultScore1;
//        }
//
//            Top10Scores[1] = PlayerPrefs.GetFloat("TopScore2");
//        if (Top10Scores[1] < defaultScore2)
//        {
//            Top10Scores[1] = defaultScore2;
//        }
//        Top10Scores[2] = PlayerPrefs.GetFloat("TopScore3");
//        if (Top10Scores[2] < defaultScore3)
//        {
//            Top10Scores[2] = defaultScore3;
//        }
//        Top10Scores[3] = PlayerPrefs.GetFloat("TopScore4");
//        if (Top10Scores[3] < defaultScore4)
//        {
//            Top10Scores[3] = defaultScore4;
//        }
//        Top10Scores[4] = PlayerPrefs.GetFloat("TopScore5");
//        if (Top10Scores[4] < defaultScore5)
//        {
//            Top10Scores[4] = defaultScore5;
//        }
//        Top10Scores[5] = PlayerPrefs.GetFloat("TopScore6");
//        if (Top10Scores[5] < defaultScore6)
//        {
//            Top10Scores[5] = defaultScore6;
//        }
//        Top10Scores[6] = PlayerPrefs.GetFloat("TopScore7");
//        if (Top10Scores[6] < defaultScore7)
//        {
//            Top10Scores[6] = defaultScore7;
//        }
//        Top10Scores[7] = PlayerPrefs.GetFloat("TopScore8");
//        if (Top10Scores[7] < defaultScore8)
//        {
//            Top10Scores[7] = defaultScore8;
//        }
//        Top10Scores[8] = PlayerPrefs.GetFloat("TopScore9");
//        if (Top10Scores[8] < defaultScore9)
//        {
//            Top10Scores[8] = defaultScore9;
//        }
//        Top10Scores[9] = PlayerPrefs.GetFloat("TopScore10");
//        if (Top10Scores[9] < defaultScore10)
//        {
//            Top10Scores[9] = defaultScore10;
//        }



        //emitterSide1.SetActive(false);
        //emitterSide2.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        highScoreText.text = topScore.ToString();

        if (menuRef == false)
        {
            if (wallKillsPlayer == true)
            {
                //    wallWarnings.SetActive(true);
                fogFX.SetActive(true);
            }
            if (wallKillsPlayer == false)
            {
                //    wallWarnings.SetActive(false);
                fogFX.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();

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

            if (playerScore > baseHighScore)
            {
                topScore = playerScore;
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
//        if (playerScore > Top10Scores[0])
//        {
//            Debug.Log("Score 1 beaten");
//            Top10Scores[9] = Top10Scores[8];
//            Top10Scores[8] = Top10Scores[7];
//            Top10Scores[7] = Top10Scores[6];
//            Top10Scores[6] = Top10Scores[5];
//            Top10Scores[5] = Top10Scores[4];
//            Top10Scores[4] = Top10Scores[3];
//            Top10Scores[3] = Top10Scores[2];
//            Top10Scores[2] = Top10Scores[1];
//            Top10Scores[1] = Top10Scores[0];
//
//            playerScore = Top10Scores[0];
//            newHighScore.SetActive(true);
//
//
//        }
//        if (playerScore > Top10Scores[1] && playerScore <= Top10Scores[0])
//        {
//            Debug.Log("Score 2 beaten");
//
//            Top10Scores[9] = Top10Scores[8];
//            Top10Scores[8] = Top10Scores[7];
//            Top10Scores[7] = Top10Scores[6];
//            Top10Scores[6] = Top10Scores[5];
//            Top10Scores[5] = Top10Scores[4];
//            Top10Scores[4] = Top10Scores[3];
//            Top10Scores[3] = Top10Scores[2];
//            Top10Scores[2] = Top10Scores[1];
//            playerScore = Top10Scores[1];
//            newHighScore.SetActive(true);
//
//
//        }
//        if (playerScore > Top10Scores[2] && playerScore <= Top10Scores[1])
//        {
//            Debug.Log("Score 3 beaten");
//
//            Top10Scores[9] = Top10Scores[8];
//            Top10Scores[8] = Top10Scores[7];
//            Top10Scores[7] = Top10Scores[6];
//            Top10Scores[6] = Top10Scores[5];
//            Top10Scores[5] = Top10Scores[4];
//            Top10Scores[4] = Top10Scores[3];
//            Top10Scores[3] = Top10Scores[2];
//            
//            playerScore = Top10Scores[2];
//            newHighScore.SetActive(true);
//
//
//        }
//        if (playerScore > Top10Scores[3] && playerScore <= Top10Scores[2])
//        {
//            Debug.Log("Score 4 beaten");
//
//            Top10Scores[9] = Top10Scores[8];
//            Top10Scores[8] = Top10Scores[7];
//            Top10Scores[7] = Top10Scores[6];
//            Top10Scores[6] = Top10Scores[5];
//            Top10Scores[5] = Top10Scores[4];
//            Top10Scores[4] = Top10Scores[3];
//            
//            playerScore = Top10Scores[3];
//            newHighScore.SetActive(true);
//
//
//        }
//        if (playerScore > Top10Scores[4] && playerScore <= Top10Scores[3])
//        {
//            Debug.Log("Score 5 beaten");
//
//            Top10Scores[9] = Top10Scores[8];
//            Top10Scores[8] = Top10Scores[7];
//            Top10Scores[7] = Top10Scores[6];
//            Top10Scores[6] = Top10Scores[5];
//            Top10Scores[5] = Top10Scores[4];
//            
//            playerScore = Top10Scores[4];
//            newHighScore.SetActive(true);
//
//
//        }
//        if (playerScore > Top10Scores[5] && playerScore <= Top10Scores[4])
//        {
//            Debug.Log("Score 6 beaten");
//
//            Top10Scores[9] = Top10Scores[8];
//            Top10Scores[8] = Top10Scores[7];
//            Top10Scores[7] = Top10Scores[6];
//            Top10Scores[6] = Top10Scores[5];
//            
//            playerScore = Top10Scores[5];
//            newHighScore.SetActive(true);
//
//
//        }
//        if (playerScore > Top10Scores[6] && playerScore <= Top10Scores[5])
//        {
//            Debug.Log("Score 7 beaten");
//
//            Top10Scores[9] = Top10Scores[8];
//            Top10Scores[8] = Top10Scores[7];
//            Top10Scores[7] = Top10Scores[6];
//            
//            playerScore = Top10Scores[6];
//            newHighScore.SetActive(true);
//
//
//        }
//        if (playerScore > Top10Scores[7] && playerScore <= Top10Scores[6])
//        {
//            Debug.Log("Score 8 beaten");
//
//            Top10Scores[9] = Top10Scores[8];
//            Top10Scores[8] = Top10Scores[7];
//            
//            playerScore = Top10Scores[7];
//            newHighScore.SetActive(true);
//
//
//        }
//        if (playerScore > Top10Scores[8] && playerScore <= Top10Scores[7])
//        {
//            Debug.Log("Score 9 beaten");
//
//            Top10Scores[9] = Top10Scores[8];
//            playerScore = Top10Scores[8];
//            newHighScore.SetActive(true);
//
//
//        }
//        if (playerScore > Top10Scores[9] && playerScore <= Top10Scores[8])
//        {
//            Debug.Log("Score 10 beaten");
//
//            playerScore = Top10Scores[9];
//            newHighScore.SetActive(true);
//
//
//        }

//        if (menuRef == true)
//        {
//            score1Text.text = Top10Scores[0].ToString();
//            score2Text.text = Top10Scores[1].ToString();
//            score3Text.text = Top10Scores[2].ToString();
//            score4Text.text = Top10Scores[3].ToString();
//            score5Text.text = Top10Scores[4].ToString();
//            score6Text.text = Top10Scores[5].ToString();
//            score7Text.text = Top10Scores[6].ToString();
//            score8Text.text = Top10Scores[7].ToString();
//            score9Text.text = Top10Scores[8].ToString();
//            score10Text.text = Top10Scores[9].ToString();
//
//        }


    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "DarkObstacle" && lightMode == true)
        {
            PlayerPrefs.SetFloat("High Score", topScore);
            PlayerPrefs.Save();

            //Debug.Log("Player died");
            gameOverText.SetActive(true);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "LightObstacle" && darkMode == true)
        {
            PlayerPrefs.SetFloat("High Score", topScore);
            PlayerPrefs.Save();

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
            gameOverText.SetActive(true);
            PlayerPrefs.SetFloat("High Score", topScore);
            PlayerPrefs.Save();

            Destroy(this.gameObject);
        }

    }

    public void OnDestroy()
    {
//        Debug.Log(Top10Scores[0]);
//        Debug.Log(Top10Scores[1]);
//        Debug.Log(Top10Scores[2]);
//        Debug.Log(Top10Scores[3]);
//        Debug.Log(Top10Scores[4]);
//        Debug.Log(Top10Scores[5]);
//        Debug.Log(Top10Scores[6]);
//        Debug.Log(Top10Scores[7]);
//        Debug.Log(Top10Scores[8]);
//        Debug.Log(Top10Scores[9]);


        //mainCamera.GetComponent<AudioSource>().enabled = false;
        //PlayerPrefs.SetFloat("High Score", topScore);
        //PlayerPrefs.Save();
//        PlayerPrefs.SetFloat("TopScore1", Top10Scores[0]);
//        PlayerPrefs.SetFloat("TopScore2", Top10Scores[1]);
//        PlayerPrefs.SetFloat("TopScore3", Top10Scores[2]);
//        PlayerPrefs.SetFloat("TopScore4", Top10Scores[3]);
//        PlayerPrefs.SetFloat("TopScore5", Top10Scores[4]);
//        PlayerPrefs.SetFloat("TopScore6", Top10Scores[5]);
//        PlayerPrefs.SetFloat("TopScore7", Top10Scores[6]);
//        PlayerPrefs.SetFloat("TopScore8", Top10Scores[7]);
//        PlayerPrefs.SetFloat("TopScore9", Top10Scores[8]);
//        PlayerPrefs.SetFloat("TopScore10", Top10Scores[9]);
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
//        baseHighScore = 100;
//
//        topScore = 100;
//
//
//        PlayerPrefs.SetFloat("High Score", defaultScore1);
//        PlayerPrefs.SetFloat("TopScore1", defaultScore1);
//        PlayerPrefs.SetFloat("TopScore2", defaultScore2);
//        PlayerPrefs.SetFloat("TopScore3", defaultScore3);
//        PlayerPrefs.SetFloat("TopScore4", defaultScore4);
//        PlayerPrefs.SetFloat("TopScore5", defaultScore5);
//        PlayerPrefs.SetFloat("TopScore6", defaultScore6);
//        PlayerPrefs.SetFloat("TopScore7", defaultScore7);
//        PlayerPrefs.SetFloat("TopScore8", defaultScore8);
//        PlayerPrefs.SetFloat("TopScore9", defaultScore9);
//        PlayerPrefs.SetFloat("TopScore10", defaultScore10);

    }

    public void FogMode()
    {
        gameModeInt = 1;
        PlayerPrefs.SetInt("Game Mode Int", 1);
    }
    public void WallMode()
    {
        gameModeInt = 2;
        PlayerPrefs.SetInt("Game Mode Int", 2);
    }

    public void LoadGameMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadGameLevel()
    {
        SceneManager.LoadScene(1);
    }

	public void QuitGame(){
		Application.Quit();

	}

}
