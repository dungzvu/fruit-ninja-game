#pragma strict

//draw slice Mesh


var mesh:Mesh;

var vert:		Array		=new Array();//chua toa do diem
var vertLife:	Array		=new Array();
var timeLife:	int			=10;

var maxW:float=1.0;
private var stepW:float;
var mat:Material;

//*****************4*************************************************

function UpdateMesh(){
	var r:float;
	var angle:float;
	//tinh step Width;
	stepW=maxW/(vert.length-1);
	//co length-2 tam giac
	var vert_fix:Vector3[]=new Vector3[vert.length*2-1];
	var triangles:int[]=new int[3*(vert.length*2-1)];
	
	vert_fix[0]=vert[0];
	var n : int=vert.length;
		for(var i=1;i<n;i++){
			r=dist(vert[i],vert[i-1]);
			angle=calcAngle(r,i*stepW);
			
			vert_fix[2*i-1]=rotVert(vert[i-1],vert[i],angle);
			vert_fix[2*i]=rotVert(vert[i-1],vert[i],-angle);
				
		}
	n=n*2-1-2;
	for(var j=0;j<n;j++){
		if(j%2==0){
			triangles[3*j]=j;
			triangles[3*j+1]=j+1;
			triangles[3*j+2]=j+2;
		}else{
			triangles[3*j]=j+1;
			triangles[3*j+1]=j;
			triangles[3*j+2]=j+2;
		}
	}
	
	mesh.vertices=vert_fix;
	mesh.triangles=triangles;
	
}
/*
function OnPostRender(){
	var r:float;
	var angle:float;
	//tinh step Width;
	stepW=maxW/(vert.length-1);
	//ve doan thang can 2 diem tro len
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
		GL.End();
	}
}

*/

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

function Start () {
	mesh=GetComponent(MeshFilter).mesh;
}

function Update () {
	//Kiem tra xem co phai dang cat k
	if(Input.GetMouseButton(0)){
		var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
		var pos=ray.GetPoint(10);
		vert.Push(pos);
		vertLife.Push(timeLife);
	}
	
	LineManager();
	
	if(vert.length>0)	UpdateMesh();
}

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
	
}

