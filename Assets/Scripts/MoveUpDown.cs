using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    private RectTransform rect;
    void Start()
    {
        rect = GetComponent<RectTransform>();
        LeanTween.move(gameObject, new Vector3(rect.position.x, rect.position.y + 10, rect.position.z), 1.5f).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
