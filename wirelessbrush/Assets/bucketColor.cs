using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bucketColor : MonoBehaviour
{
    public GameObject bucketAnchor;

    Color newColor;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Renderer from the GameObject
        rend = GetComponent<Renderer>();

        //Set the main Color of the Material to green
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Color.green);

        //Find the Specular shader and change its Color to red
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        float rotationY = bucketAnchor.GetComponent<Transform>().transform.eulerAngles.y;
        // Debug.Log(rotationY);
        float unitizedHue = (Mathf.Abs(rotationY) % 360)/360;
        rend.material.color = Color.HSVToRGB(unitizedHue, 1f, 1f);
    }
}
