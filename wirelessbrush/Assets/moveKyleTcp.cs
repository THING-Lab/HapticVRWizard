using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveKyleTcp : MonoBehaviour
{
    bool rotationToProcess = false;
    public float degreesToRotate;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (rotationToProcess)
        {
            transform.Rotate(0f, degreesToRotate, 0f);
            rotationToProcess = false;
        }
    }

    public void moveFromTcp(float degreesToRotate_)
    {
        degreesToRotate = degreesToRotate_;
        rotationToProcess = true;
    }
}