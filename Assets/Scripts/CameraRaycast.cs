using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField] private LayerMask accesableArea;
    [SerializeField] private LayerMask unaccesableArea;
    [SerializeField] private LayerMask enemy;

    private RaycastHit destinationInfo;

    public event Action<Vector3> OnPlayerMove;
    public event Action OnPlayerStop;
    public event Action<Vector3> OnPlayerInteract;
    public event Action<Vector3> OnPlayerAttack;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var moveDirection = ray.direction.normalized;
            if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, unaccesableArea))
            {
                OnPlayerStop?.Invoke();
            }

            else if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, enemy))
            {
                
                OnPlayerAttack?.Invoke(destinationInfo.point);
            }

            else if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, accesableArea))
            {
                OnPlayerMove?.Invoke(destinationInfo.point);
            }


        }

    }

}
