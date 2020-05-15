using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFire : MonoBehaviour
{
    public bool isFiring = false;

    public void OnPointerUp()
    {
        isFiring = false;
    }
    public void OnPointerDown()
    {
        isFiring = true;
    }
}
