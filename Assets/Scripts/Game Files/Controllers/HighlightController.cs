using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
	public GameObject highlightObject;
	public float highlightSpeed;

    public GameObject ground;

	private Vector3 _currentDestination;

    void Update()
	{
		if (Input.GetMouseButton(2) /*|| GameController.Instance.Mode != GameController.GameMode.Build*/)
		{
            if (highlightObject.activeSelf)
			    highlightObject.SetActive(false);
            return;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			// Round the position to be in the center of the tile.
			// e.g. (0.31, 0.92) -> (0.5, 0.5), which is tile at position (0, 0)
			Vector2 hitPos = new Vector2(Mathf.RoundToInt(hit.point.x + 0.5f), Mathf.RoundToInt(hit.point.z + 0.5f));

			// Show the highlight object
			highlightObject.SetActive(true);

			_currentDestination = new Vector3(hitPos.x, 0f, hitPos.y);
        }

		// Smoothly move the highlight object to where it needs to be
		highlightObject.transform.position = Vector3.Lerp(highlightObject.transform.position, _currentDestination, highlightSpeed);
        ground.GetComponent<MeshRenderer>().material.SetVector("_FocusPos", new Vector4(
            highlightObject.transform.position.x - 0.5f,
            highlightObject.transform.position.y,
            highlightObject.transform.position.z - 0.5f,
            0.0f)
        );
    }
}