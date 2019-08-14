using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorAdoptionSphere : MonoBehaviour
{
    public GameObject brushMain;
    public GameObject bucketVisiblePart;
    public float opacity;
    // Start is called before the first frame update
    void Start()
    {
        float _adoptDistance = brushMain.GetComponent<adoptColor>().adoptDistance;
        gameObject.GetComponent<Transform>().localScale = new Vector3(2*_adoptDistance,2*_adoptDistance,2*_adoptDistance);
    }

    // Update is called once per frame
    void Update()
    {
        Color _color = bucketVisiblePart.GetComponent<Renderer>().material.color;
        _color.a = opacity;
        gameObject.GetComponent<Renderer>().material.color = _color;
    }
}
