using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    private Transform cameraTransform;
    
    void Start()
    {
        //luam transformul de pe camera la start
        cameraTransform = transform;
    }

 
  //LateUpdate este apelata dupa ce toate actualizarile au fost facute la player
    void LateUpdate()
    {
        Vector3 newPosition = playerTransform.position;
        newPosition.z = -10; //pentru a se vedea totul ok z e cu minus

        cameraTransform.position = newPosition;
    }
}
