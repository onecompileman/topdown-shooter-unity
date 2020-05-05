using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    public Transform follow;

    [SerializeField]
    public float followSpeed = 2.5f;

    public float xOffset = 0;
    public float yOffset = 0;

    public float xOffsetC = 0;
    public float yOffsetC = 0;

    void Update()
    {
        xOffsetC = Mathf.Lerp(xOffsetC, xOffset, 0.2f);
        yOffsetC = Mathf.Lerp(yOffsetC, yOffset, 0.2f);
        var positionToFollow = new Vector3(follow.position.x + xOffsetC, 14f, follow.position.z - 5f + yOffsetC);

        transform.position = Vector3.Lerp(transform.position, positionToFollow, followSpeed * Time.deltaTime);
    }
}
