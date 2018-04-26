using UnityEngine;
using System.Collections;

public class RotBySelf : MonoBehaviour 
{
    const float rotSpeed = 360;
    [Header("旋转方向")]
    public Vector3 rotDir;
    [Header("是否激活")]
    public bool isActive;

    void OnEnable()
    {
        isActive = false;
    }

    void Update()
    {
        if (isActive)
            transform.Rotate(rotDir, rotSpeed * Time.deltaTime);
    }

}
