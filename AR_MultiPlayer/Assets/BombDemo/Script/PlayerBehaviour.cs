using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public bool bombNearby;
    public CapsuleCollider capsuleCollider;
    private void OnDrawGizmos()
    {
        if(bombNearby)
            Gizmos.DrawSphere(transform.position, capsuleCollider.radius);
    }

}
