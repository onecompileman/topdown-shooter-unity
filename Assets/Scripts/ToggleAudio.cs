using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField]
    public Sprite audioOn;

    [SerializeField]
    public Sprite audioOff;

    [SerializeField]
    public Image audioImage;
    private bool hasAudio = true;

    public void Toggle()
    {
        hasAudio = !hasAudio;

        audioImage.sprite = hasAudio ? audioOn : audioOff;
    }
}
