using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    [SerializeField]
    public string link;

    public void OpenUrl()
    {
        Application.OpenURL(link);
    }
}
