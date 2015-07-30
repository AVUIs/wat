using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LibPDBinding;

public class UniCornerTriUp : MonoBehaviour {


	public float watfactor = 0.78f;

	MuhShader mh;

	[HideInInspector]
	public List<Vector3> triangleVertices;
	
	[HideInInspector]

	public List<int> triangles;
	
	[HideInInspector]

	public List<Vector2> UV; // texture  modifies this

	[HideInInspector]
	public static float time; 

	public float depthScroll; 
	
	
	Mesh uniCornerMesh ;
	
	//public int w;
	//public int h;

	int numOfTrianglesinRow =150;//100;
	int numOfTrianglesinColumn =75;//50;

	int width = 256;
	int height = 128;
	
	float edgeLength = 5f;
	float triHeight = 5f;
	
	bool KOALA = false;
	int count = 0;
	
	// Use this for initialization
	void Start () {

		float[] z  ;
		
		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		
		uniCornerMesh = GetComponent<MeshFilter>().mesh;

		mh = GameObject.Find ("plane").GetComponent<MuhShader> ();

		depthScroll = 20.0f;


	}

	public int g2d(int x, int y)
	{
		return  x + y * 200;

	}

	// Update is called once per frame
	void Update () {



		// This first list contains every vertex of the mesh that we are going to render
		triangleVertices = new List<Vector3>();
		
		triangles = new List<int>();
		
		UV = new List<Vector2>();

		float[] wat = mh.wat;  
		float[] koalaMatrix = mh.koalaMatrix;

		Color[] clrs = mh.tex.GetPixels ();

		//float maxAmplitude = 10f;

		int timer = 50;


		if (Input.GetKeyDown (KeyCode.Mouse2)) {
			KOALA=true;
			count=timer;
			//print ("mouse = " +  KeyCode.Mouse2);
		}


		if (KOALA) {
			KOALA=!KOALA;
			//print ("KOALA!!! " + count);

		}

		count--;


	

		//float z = maxAmplitude * Mathf.Sin (Time.time * Mathf.PI * 2) / numOfTrianglesinRow;
			

		depthScroll = depthScroll + Input.mouseScrollDelta.y*3;

//		print (depthScroll);

		LibPD.SendFloat("depthScroll",depthScroll);

		for (int k =0; k<numOfTrianglesinColumn; k++){
			for (int i =0; i<numOfTrianglesinRow; i++){

				int wati = 200*i/numOfTrianglesinRow;
				//int wati = w*i/numOfSquaresinRow;
				int watk = 100*k/numOfTrianglesinColumn;
				//int watk = h*k/numOfSquaresinColumn;

				//float startZ = - koalaMatrix[g2d (i,k) ];
//				float startZ = clrs[g2d (i,k)].grayscale;
				float startZ = mh.tex.GetPixel(wati,watk).grayscale;

				float koalaZ = mh.koa.GetPixel(wati,watk).grayscale;
				//float startZ = wat[g2d (i,k)];
				//float z = wat[g2d (i,k)]; //slow?
				float totalZ = 1f;

				//float watZ = wat[g2d (i,k)]; //this doesn't make sense;

				if (count>0 && count>timer/2){

					totalZ = (startZ*count/timer + koalaZ*(timer-count)/timer )*0.5f;
	
				}

				else if (count>0 && count<=timer/2){//reverse

					totalZ = (startZ*(timer-count)/timer + koalaZ*count/timer )*0.5f;

				}

				else if (count <=0){

					count=0;
					totalZ = startZ;

				}
			

				totalZ = totalZ * depthScroll;

				triangleVertices.Add( new Vector3 ( i*edgeLength , k*triHeight , totalZ ));  // first corner
				triangleVertices.Add( new Vector3 ( i*edgeLength , k*triHeight + triHeight , totalZ+totalZ/30*(timer-count)/timer ));  //second corner
				triangleVertices.Add( new Vector3 ( i*edgeLength + edgeLength, k*triHeight + triHeight , totalZ  ));  //third corner
				triangleVertices.Add( new Vector3 ( i*edgeLength + edgeLength, k*triHeight , totalZ+totalZ/30*(timer-count)/timer));  //fourth  corner

				
				
				triangles.Add(4*k*numOfTrianglesinRow+4*i);
				triangles.Add(4*k*numOfTrianglesinRow+4*i+1);
				triangles.Add(4*k*numOfTrianglesinRow+4*i+2);
				
				triangles.Add(4*k*numOfTrianglesinRow+4*i);
				triangles.Add(4*k*numOfTrianglesinRow+4*i+2);
				triangles.Add(4*k*numOfTrianglesinRow+4*i+3);
				
			}			
		}


		int vertecesLength = triangleVertices.Count;
		Color[] colors = new Color[vertecesLength];
		Color colorT = Color.red;
		Color colorK = Color.yellow;


		clrs = mh.tex.GetPixels();


		if (count <= 0) {
			for (int i = 0; i < vertecesLength; i++) {
			colorT = clrs[i/3];
				colors[i]=colorT;
			}
			}
		else if (count>0){

			for (int k =0; k<numOfTrianglesinColumn; k++){
				for (int i =0; i<numOfTrianglesinRow; i++){
					
					int wati = 200*i/numOfTrianglesinRow;
	
					int watk = 100*k/numOfTrianglesinColumn;
	
					if (count%2 ==0){
					colorK = Color.Lerp(mh.koa.GetPixel(wati,watk),Color.yellow,  (float)(count)/timer);
					}
					else
					colorK = Color.Lerp(mh.koa.GetPixel(wati,watk),Color.cyan,  (float)(count)/timer);

					if (count>0 && count>timer/2){

						colorT = Color.Lerp(colorK , mh.tex.GetPixel(wati,watk),  (float)(count)/timer);
					//colorT = Color.Lerp(mh.koa.GetPixel(wati,watk),mh.tex.GetPixel(wati,watk),  (float)(count)/timer);
					}
					else if (count>0 && count<=timer/2){//reverse

						//Color colorB = Color.Lerp(mh.koa.GetPixel(wati,watk),Color.yellow,  (float)(timer-count)/timer);
						colorT = Color.Lerp( colorK , mh.tex.GetPixel(wati,watk),  (float)(timer-count)/timer);
					//colorT = Color.Lerp(mh.koa.GetPixel(wati,watk),mh.tex.GetPixel(wati,watk),  (float)(timer-count)/timer);

					}
					colors[8*k*numOfTrianglesinColumn+i*4] = colorT;
					colors[8*k*numOfTrianglesinColumn+i*4+1] = colorT;
					colors[8*k*numOfTrianglesinColumn+i*4+2] = colorT;
					colors[8*k*numOfTrianglesinColumn+i*4+3] = colorT;
				}
			}
		}


		uniCornerMesh.colors = colors;
	

		uniCornerMesh.Clear();
		uniCornerMesh.vertices = triangleVertices.ToArray();
		uniCornerMesh.triangles = triangles.ToArray();

		uniCornerMesh.Optimize();
		uniCornerMesh.RecalculateNormals();
		//this.transform.Rotate(0f,Random.value-0.5f,0f);
		
	}
}//end of class


