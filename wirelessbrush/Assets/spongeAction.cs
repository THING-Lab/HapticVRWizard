using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spongeAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.latestJSON != null)
        {
            if (Global.latestJSON.tool == "spong00")
            {
                float squishReading = (float) Global.latestJSON.data;
                gameObject.GetComponent<Transform>().localScale = new Vector3(
                    (0.10f * (1023 - squishReading))/1023,
                    (0.07f * (1023 - squishReading))/1023,
                    (0.05f * (1023 - squishReading))/1023
                    );
            }
        }
    }
}
