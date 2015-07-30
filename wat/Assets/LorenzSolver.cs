using UnityEngine;
using System.Collections;

public class LorenzSolver : MonoBehaviour {

	float xn,yn,zn;
	[Range(0f, 30f)]
	public float pr = 1f;
	[Range(0f, 30f)]
	public float r = 0.7f;
	[Range(0f, 30f)]
	public float b = 1.2f;
	// Use this for initialization
	void Start () {
		xn = 5.1f;
		yn = 12f;
		zn = -0.5f;
	}

	public float iterate(float dt)
	{
		float xnn = xn + (pr * (yn - xn)) * dt;
		float ynn = xn*yn + (-xn * zn + r * xn - yn) * dt;
		float znn = zn + (xn * yn - b * zn) * dt;

		xn = xnn;
		yn = ynn;
		zn = znn;

		return xn * zn * yn;

		Debug.Log (xnn);	
		this.transform.position = new Vector3 (xnn, ynn, znn);
		//GameObject particle = Resources.Load ("particle") as GameObject;
		//Instantiate (particle, this.transform.position, Random.rotation);
		Camera.main.transform.LookAt (this.transform.position);
	}

	// Update is called once per frame
	void Update () {
		iterate (0.02f);
	}
}
