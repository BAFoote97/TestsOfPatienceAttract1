using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform player;

    public float altitude;

    public float panSpeed;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //gameObject.transform.position = new Vector3(player.position.x, altitude, player.position.z);


//        gameObject.transform.position = new Vector3(gameObject.transform.position.x, altitude, gameObject.transform.position.z);


//        if (player.position.x > gameObject.transform.position.x)
//        {
//            xPanSpeed += gameObject.transform.position.x * Time.deltaTime;
//            //gameObject.position.x += panSpeed * Time.deltaTime;
//            transform.Translate(xPanSpeed, gameObject.transform.position.y, gameObject.transform.position.z);
//        }
//        if (player.position.x < gameObject.transform.position.x)
//        {
//            xPanSpeed -= gameObject.transform.position.x * Time.deltaTime;
//            //gameObject.position.x += panSpeed * Time.deltaTime;
//            transform.Translate(xPanSpeed, gameObject.transform.position.y, gameObject.transform.position.z);
//        }
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, panSpeed *Time.deltaTime);
        transform.position = smoothedPosition;
    }
    public void RestartStage()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
