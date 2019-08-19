using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTriangle : MonoBehaviour
{
    public GameObject inputAnchor;
    public List<GameObject> bristlesList = new List<GameObject>(5);
    public bool randomBristleActivation;
    public float brushWidth;
    public GameObject brushVisiblePart;

    // Mesh ribbon;
    Mesh ribbon2;
    List<Vector3> bristleLocations2 = new List<Vector3>(); // save every bristle position
    List<int> ribbonVertices2 = new List<int>(); // make triangles from the vertices
    List<Color> triColors = new List<Color>();
    private const int bristles = 5;
    private const int timeSamples = 6;
    private const int squaresAtOnce = bristles - 1; // number of squares to check at a time sample is one less than the number of bristles
    private List<int> ribbonTriangles2 = new List<int>();

    private List<bool> bristlesActive = new List<bool>();

    // private float sphereDistance = 0.75f;

    // Start is called before the first frame update
    void Start()
    {        
        // gameObject.AddComponent<MeshFilter>();
        // gameObject.AddComponent<MeshRenderer>();

        ribbon2 = new Mesh();
        GetComponent<MeshFilter>().mesh = ribbon2;
        
        Renderer rendOther = gameObject.GetComponent<Renderer>();
        // rendOther.material.SetColor("_Color", new Color(0.8f, 0.8f, 0.8f, .8f));

        for (int p = 0; p < bristles; p++)
        {
            Vector3 localPos = new Vector3(0f, -brushWidth/2 + brushWidth*p/5 , 0f);
            bristlesList[p].transform.localPosition = localPos;
        }
        
        
    }

    // Update is called once per frame
    bool skippedFirstSet = false;
    void Update()
    {
        // Debug.Log("update active");
        // if (Time.frameCount % 3 == 0)
        if (true) // this was limiting the paint sequence but now going full speed
        {
            // Debug.Log("third frame active");
            for (int j = 0; j < bristles; j++)
            {
                
                if (randomBristleActivation)
                {
                    // Debug.Log("random bristle active");
                    if (0 != (int) Mathf.Floor(Random.value * 15)) // add a true for bristle active state unless the floor of rand(0,15) is 0
                    {
                        bristlesActive.Add(true);
                    } else 
                    {
                        bristlesActive.Add(false);
                    }
                } else if (Global.latestJSON != null)
                {
                    // Debug.Log("NOT random bristle active");
                    if (Global.latestJSON.data != 0 && Global.latestJSON.tool == "brush00")
                    {
                        bristlesActive.Add(true);
                    } else
                    {
                        bristlesActive.Add(false);
                    }
                } else
                {
                    bristlesActive.Add(false);
                }
                
            }
            for (int i = 0; i < bristles; i++)
            {
                // GameObject sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bristleLocations2.Add(bristlesList[i].GetComponent<Transform>().position);
                triColors.Add(brushVisiblePart.GetComponent<Renderer>().material.color);
                // // Debug.Log(brushVisiblePart.GetComponent<Renderer>().material.color.ToString());
                // // Debug.Log("Num verts: " + bristleLocations2.Count + "\t num colors: " + triColors.Count);
                // sphere2.transform.position = bristleLocations2[bristleLocations2.Count-1];
                // sphere2.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                // Renderer rend2 = sphere2.GetComponent<Renderer>();
                // if (bristlesActive[bristlesActive.Count - 5 + i])
                // {
                //     rend2.material.SetColor("_Color", Color.red);
                // } else
                // {
                //     rend2.material.SetColor("_Color", Color.black);
                // }
            }
            if (skippedFirstSet)
            {
                drawTrianglesForActiveBristles2();
            }
            skippedFirstSet = true; // Do not run the first time because there are no geometries to analyze
        }

        ribbon2.Clear();

        Vector3[] bristleLocationsList2 = bristleLocations2.ToArray();
        ribbon2.vertices = bristleLocationsList2;

        Color[] triColorsList = triColors.ToArray();
        ribbon2.colors = triColorsList;

        int[] ribbonTrianglesList2 = ribbonTriangles2.ToArray();
        ribbon2.triangles = ribbonTrianglesList2;
    }

    void drawTrianglesForActiveBristles2()
    {
        for (int l = 0; l < squaresAtOnce; l++)
        {
            string stateABCD = checkStateABCD2(l);
            // Debug.Log(stateABCD);
            switch (stateABCD)
            {
                case "TTTT":
                    triangle123andMirror2(l);
                    triangle134andMirror2(l);
                    break;
                case "TTTF":
                    triangle123andMirror2(l);
                    break;
                case "TTFT":
                    triangle124andMirror2(l);
                    break;
                case "TFTT":
                    triangle134andMirror2(l);
                    break;
                case "FTTT":
                    triangle234andMirror2(l);
                    break;
                case "skip":
                    break;
                default:
                    Debug.Log("Something is wrong if you are reading this in the console");
                    break;
            }
        }
    }

    string checkStateABCD2(int n){
        // Debug.Log("checking ABCD state");
        // Find on/off values for each of the four corners, going A-B-C-D in CW direction from upper-left corner being (0,0)
        bool cornerA = bristlesActive[bristlesActive.Count - 10 + n]; // Upper-left, position (0,0);
        bool cornerB = bristlesActive[bristlesActive.Count -  9 + n]; // Upper-right, position (0,1);
        bool cornerC = bristlesActive[bristlesActive.Count -  4 + n]; // Bottom-left, position (1,0);
        bool cornerD = bristlesActive[bristlesActive.Count -  5 + n]; // Bottom-right, position (1,1);

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

    void triangle123andMirror2(int p){ // o and p are the indices of the square being checked
        // tri123forward
        ribbonTriangles2.Add(bristlesActive.Count - 10 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  9 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  4 + p);
        // tri123mirror
        ribbonTriangles2.Add(bristlesActive.Count -  4 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  9 + p);
        ribbonTriangles2.Add(bristlesActive.Count - 10 + p);
    }
    void triangle124andMirror2(int p){
        // tri124forward
        ribbonTriangles2.Add(bristlesActive.Count - 10 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  9 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  5 + p);
        // tri124mirror
        ribbonTriangles2.Add(bristlesActive.Count -  5 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  9 + p);
        ribbonTriangles2.Add(bristlesActive.Count - 10 + p);
    }
    void triangle134andMirror2(int p){
        // tri134forward
        ribbonTriangles2.Add(bristlesActive.Count - 10 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  4 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  5 + p);
        // tri134mirror
        ribbonTriangles2.Add(bristlesActive.Count -  5 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  4 + p);
        ribbonTriangles2.Add(bristlesActive.Count - 10 + p);
    }
    void triangle234andMirror2(int p){
        // tri234forward
        ribbonTriangles2.Add(bristlesActive.Count -  9 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  4 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  5 + p);
        // tri234mirror
        ribbonTriangles2.Add(bristlesActive.Count -  5 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  4 + p);
        ribbonTriangles2.Add(bristlesActive.Count -  9 + p);
    }
}
