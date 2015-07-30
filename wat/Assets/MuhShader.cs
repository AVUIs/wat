using UnityEngine;
using System.Collections;
using LibPDBinding;

public class MuhShader : MonoBehaviour {
	public Texture2D tex;
	public Texture2D koa;
	int mipCount;

	public waveguideController wgController;

	public float[] wat;
	public float[] koalaMatrix;

	//public int w= 180;
	//public int h= 90;

	float[] buffer;
	float[] mainBuffer;

	bool ready = false;
	public static Color HSVToRGB(float H, float S, float V)
	{
		if (S == 0f)
			return new Color(V,V,V);
		else if (V == 0f)
			return  Color.black;
		else
		{
			Color col = Color.black;
			float Hval = H * 6f;
			int sel = Mathf.FloorToInt(Hval);
			float mod = Hval - sel;
			float v1 = V * (1f - S);
			float v2 = V * (1f - S * mod);
			float v3 = V * (1f - S * (1f - mod));
			switch (sel + 1)
			{
			case 0:
				col.r = V;
				col.g = v1;
				col.b = v2;
				break;
			case 1:
				col.r = V;
				col.g = v3;
				col.b = v1;
				break;
			case 2:
				col.r = v2;
				col.g = V;
				col.b = v1;
				break;
			case 3:
				col.r = v1;
				col.g = V;
				col.b = v3;
				break;
			case 4:
				col.r = v1;
				col.g = v2;
				col.b = V;
				break;
			case 5:
				col.r = v3;
				col.g = v1;
				col.b = V;
				break;
			case 6:
				col.r = V;
				col.g = v1;
				col.b = v2;
				break;
			case 7:
				col.r = V;
				col.g = v3;
				col.b = v1;
				break;
			}
			col.r = Mathf.Clamp(col.r, 0f, 1f);
			col.g = Mathf.Clamp(col.g, 0f, 1f);
			col.b = Mathf.Clamp(col.b, 0f, 1f);
			return col;
		}
	}
	
	void Start() {

		//Screen.showCursor = false;

		Cursor.visible = false;

		wgController = Camera.main.GetComponent<waveguideController> ();

		Renderer rend = GetComponent<Renderer>();
		
		// duplicate the original texture and assign to the material
		tex =rend.material.mainTexture as Texture2D;
		tex.Resize (200, 100);
		//tex.Resize (100, 50);
		//tex.Resize (w, h);
		rend.material.mainTexture = tex;

		//koa =rend.material.mainTexture as Texture2D;
		koa.Resize (200, 100);
		//koa.Resize (100, 50);
		//koa.Resize (w, h);
		//rend.material.mainTexture = koa;
		
		// colors used to tint the first 3 mip levels
		Color[] colors = new Color[3];
		colors[0] = Color.red;
		colors[1] = Color.green;
		colors[2] = Color.blue;
		mipCount = Mathf.Min(3, tex.mipmapCount);
		
		// tint each mip level
		/*
		for( var mip = 0; mip < mipCount; ++mip ) {
			var cols = tex.GetPixels( mip );
			for( var i = 0; i < cols.Length; ++i ) {
				cols[i] = new Color(Random.value,Random.value,Random.value);
			}
			tex.SetPixels( cols, mip );
		}*/

		// actually apply all SetPixels, don't recalculate mip levels
		//tex.Apply(false);
		int length = tex.GetPixels ().Length;
		wat = new float[length];  // 
		for (int i =0; i< wat.Length; i++) 
			wat [i] = Random.value;	
	

		int koaLength = koa.GetPixels ().Length;
 
		koalaMatrix = new float[koaLength];
			
		Color [] graypix = koa.GetPixels ();

		for (int i =0; i< koaLength; i++) {
			koalaMatrix [i] = graypix[i].grayscale;

		}

		//print (graypix.Length);



		Debug.Log ("n minmaps: " + mipCount);
		solver = new LorenzSolver ();
		//for(int i =0; i< wa

		this.GetComponent<AudioSource> ().clip.GetData (buffer, 0);
		//decimate stereo to mono
		int count = 0;;
		mainBuffer = new float[buffer.Length / 2]; 
		for (int i =0; i<buffer.Length; i+=2) {
			mainBuffer[count] = buffer[i];
			count++;
		}

		bufRead = 0;
		ready = true;
	}
	
	public int g2d(int x, int y)
	{
		return  x + y * 200;
		//return  x + y * 100;
		//return  x + y * w;
	}
	
	int state=-1;

	void reset()
	{
		for (int i =0; i< wat.Length; i++)
			wat [i] = Random.value;
			//wat [i] = koa [i];//.value;
		driver = 1000.0f;
	}

	int prevState = -1;

	float driver = 1000.0f;

	int colorSet = 2;
	float curve = 0f;
	LorenzSolver solver;
	float dt=0f;



	void Update()
	{

		Cursor.visible = false;

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			state = 0;

		}
		
		if (Input.GetKeyDown (KeyCode.Alpha2)){
			state = 1;

		}
		if (Input.GetKeyDown (KeyCode.Alpha3)){
			state = 2;

		}
		if (Input.GetKeyDown (KeyCode.Alpha4)){
			state = 3;

		}
		if (Input.GetKeyDown (KeyCode.Alpha5)){
			state = 4;

		}
		if (Input.GetKeyDown (KeyCode.Alpha6)){
			state = 5;

		}
		if (Input.GetKeyDown (KeyCode.Alpha7)){
			state = 6;

		}
		if (Input.GetKeyDown (KeyCode.Alpha8)){
			state = 7;

		}
		if (Input.GetKeyDown (KeyCode.Alpha9)){
			state = 8;
		
		}

		if (Input.GetKeyDown (KeyCode.Alpha0)){
			state = 9;
			
		}

		if (Input.GetKeyDown (KeyCode.Minus)) {
			state=-1;
		}


		if (Input.GetKeyDown (KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Mouse1)){
			state = 10;
			
		}

		if (Input.GetKeyDown (KeyCode.Return)) {
			reset ();
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			colorSet=0;
		}
		
		if (Input.GetKeyDown (KeyCode.S)) {
			colorSet=1;
		}	
		
		if (Input.GetKeyDown (KeyCode.D)) {
			colorSet=2;
		}
		
		if (Input.GetKeyDown (KeyCode.F)) {
			colorSet=3;
		}
		
		if (Input.GetKeyDown (KeyCode.G)) {
			colorSet=4;
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			colorSet=5;
		}
		if (Input.GetKeyDown (KeyCode.J)) {
			colorSet=6;
		}


		driver += 0.01f;

		Debug.Log ("STATE: " + state);
		Color[] pix = tex.GetPixels ();
		Color[] pixKoa = koa.GetPixels ();


		for (int x = 2; x < tex.width-2; x++) {
			// Loop through every pixel row
			for (int y = 2; y < tex.height-2; y++) {
				
				float mouseX = Input.mousePosition.x;
				float mouseY = Input.mousePosition.y;

				float startMean = 0.0f;
				Vector3 hsl = new Vector3(1.0f,1.0f,0.7f);

				if(state==0)
				{

					wat[g2d(x,y)] =  Mathf.Abs (Mathf.Sin (((wat[g2d (x,y)]) +
					                             (wat[g2d (x-1,y)]) +
					                             (wat[g2d (x,y-1)]) +
					                             (wat[g2d (x,y+1)])) + (driver)));
				}
				
				if(state==1)
				{
					wat[g2d(x,y)] =  Mathf.Abs (Mathf.Cos (((wat[g2d (x,y)]) +
					                             (wat[g2d (x-1,y)]) +
					                             (wat[g2d (x,y+1)]))*Mathf.Sin (driver*0.01f))*Mathf.Cos (((wat[g2d (x,y)]) +
					                                               (wat[g2d (x+1,y+1)]) +
					                                               (wat[g2d (x,y+1)]))*Mathf.Cos (driver)));
				}
				
				if(state==2)
				{
					wat[g2d(x,y)] =  Mathf.Tan (((wat[g2d (x+1,y-1)]) + (float) x+
					                             (wat[g2d (x-1,y)]) +
					                             (wat[g2d (x,y+1)]))*Mathf.Cos (driver));
				}

				if(state==3)
				{
					wat[g2d(x,y)] =  Mathf.Cos (((wat[g2d (x+1,y-1)]) +
					                              (wat[g2d (x-1,y-1)]) +
					                              (wat[g2d (x+1,y+1)]) +
					                             (wat[g2d (x,y+1)]))*Mathf.Cos (driver));
				}
				if(state==4)
				{
					wat[g2d(x,y)] =  Mathf.Sin (((wat[g2d (x,y)]) + (wat[g2d (x-1,y)])*0.2f +
					                             (wat[g2d (x,y+1)])*2f));
				}
				if(state==5)
				{
					wat[g2d(x,y)] =  Mathf.Cos (Mathf.Sin((wat[g2d (x+1,y-1)]) + (float) x+
					                             (wat[g2d (x-1,y)]) +
					                             (wat[g2d (x,y+1)]))*Mathf.Cos (driver));
					
				}

				if(state==6)
				{
					
					wat[g2d(x,y)] =  Mathf.Abs (Mathf.Sin (((wat[g2d (x,y)]) +
					                                        (wat[g2d (x-2,y)]) +
					                                        (wat[g2d (x,y-2)]) +
					                                        (wat[g2d (x,y+2)])) + (driver)));
				}
				if(state==7)
				{
					
					wat[g2d(x,y)] =  Mathf.Abs (Mathf.Cos (( 
					                                        (wat[g2d (x-1,y)]) +
					                                        (wat[g2d (x,y+1)]))*Mathf.Sin (driver*0.04f))*(((wat[g2d (x,y)]) +
					                                                          (wat[g2d (x+1,y+1)]) +
					                                                          (wat[g2d (x,y+1)]))*Mathf.Cos (driver)));
				}
				if(state==8)
				{
					
					wat[g2d(x,y)] =  Mathf.Tan( (Mathf.Cos (( 
					                                        (wat[g2d (x-1,y)])*0.01f +
					                                        (wat[g2d (x-2,y)])*0.05f +
					                                        (wat[g2d (x,y+1)])*0.2f +
					                                        (wat[g2d (x-1,y+2)])*0.001f +
					                                        (wat[g2d (x+1,y+1)])*0.3f +
					                                        (wat[g2d (x-1,y-2)])*0.2f +
					                                        (wat[g2d (x-2,y-1)])*0.1f +
					                                        (wat[g2d (x,y-2)])*0.2f))));
				}

				if(state==9)
				{
				

					float val = wat[g2d(x,y)];
					if(val<0.5f)
					{
						wat[g2d(x,y)] =  Mathf.Tan( (Mathf.Cos (( 
						                                         (wat[g2d (x-1,y)])*0.01f +
						                                         (wat[g2d (x-2,y)])*0.05f +
						                                         (wat[g2d (x,y+1)])*0.1f +
						                                         (wat[g2d (x-1,y+2)])*0.001f +
						                                         (wat[g2d (x+1,y+1)])*0.1f +
						                                         (wat[g2d (x-1,y-2)])*0.1f +
						                                         (wat[g2d (x-2,y-1)])*0.1f +
						                                         (wat[g2d (x,y-2)])*0.1f))));
					}
					else
					{
						wat[g2d(x,y)] =  Mathf.Sin ((float)(x%10) * (float)(y%5));
					}
				}

				if(state ==10)
				{
				}

				if(state==-1)
				{
					wat[g2d(x,y)]=0f;
				}


				float watput = wat[g2d (x,y)];

				//pix[g2d (x,y)] = HSVToRGB(watput,watput*4f,1f);
				//pixKoa[g2d (x,y)] = koa.GetPixel(x,y);//(Color.red);
				
				//if (colorSet==0) {
					//pix[g2d (x,y)] = HSVToRGB(watput,watput*4f,1f);
				//}



				if (colorSet==0) {
					pix[g2d (x,y)] = HSVToRGB(watput,watput*4f,1f);
				}

				
				else if (colorSet==1) {
					pix[g2d(x,y)] =  (Color.Lerp(Color.black,Color.red,watput*watput));//, Color.Lerp(Color.green,Color.red,Mathf.Cos (watput)),Mathf.Sin(watput));
					pix[g2d(x,y)] = Color.Lerp (pix[g2d(x,y)], Color.green,watput*watput*watput);
					pix[g2d(x,y)] = pix[g2d(x,y)] *pix[g2d(x,y)] ;
				}
				
				else if (colorSet==2){
					pix[g2d (x,y)] = HSVToRGB(watput,1f,watput);
				}
				else if (colorSet==3){
					pix[g2d (x,y)] = HSVToRGB(watput*2f,watput*4f,1f);
				}
				else if (colorSet==4){
					//nice bw + colors
					pix[g2d (x,y)] = HSVToRGB(watput*2f,watput*2f,1f);
				}
				else if (colorSet==5){
					//dgpix[g2d (x,y)] = HSVToRGB(watput%255,watput%255,watput%255);
					pix[g2d (x,y)] = HSVToRGB(watput*8f,watput*2f,1f);
				}
				else if (colorSet==6){
					//dgpix[g2d (x,y)] = HSVToRGB(watput%255,watput%255,watput%255);
					pix[g2d (x,y)] = HSVToRGB(watput,watput*2f,1f);
				}
				else {
					
				}

			}

		}
					
		LibPD.SendFloat("colormode",colorSet);

		tex.SetPixels( pix);
		//tex.SetPixels( pixKoa);
		//tex.SetPixels( koa.GetPixels());
		
		// actually apply all SetPixels, don't recalculate mip levels
		tex.Apply(false);

		// audio
		wgController.floatArrayToPD (wat);

	}

	float bufRead;
	void OnAudioFilterRead(float[] data, int channels) {
	//	Debug.Log (ready);
		//if (ready) {
			for (int i = 0; i < data.Length; i+=channels) {

				//bufRead+=1f;//wat[i+10000]/100f;

				//if(bufRead==mainBuffer.Length)
				//	bufRead=0;
				//for (int j=0; j<50; j++) {
				//	data [i] += Mathf.Cos (2.0f * Mathf.PI + wat [j + i] * (float)j);
				//}
				//data [i] = data [i] / 50.0f;
				//data[i] = wat[i+10000];	//buffer[ (int)Mathf.Clamp((int)wat[i] * 1000.0f,0f,buffer.Length) ];
			//data[i]=wat[i+900	];
			//data[i+1]=wat[i+3000	];//mainBuffer[(int)bufRead];
			}
		//}
	}

}