using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spongeAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Transform>().localScale = new Vector3(0.10f, 0.07f, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.latestJSON != null)
        {
            if (Global.latestJSON.tool == "spong00")
            {
                float squishReading = (float) Global.latestJSON.data;
                if(squishReading < 1000 && squishReading > 5)
                {
                    gameObject.GetComponent<Transform>().localScale = new Vector3(
                        (0.30f * (Mathf.Sqrt(1023) - Mathf.Sqrt(squishReading))/Mathf.Sqrt(1023)),
                        (0.21f * (Mathf.Sqrt(1023) - Mathf.Sqrt(squishReading))/Mathf.Sqrt(1023)),
                        (0.15f * (Mathf.Sqrt(1023) - Mathf.Sqrt(squishReading))/Mathf.Sqrt(1023)));
                } else 
                {
                    // don't change from valid value to invalid
                }
                
            }
        }
    }
}
