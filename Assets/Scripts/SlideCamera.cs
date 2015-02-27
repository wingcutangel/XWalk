using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SlideCamera : MonoBehaviour {
	public LayerMask lm;
	public float distance = 2f;
	public float sensitivityX = .5f;
	public float sensitivityY = .5f;
	public float sensitivityZoom = 3f;
	public float decayRate = 1f;
	public float maxHeight = 10f;
	public float maxAngle = 80f;
	public float bendHeight = 3f;
	public float bendRadius = 5f;
	private float finalY = 1f;
	private float dX = 0f;
	private float dY = 0f;
	private float zoomFactor = 0f;

	Vector3 dummyPosition;
	// Use this for initialization
	void Start () {
		dummyPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1") && !EventSystem.current.IsPointerOverGameObject())
		{
			dX = -Input.GetAxis("Mouse X") * sensitivityX;
			dY = -Input.GetAxis("Mouse Y") * sensitivityY;
		}
		if ( Input.GetAxis("Mouse ScrollWheel") > 0.0f )
		{
			zoomFactor -= sensitivityZoom * Time.deltaTime;
		}
		else if ( Input.GetAxis("Mouse ScrollWheel") < 0.0f )
		{
			zoomFactor += sensitivityZoom * Time.deltaTime;
		}
		distance += zoomFactor;
		distance = Mathf.Clamp(distance, 2f, 20f); 
		finalY += dY;
		finalY = Mathf.Clamp(finalY, 0.3f, maxHeight); 
		Vector3 rayOrigin = dummyPosition + (transform.right * dX) + (transform.up * dY); 
		rayOrigin.y = 1f;
		RaycastHit hit;
		if(Physics.Raycast(rayOrigin, transform.forward, out hit, distance + 500f, lm ))
		{
			MeshCollider meshCollider = hit.collider as MeshCollider;
			if (meshCollider == null || meshCollider.sharedMesh == null)
				return;
			
			Mesh mesh = meshCollider.sharedMesh;
			Vector3[] normals = mesh.normals;
			int[] triangles = mesh.triangles;
			Vector3 n0 = normals[triangles[hit.triangleIndex * 3 + 0]];
			Vector3 n1 = normals[triangles[hit.triangleIndex * 3 + 1]];
			Vector3 n2 = normals[triangles[hit.triangleIndex * 3 + 2]];
			Vector3 baryCenter = hit.barycentricCoordinate;
			Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
			interpolatedNormal = interpolatedNormal.normalized;
			Transform hitTransform = hit.collider.transform;
			interpolatedNormal = hitTransform.TransformDirection(interpolatedNormal);

			var newPosition = hit.point + (distance * interpolatedNormal);
			dummyPosition = hit.point + (5f * interpolatedNormal);
			dummyPosition.y = 1f;
			newPosition.y = finalY;
			transform.position = newPosition;//+ (new Vector3(0f, finalY, 0f));

			transform.forward = -interpolatedNormal;
			transform.position = transform.position + (transform.forward * bendRadius);
			transform.Rotate(Vector3.right, Mathf.Lerp(0, maxAngle, (transform.position.y - bendHeight) /maxHeight));
			transform.position = transform.position - (transform.forward * bendRadius);
//				transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 20);
//				transform.forward = Vector3.Lerp(transform.forward, -interpolatedNormal, Time.deltaTime * 20);
		}
		else
		{
			print ("Missed");
		}

		dX -= dX * decayRate * Time.deltaTime;

		dY -= dY * decayRate * Time.deltaTime;

		zoomFactor -= zoomFactor * decayRate * Time.deltaTime;

	}
}
