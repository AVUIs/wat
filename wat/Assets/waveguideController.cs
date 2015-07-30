using UnityEngine;
using System.Collections;
using LibPDBinding;

public class waveguideController : MonoBehaviour {

	float[] lastArray;

	// Use this for initialization
	void Start () {
	
		lastArray = new float[20000];

		Cursor.visible = false;
		//Screen.lockCursor = true;

	}
	
	// Update is called once per frame
	void Update () {

		//floatArrayToPD ( new float[6]{0, 1, 10, 1, 57, 1} );

		Cursor.visible = false;
		//Screen.lockCursor = true;
	}

	public void floatArrayToPD(float[] arrayInput)	{
		//LibPD.SendList ("testing", 10,1);

		float[] array = new float[1024 * 2];



		for(int i=0;i<1024;i++){//i++)	{

			if(i%2 == 0)	{
				
				int idx = i/2;

				array[idx*2] = idx;

				array[idx*2+1] = (Mathf.Cos(arrayInput[idx*20000/1024]));// - Mathf.Cos(1-lastArray[idx*20000/1024]);
			}
		}

		System.Array.Copy (arrayInput, lastArray, 1024);//arrayInput.Length);



		object[] objectArray = new object[2048];
		System.Array.Copy (array, objectArray, 2048);//array.Length);



		//print (objectArray.Length);//array[6] + " " + array[7]);

		LibPD.SendList ("params", objectArray);

	}
}
