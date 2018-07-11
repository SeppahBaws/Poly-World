using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    public GameObject highlightObject;
    public float highlightSpeed;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector2 hitPos = new Vector2(Mathf.RoundToInt(hit.point.x + 0.5f), Mathf.RoundToInt(hit.point.x + 0.5f));
            
            // Show the highlight object
            highlightObject.SetActive(true);
            // Set the position of the object. The lerp is for smooth positioning
            highlightObject.transform.position = Vector3.Lerp(highlightObject.transform.position,
                new Vector3(hitPos.x, 0f, hitPos.y), highlightSpeed);
        }
    }

    bool IsMouseMoving()
    {
        return Input.GetAxis("Mouse X") != 0f && Input.GetAxis("Mouse Y") != 0f;
    }
}