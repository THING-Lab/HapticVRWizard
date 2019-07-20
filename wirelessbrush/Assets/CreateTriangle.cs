using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTriangle : MonoBehaviour
{
    public GameObject inputAnchor;
    public List<GameObject> bristlesList = new List<GameObject>(5);

    Mesh ribbon;
    List<Vector3> bristleLocations = new List<Vector3>(); // save every bristle position
    List<int> ribbonVertices = new List<int>(); // make triangles from the vertices
    private const int bristles = 5;
    private const int timeSamples = 6;
    private const int squaresLength = timeSamples -1; // the number of sets of squares to check is one less than the number of time samples
    private const int squaresAtOnce = bristles - 1; // number of squares to check at a time sample is one less than the number of bristles
    private List<int> ribbonTriangles = new List<int>();

    private bool[,] brushHistory = new bool[timeSamples,bristles]
    {
        {true, false, false, false, false}, // 5 bristles at time sample 0
        {true, true , false, false, false}, // 5 bristles at time sample 1, and so on...
        {true, true , false, false, false},
        {true, true , true , true , false},
        {true, false, false, true , true },
        {true, false, true , true , true }
    };

    private float sphereDistance = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        ribbon = new Mesh();
        GetComponent<MeshFilter>().mesh = ribbon;

        sphereAtEachBristleLocation();
        
        Vector3[] bristleLocationsList = bristleLocations.ToArray();
        ribbon.vertices = bristleLocationsList;

        drawTrianglesForActiveBristles();

        int[] ribbonTrianglesList = ribbonTriangles.ToArray();
        ribbon.triangles = ribbonTrianglesList;
        
        Renderer rendOther = gameObject.GetComponent<Renderer>();
        rendOther.material.SetColor("_Color", Color.green);

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 20 == 0)
        {
            for (int i = 0; i < bristles; i++)
            {
                GameObject sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere2.transform.position = bristlesList[i].GetComponent<Transform>().position;
                sphere2.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            }
        }
    }

    //////////////////////////////
    ///// CUSTOM FUNCTIONS ///////
    //////////////////////////////
    
    void sphereAtEachBristleLocation()
    {
        for (int i = 0; i < timeSamples; i++)
        {
            for (int j = 0; j < bristles; j++)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Vector3 positionOfBristleTip = new Vector3(i*sphereDistance, j*sphereDistance, 0);
                sphere.transform.position = positionOfBristleTip;
                bristleLocations.Add(positionOfBristleTip); // this inefficiently stores every single bristle position, even if it will not be used because the sensor is not activited
                sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                Renderer rend = sphere.GetComponent<Renderer>();
                if (brushHistory[i,j])
                {
                    rend.material.SetColor("_Color", Color.blue);
                } else
                {
                    rend.material.SetColor("_Color", Color.black);
                }
                
            }
        }
    }

    void drawTrianglesForActiveBristles()
    {
        for (int k = 0; k < squaresLength; k++)
        {
            for (int l = 0; l < squaresAtOnce; l++)
            {
                string stateABCD = checkStateABCD(k,l);
                switch (stateABCD)
                {
                    case "TTTT":
                        triangle123andMirror(k,l);
                        triangle134andMirror(k,l);
                        break;
                    case "TTTF":
                        triangle123andMirror(k,l);
                        break;
                    case "TTFT":
                        triangle124andMirror(k,l);
                        break;
                    case "TFTT":
                        triangle134andMirror(k,l);
                        break;
                    case "FTTT":
                        triangle234andMirror(k,l);
                        break;
                    case "skip":
                        break;
                    default:
                        Debug.Log("Something is wrong if you are reading this in the console");
                        break;
                }
            }
        }
    }

    string checkStateABCD(int m, int n){
        // Find on/off values for each of the four corners, going A-B-C-D in CW direction from upper-left corner being (0,0)
        bool cornerA = brushHistory[m  ,n  ]; // Upper-left, position (0,0);
        bool cornerB = brushHistory[m  ,n+1]; // Upper-right, position (0,1);
        bool cornerC = brushHistory[m+1,n+1]; // Bottom-left, position (1,0);
        bool cornerD = brushHistory[m+1,n  ]; // Bottom-right, position (1,1);

        if        ( cornerA &&  cornerB &&  cornerC &&  cornerD) // ABCD is TTTT
        {
            return "TTTT";
        } else if ( cornerA &&  cornerB &&  cornerC && !cornerD) // ABCD is TTTF
        {
            return "TTTF";
        } else if ( cornerA &&  cornerB && !cornerC &&  cornerD) // ABCD is TTFT
        {
            return "TTFT";
        } else if ( cornerA && !cornerB &&  cornerC &&  cornerD) // ABCD is TFTT
        {
            return "TFTT";
        } else if (!cornerA &&  cornerB &&  cornerC &&  cornerD) // ABCD is FTTT
        {
            return "FTTT";
        } else
        {
            return "skip"; // do not draw a triangle if there are not at least three active points in the square
        }
        
    }

    void triangle123andMirror(int o, int p){ // o and p are the indices of the square being checked
        // tri123forward
        ribbonTriangles.Add(getVertexIndex(o  ,p  ));
        ribbonTriangles.Add(getVertexIndex(o  ,p+1));
        ribbonTriangles.Add(getVertexIndex(o+1,p+1));
        // tri123mirror
        ribbonTriangles.Add(getVertexIndex(o+1,p+1));
        ribbonTriangles.Add(getVertexIndex(o  ,p+1));
        ribbonTriangles.Add(getVertexIndex(o  ,p  ));
    }
    void triangle124andMirror(int o, int p){
        // tri124forward
        ribbonTriangles.Add(getVertexIndex(o  ,p  ));
        ribbonTriangles.Add(getVertexIndex(o  ,p+1));
        ribbonTriangles.Add(getVertexIndex(o+1,p  ));
        // tri124mirror
        ribbonTriangles.Add(getVertexIndex(o+1,p  ));
        ribbonTriangles.Add(getVertexIndex(o  ,p+1));
        ribbonTriangles.Add(getVertexIndex(o  ,p  ));
    }
    void triangle134andMirror(int o, int p){
        // tri134forward
        ribbonTriangles.Add(getVertexIndex(o  ,p  ));
        ribbonTriangles.Add(getVertexIndex(o+1,p+1));
        ribbonTriangles.Add(getVertexIndex(o+1,p  ));
        // tri134mirror
        ribbonTriangles.Add(getVertexIndex(o+1,p  ));
        ribbonTriangles.Add(getVertexIndex(o+1,p+1));
        ribbonTriangles.Add(getVertexIndex(o  ,p  ));
    }
    void triangle234andMirror(int o, int p){
        // tri234forward
        ribbonTriangles.Add(getVertexIndex(o+1,p  ));
        ribbonTriangles.Add(getVertexIndex(o+1,p+1));
        ribbonTriangles.Add(getVertexIndex(o  ,p+1));
        // tri234mirror
        ribbonTriangles.Add(getVertexIndex(o  ,p+1));
        ribbonTriangles.Add(getVertexIndex(o+1,p+1));
        ribbonTriangles.Add(getVertexIndex(o+1,p  ));
    }

    int getVertexIndex(int q, int s)
    {
        return q * bristles + s;
    }

}
