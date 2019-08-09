using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adoptColor : MonoBehaviour
{
    public GameObject bucket;
    public GameObject bucketVisiblePart;
    public GameObject brushVisiblePart;
    public float adoptDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bucketPos = bucket.GetComponent<Transform>().position;
        float distance = Vector3.Distance(bucketPos,gameObject.transform.position);
        // Debug.Log(distance);
        if (distance < adoptDistance){
            brushVisiblePart.GetComponent<Renderer>().material.color = bucketVisiblePart.GetComponent<Renderer>().material.color;
        }
    }
}
