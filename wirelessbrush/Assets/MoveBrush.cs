using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBrush : MonoBehaviour
{
    
    // public GameObject brush;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(25f, 25f, 25f);
        gameObject.transform.position = new Vector3(0f, 1f, -12f);
        gameObject.transform.Rotate(0f, 180f, 90f, Space.World);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movingBrushDirection = new Vector3(0.1f, 0f, 0f);
        gameObject.transform.Translate(movingBrushDirection*Time.deltaTime, Space.World);
        
    }
}
