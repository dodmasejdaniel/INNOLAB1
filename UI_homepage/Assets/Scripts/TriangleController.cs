using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleController : MonoBehaviour
{   // globle variable
    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //local variable
        bool isSomething = true;
        int x = 420;
        float y = 3.14f;
        double z = 2.235263473d;
        char letter = 'd';
        string message = "hello";


        //esay and fast Console message
        Debug.Log(message);

        //better readable Console message
        Debug.LogError("test error");
        Debug.LogWarning("test warning");
        
    }

    // Update is called once per frame
    void Update()
    {
        counter++;
        if(counter%2 == 0)
        {
            Debug.LogFormat("counter even: {0}", counter);
        }
        else
        {
            Debug.LogFormat("counter odd: {0}", counter);
        }
        
    }
}
