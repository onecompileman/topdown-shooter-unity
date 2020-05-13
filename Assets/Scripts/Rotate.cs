using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField]
    public float rotateSpeed = 1;

    [SerializeField]
    public string rotateCoords = "y";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (rotateCoords)
        {
            case "y":
                transform.Rotate(0, rotateSpeed, 0);
                break;
            case "x":
                transform.Rotate(rotateSpeed, 0, 0);
                break;
            case "z":
                transform.Rotate(0, 0, rotateSpeed);
                break;
        }
    }
}
