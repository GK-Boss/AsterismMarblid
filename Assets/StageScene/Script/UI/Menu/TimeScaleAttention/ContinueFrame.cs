using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueFrame : MonoBehaviour
{
    float currentRotZ;
    private void Update()
    {
        //currentRotZ = transform.localRotation.z;
        currentRotZ += Time.deltaTime * 90;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, currentRotZ);
    }
}
