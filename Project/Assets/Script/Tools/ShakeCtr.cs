using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ShakeCtr : MonoBehaviour {

    public float startTime = 1.0f;
    public Vector3 directionStrength = new Vector3(0,1,0); 
    public float Speed = 1.0f ;  
    public AnimationCurve curve  = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.13f, 0.4f), new Keyframe(0.33f, -0.33f), new Keyframe(0.5f, 0.17f), new Keyframe(0.71f, -0.12f),new Keyframe(1, 0));

    float timer;
    Vector3 thisPosition;
    Vector3 shakePosition;
    void FixedUpdate() {
        thisPosition = transform.localPosition - shakePosition;
        shakePosition = new Vector3(curve.Evaluate((Time.time - timer) * Speed) * directionStrength.x, curve.Evaluate((Time.time - timer) * Speed) * directionStrength.y, curve.Evaluate((Time.time - timer) * Speed) * directionStrength.z);
        if (timer >= startTime)
            transform.localPosition = shakePosition + thisPosition;
        else
            timer += Time.deltaTime;
    }
}
