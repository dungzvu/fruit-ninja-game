#pragma strict

//draw slice use GL

var vert:		Array		=new Array();//chua toa do diem
var vertLife:	Array		=new Array();
var timeLife:	int			=5;		//thoi gian tu luc init den luc destroy cua mot diem tren slice

var maxW:float=1.0;
private var stepW:float;

var mat:Material;


var hitObjects:RaycastHit[];
var countHitObjects:int=0;
var direction:Vector3;

var maxLayer:int;					//dua vao script me(scriptClassic)
var layerStep:float=5;

//sound
var splatter1:AudioClip;
var splatter2:AudioClip;
//*****************4*************************************************

function Start () {
	
}

function sliceUpdate () {
	//Kiem tra xem co phai dang cat k
	
	if(PlayerPrefs.GetInt("isPause")==0){//Khi pause game thi khong cap nhat them diem nua
	
		if(Input.GetMouseButton(0)){
			var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
			var pos=ray.GetPoint(10);
			vert.Push(pos);
			vertLife.Push(timeLife);
		}
	}
	
	//Debug.Log("update");
	LineManager();
	
	checkCollider();
}


function sliceDraw(){
	var r:float;
	var angle:float;
	
		
		//tinh step Width;
		stepW=maxW/(vert.length-1);
		//ve doan thang can 2 diem tro len
		//Debug.Log(mat.name);
		if(vert.length>=2){
			mat.SetPass(0);
			GL.Begin(GL.TRIANGLE_STRIP);
					GL.TexCoord2(0,0.5);	GL.Vertex(vert[0]);
				var n:int=vert.length;
				for(var i=1;i<n;i++){
					r=dist(vert[i],vert[i-1]);
					angle=calcAngle(r,i*stepW);
					
					GL.TexCoord2(i/(n-1),1);	GL.Vertex(rotVert(vert[i-1],vert[i],angle));
					GL.TexCoord2(i/(n-1),0);	GL.Vertex(rotVert(vert[i-1],vert[i],-angle));
					
				}
					//ve dau nhon
					var v1:Vector3=vert[n-1];
					var v2:Vector3=vert[n-2];
					var endVert:Vector3=(v1-v2)/dist(v1,v2)+v1;
					GL.TexCoord2(1,0.5);	GL.Vertex(endVert);
			GL.End();
		}
		
	
}


///////////////////////////////////////////////////////////////////////////////////////
//ham tinh toan de ve duong slice

function calcAngle(r:float,w:float):float{
	var angle:float=w/(2*r);
	return angle;
}

function dist(v1:Vector3,v2:Vector3):float{
	var d:float;
	d=Mathf.Sqrt((v1.x-v2.x)*(v1.x-v2.x)+(v1.y-v2.y)*(v1.y-v2.y));
	return d;
}

function rotVert(v0:Vector3,v:Vector3,angle:float):Vector3{
	var tg:Vector3;
	
	v.x-=v0.x;
	v.y-=v0.y;
	tg.x=v.x*Mathf.Cos(angle)-v.y*Mathf.Sin(angle)+v0.x;
	tg.y=v.x*Mathf.Sin(angle)+v.y*Mathf.Cos(angle)+v0.y;
	tg.z=0;
	return tg;
}


//ham update thay doi slice

function LineManager(){
	var gt:int;
	//var v:int[]=vertLife.int;
	if(vertLife.length>0){
		for(var i=0; i<vertLife.length;i++){
			gt=vertLife[i];
			gt-=1;
			vertLife[i]=gt;
			//vertLife[i].int--;
		}
		gt=vertLife[0];
		if(gt<=0){
			//destroy
			vert.Shift();
			vertLife.Shift();
		}
	}
	/*
	if(PlayerPrefs.GetInt("isPause")!=0){
		vert.Clear();
		vertLife.Clear();
	}
	*/
}

//ham kiem tra va cham vs fruit va bomb
//update vao hitObjects array

function checkCollider(){
	var hits:RaycastHit[]=new RaycastHit[maxLayer];
	var hitMem:RaycastHit;
	var dir:Vector3;//direction
	var dist:float;
	
	var cout:int=-1;//de kiem tra co va cham k, neu =-1 thi chua vc
	
	
	if(vert.length>1){
	
	
		var origin:Vector3=vert[vert.length-2];
		var target:Vector3=vert[vert.length-1];
		
		dir=target-origin;
		
		dist=Mathf.Sqrt(dir.x*dir.x+dir.y*dir.y+dir.z*dir.z);
		
		dir/=dist;			//normalize
		
		//lay toan bo va cham
		//kiem tra tung layer, co maxlayer
		for(var i=0;i<maxLayer;i++){
			origin.z=i*layerStep;
			if(Physics.Raycast(origin,dir,hitMem,dist)){
				cout++;
				hits[cout]=hitMem;	
			}
		}
		//hits=Physics.Raycast(origin,dir,dist);
		
		//reset hitObjects
		countHitObjects=cout+1;
		//kiem tra va cham
		if(cout>=0){
			//va cham
			Debug.Log("cham" + hits.Length);
			GetComponent.<AudioSource>().PlayOneShot(splatter1);
			if(countHitObjects>=3) GetComponent.<AudioSource>().PlayOneShot(splatter2);
			
			hitObjects=hits;
			
			//tinh direction mp 0xy
			var angle:float=Mathf.Atan2(dir.y,dir.x);
			direction=Vector3(0,0,angle*180/Mathf.PI);
			//Debug.Log("direcrion= "+direction);
		}
		
	}
	
}



