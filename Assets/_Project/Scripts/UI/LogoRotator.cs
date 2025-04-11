using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class LogoRotator : MonoBehaviour
{
    public float speed = 45f;

    void Update()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
