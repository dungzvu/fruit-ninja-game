using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Slice : MonoBehaviour
{
    //draw slice Mesh

    public delegate void SliceEvent(GameObject[] obj, Vector3 direction);

    public event SliceEvent OnSlice;

    private Mesh mesh;
    private LinkedList<Vector3> vert = new LinkedList<Vector3>();

    private float maxW = 0.72f;
    private float stepW;
    private int maxSlice = 16;

    private bool isEnable = true;

    private AudioSource audio;

    private int countForRemoveVert = 0;

    //*****************4*************************************************

    void UpdateMesh()
    {
        if (vert.Count <= 1)
        {
            mesh.triangles = null;
            mesh.vertices = null;
            return;
        }
        float r;
        float angle;
        //tinh step Width;
        stepW = maxW / (vert.Count - 1);
        //co length-2 tam giac
        int nVert = vert.Count;
        int nVert_fix = nVert * 2;
        int nTriangles = ((nVert - 2) * 2 + 2) * 3;
        Vector3[] vert_fix = new Vector3[nVert_fix];
        Vector2[] uv = new Vector2[nVert_fix];
        int[] triangles = new int[nTriangles];

        vert_fix[0] = vert.First.Value;
        uv[0] = new Vector2(0, 0.5f);

        int i = 0;
        Vector3 prev = vert_fix[0];
        float uv_x = 0.25f;
        float stepuv = 0.5f / (nVert - 2);//nVert>=2
        //Debug.Log("-----------------------");
        foreach (var v in vert)
        {
            //Debug.Log("vert " + v.ToString());
            if (i == 0)
            {
                i++;
                continue;
            }
            r = Vector3.Distance(v, prev);
            angle = calcAngle(r, i * stepW);

            vert_fix[2 * i - 1] = rotVert(prev, v, angle);
            vert_fix[2 * i] = rotVert(prev, v, -angle);
            uv[2 * i - 1] = new Vector2(uv_x, 0);
            uv[2 * i] = new Vector2(uv_x, 1);
            prev = v;
            i++;
            uv_x += stepuv;
        }

        //them dau slice
        Vector3 pos1 = vert.Last.Value;
        Vector3 pos2 = (nVert == 1) ? pos1 : vert.Last.Previous.Value;
        Vector3 dir = pos1 - pos2;
        float length = 0.78f;
        Vector3 head = dir * length / Vector3.Distance(pos1, pos2) + pos1;

        vert_fix[nVert_fix - 1] = head;
        uv[nVert_fix - 1] = new Vector2(1, 0.5f);

        for (var j = 0; j < nVert_fix - 2; j++)
        {
            if (j % 2 == 0)
            {
                triangles[3 * j] = j;
                triangles[3 * j + 1] = j + 1;
                triangles[3 * j + 2] = j + 2;
            }
            else
            {
                triangles[3 * j] = j + 1;
                triangles[3 * j + 1] = j;
                triangles[3 * j + 2] = j + 2;
            }
        }
        //Debug.Log((nVert_fix - 1) + " = " + triangles[nTriangles - 1]);
        mesh.vertices = vert_fix;
        mesh.triangles = triangles;
        mesh.uv = uv;
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

    public void reset()
    {
        mesh.vertices = null;
        mesh.triangles = null;
        mesh.uv = null;
        isEnable = true;
    }

    private float calcAngle(float r, float w)
    {
        float angle = w / (2 * r);
        return angle;
    }

    Vector3 rotVert(Vector3 v0, Vector3 v, float angle)
    {
        Vector3 tg = new Vector3();

        v.x -= v0.x;
        v.y -= v0.y;
        tg.x = v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle) + v0.x;
        tg.y = v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle) + v0.y;
        tg.z = 0;
        return tg;
    }

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        audio = GetComponent(typeof(AudioSource)) as AudioSource;
    }

    public void SetDisable()
    {
        isEnable = false;
    }

    void Update()
    {
        if (isEnable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                countForRemoveVert = 10;
            }
            if (Input.GetMouseButton(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var pos = ray.GetPoint(10);
                pos.z = 0;
                if (vert.Count == 0)
                {
                    vert.AddLast(pos);
                }
                else if (!vert.Last.Value.Equals(pos))
                {
                    vert.AddLast(pos);
                }

                if (vert.Count > 1)
                {
                    checkSlice();
                }
            }
            else
            {
                vert.Clear();
            }
            LineManager();
            UpdateMesh();
        }
    }

    void FixedUpdate()
    {
        //UpdateMesh();
    }


    void LineManager()
    {
        if (countForRemoveVert > 0)
        {
            countForRemoveVert--;
        }
        else
        {
            if (vert.Count > 0)
            {
                vert.RemoveFirst();
            }
        }
    }

    private bool checkSlice()
    {
        Vector3 first = vert.Last.Value;
        Vector3 second = vert.Last.Previous.Value;
        first.z = second.z = 0;
        float dist = Vector3.Distance(first, second);
        RaycastHit[] hits = Physics.RaycastAll(new Ray(first, second - first), dist);
        LinkedList<GameObject> temp = new LinkedList<GameObject>();
        if (hits != null)
        {
            foreach (var hit in hits)
            {
                if (hit.collider.tag == "Fruit" || hit.collider.tag == "Bomb"
                    || hit.collider.tag == "ExtraFruit"
                    || hit.collider.tag == "FruitNewGame"
                    || hit.collider.tag == "FruitQuit")
                {
                    temp.AddLast(hit.collider.gameObject);
                }
            }
        }
        if (temp.Count > 0)
        {
            Debug.Log("slice hit");
            GameObject[] result = new GameObject[temp.Count];
            int i = 0;
            foreach (var obj in temp)
            {
                result[i++] = obj;
            }
            if (OnSlice != null)
            {
                Vector3 dir = first - second;
                Debug.Log("Dir " + dir);
                OnSlice(result, dir);
                audio.Play();
            }
            return true;
        }
        return false;
    }


}
