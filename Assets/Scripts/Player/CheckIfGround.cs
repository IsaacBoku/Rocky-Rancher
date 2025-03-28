using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfGround : MonoBehaviour
{

    public Collider playerCollider;

    public bool isGround;
    public bool isOnTerrain;
    public bool isInside;

    RaycastHit hit;

    private void Update()
    {
        isGround = PlayerGrounded();
        isOnTerrain = CheckOnTerrain();
    }

    bool PlayerGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down,out hit, playerCollider.bounds.extents.y +0.5f);
    }
    bool CheckOnTerrain()
    {
        if(hit.collider != null && hit.collider.tag == "Terrain")
        {
            return true;
        }
        else
            return false;
    }

}
