using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    public Transform follow;

    [SerializeField]
    public float followSpeed = 2.5f;

    [HideInInspector]
    public bool isFollowingPlayer = true;

    private Quaternion rotateTo;
    public float xOffset = 0;
    public float yOffset = 0;

    public float xOffsetC = 0;
    public float yOffsetC = 0;

    void Update()
    {
        if (isFollowingPlayer)
        {
            xOffsetC = Mathf.Lerp(xOffsetC, xOffset, 0.2f);
            yOffsetC = Mathf.Lerp(yOffsetC, yOffset, 0.2f);
            var positionToFollow = new Vector3(follow.position.x + xOffsetC, 17.5f, follow.position.z - 4.5f + yOffsetC);

            rotateTo = Quaternion.Euler(70, 0, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotateTo, followSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, positionToFollow, followSpeed * Time.deltaTime);
        }
    }

    public void MoveToShopKeeper(Action callback)
    {
        isFollowingPlayer = false;
        LeanTween.move(gameObject, new Vector3(-13.25f, 1, -5.64f), 1.5f);
        LeanTween.rotate(gameObject, new Vector3(0, -39, 0), 1.5f).setOnComplete(() => callback());
    }

    public void MoveToFloorGem(Action callback)
    {
        isFollowingPlayer = false;
        LeanTween.move(gameObject, new Vector3(0, 0, 20), 1.5f);
        LeanTween.rotate(gameObject, new Vector3(0, 0, 0), 1.5f).setOnComplete(() => callback());
    }

    public void MoveToLevelUp(Action callback)
    {
        isFollowingPlayer = false;
        LeanTween.move(gameObject, new Vector3(14, 1, -1), 1.5f);
        LeanTween.rotate(gameObject, new Vector3(0, 90, 0), 1.5f).setOnComplete(() => callback());
    }
}
