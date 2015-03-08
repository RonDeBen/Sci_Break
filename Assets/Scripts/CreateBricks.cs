using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CreateBricks : MonoBehaviour {

	public Texture2D tesselate;
	public int columns, rows;
	public Vector3 brickScale = new Vector3(1,1,1);
	public float perctangeOfScreen;
	private float screenHeight, screenWidth;
	private Vector2 pictureTile, screenTile;
	private int brickNumber = 0;

	void Awake () {
		screenHeight = 2f * Camera.main.orthographicSize;
		screenWidth = screenHeight * Camera.main.aspect;
		if (perctangeOfScreen > 1){
			perctangeOfScreen /= 100;
		}

		CreateGrid(columns, rows, perctangeOfScreen);
	}

	private void CreateGrid(int columns, int rows, float perctangeOfScreen){
		screenTile = new Vector2(screenWidth / columns, (screenHeight * perctangeOfScreen) / rows);
		pictureTile = new Vector2(tesselate.width / columns, tesselate.height / rows);

		Vector3 bottomLeft = new Vector3(0f, screenHeight * (1 - perctangeOfScreen), 0f);

		Vector2[] uvs = new Vector2[]
					{
			            new Vector2(0, 0),
                        new Vector2(1, 0),
                        new Vector2(1, 1),
                        new Vector2(0, 1)
					};
    	Vector3[] norms = new Vector3[]{
                  Vector3.forward,
                  Vector3.forward,
                  Vector3.forward,
                  Vector3.forward
            };

    	int[] tris = new int[6] {2, 1, 0, 0, 3, 2};

    	Vector3[] verts = {
			new Vector3(0f, 0f, 0f),//bottom left
			new Vector3(screenTile.x, 0f, 0f),//bottom right
			new Vector3(screenTile.x, screenTile.y, 0f),//top right
			new Vector3(0f, screenTile.y, 0f)//top left
		};

		Mesh mesh = new Mesh();
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.normals = norms;
		mesh.uv = uvs;
		calculateMeshTangents(mesh);

		for(int x = 0; x < columns; x++){
			for(int y = 0; y < rows; y++){

				Color brickColor = modeColor(x, y, pictureTile, tesselate);
				if(brickColor.a >0.9){

					brickNumber++;

					GameObject go = new GameObject("New_Mesh");
					MeshFilter mf = go.gameObject.AddComponent<MeshFilter>();
					go.AddComponent<MeshRenderer>();

					mf.mesh = mesh;
					mesh.RecalculateBounds();

					go.transform.position = new Vector3((screenTile.x * x), (screenTile.y * y) + bottomLeft.y, 0f);
					go.transform.localScale = brickScale;

					BoxCollider2D bc2d = go.gameObject.AddComponent<BoxCollider2D>();
					bc2d.size = screenTile;

					mf.renderer.material.shader = Shader.Find("Bumped Diffuse");
					go.renderer.material.color = brickColor;
				}
				
			}
		}
	}

	private Color modeColor(int x, int y, Vector2 pictureTile, Texture2D tex){
		Color[] cArray = tex.GetPixels(x*(int)pictureTile.x, y*(int)pictureTile.y, (int)pictureTile.x, (int)pictureTile.y);
		var query =		from colors in cArray
						group colors by colors
						into groupedColors
						select new { Color = groupedColors.Key, Count = groupedColors.Count() };

		int max = query.Max(g => g.Count);
		IEnumerable<Color> modes = query.Where(z => z.Count == max).Select(z => z.Color);	

		return modes.First();
	}

	public static void calculateMeshTangents(Mesh mesh){
	    //speed up math by copying the mesh arrays
	    int[] triangles = mesh.triangles;
	    Vector3[] vertices = mesh.vertices;
	    Vector2[] uv = mesh.uv;
	    Vector3[] normals = mesh.normals;
	     
	    //variable definitions
	    int triangleCount = triangles.Length;
	    int vertexCount = vertices.Length;
	     
	    Vector3[] tan1 = new Vector3[vertexCount];
	    Vector3[] tan2 = new Vector3[vertexCount];
	     
	    Vector4[] tangents = new Vector4[vertexCount];
	     
	    for (long a = 0; a < triangleCount; a += 3)
	    {
	    long i1 = triangles[a + 0];
	    long i2 = triangles[a + 1];
	    long i3 = triangles[a + 2];
	     
	    Vector3 v1 = vertices[i1];
	    Vector3 v2 = vertices[i2];
	    Vector3 v3 = vertices[i3];
	     
	    Vector2 w1 = uv[i1];
	    Vector2 w2 = uv[i2];
	    Vector2 w3 = uv[i3];
	     
	    float x1 = v2.x - v1.x;
	    float x2 = v3.x - v1.x;
	    float y1 = v2.y - v1.y;
	    float y2 = v3.y - v1.y;
	    float z1 = v2.z - v1.z;
	    float z2 = v3.z - v1.z;
	     
	    float s1 = w2.x - w1.x;
	    float s2 = w3.x - w1.x;
	    float t1 = w2.y - w1.y;
	    float t2 = w3.y - w1.y;
	     
	    float r = 1.0f / (s1 * t2 - s2 * t1);
	     
	    Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
	    Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
	     
	    tan1[i1] += sdir;
	    tan1[i2] += sdir;
	    tan1[i3] += sdir;
	     
	    tan2[i1] += tdir;
	    tan2[i2] += tdir;
	    tan2[i3] += tdir;
	    }
	     
	     
	    for (long a = 0; a < vertexCount; ++a)
	    {
	    Vector3 n = normals[a];
	    Vector3 t = tan1[a];
	     
	    //Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
	    //tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
	    Vector3.OrthoNormalize(ref n, ref t);
	    tangents[a].x = t.x;
	    tangents[a].y = t.y;
	    tangents[a].z = t.z;
	     
	    tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
	    }
	     
	    mesh.tangents = tangents;
    }

	public int getBrickNumber(){
		return brickNumber;
	}

}
