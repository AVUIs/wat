using UnityEngine;
using System.Collections;

public class Brotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (0.1f, 0f, 0f);
	}
}
