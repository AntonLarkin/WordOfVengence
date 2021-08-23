using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    [SerializeField] private Texture2D mainCursor;
    [SerializeField] private Vector2 cursoreHotspot = new Vector2(0, 0);

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayerMask;

    private RaycastHit destinationInfo;

    private void Update()
    {
        SetCursor();
    }

    private void SetCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, groundLayerMask))
        {
            Cursor.SetCursor(mainCursor, cursoreHotspot, CursorMode.Auto);
        }
    }
}
