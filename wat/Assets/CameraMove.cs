using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	Vector3 newNoise;
	Vector3 camOrigin;
	Vector3 camTranslation;
	Vector3 forwardMovement;
	Vector3 desiredForwardMovement;
	// Use this for initialization
	void Start () {
		newNoise = new Vector3 (Random.value, Random.value, Random.value);
		camOrigin = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
		camTranslation = new Vector3 (0f, 0f, 0f);
		forwardMovement = new Vector3 (0f, 0f, 0f);
		desiredForwardMovement = new Vector3 (0f, 0f, 0f);
		Cursor.visible = false;
		Screen.lockCursor = true;
	}
	int counter =0; 
	// Update is called once per frame
	void Update () {

		//desiredForwardMovement = new Vector3 (Input.mousePosition.x/10, 0f, Input.mousePosition.y);

		desiredForwardMovement += new Vector3 (Input.GetAxis("Mouse X")*10, 0f, Input.GetAxis("Mouse Y")*5);
		//print(Input.GetAxis("Mouse X"));
		forwardMovement = Vector3.Lerp (forwardMovement, desiredForwardMovement, 0.05f);
		//Vector3 sideMovement = new Vector3 (Input.mousePosition.x, 0, 0);
		//float sideMovement = Input.mousePosition.x / Camera.main.pixelWidth;

		//sideMovement = sideMovement * 2.0f - 1.0f;

		counter++;
		if (counter == 5) {
			newNoise = new Vector3 (Random.value-0.5f, Random.value-0.5f, Random.value-0.5f);
			
			counter = 0;
		}
		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			//sideMovement=
		}
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			//transform.Rotate(Vector3.down * Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.R))	{
			this.forwardMovement = Vector3.zero;
		}


		camTranslation = Vector3.Lerp(camTranslation,newNoise,0.01f);
		this.transform.position = camOrigin + camTranslation*200.0f+forwardMovement;

	}
}
