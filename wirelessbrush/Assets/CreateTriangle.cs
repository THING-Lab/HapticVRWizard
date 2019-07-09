using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTriangle : MonoBehaviour
{

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new[] {
            new Vector3(0,0,0),
            new Vector3(0,10,10),
            new Vector3(10,0,10)
        };

        mesh.vertices = vertices;

        triangles = new[]{0, 1, 2};

        mesh.triangles = triangles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
