using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTransform : MonoBehaviour
{
    public bool fixRotation;
    void Update()
    {
        if(fixRotation)
            if (transform.eulerAngles != Vector3.zero)
                transform.eulerAngles = Vector3.zero;
    }
}
