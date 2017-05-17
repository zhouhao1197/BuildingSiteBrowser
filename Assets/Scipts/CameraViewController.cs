using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewController:Singleton<CameraViewController>
{
    public float RotateSensitive=300.0f;

    /// <summary>
    /// The Vector which is the delta position of camera and target obj
    /// </summary>
    public Vector3 MoveDistance = new Vector3(0, 10, -20);

    [HideInInspector]
    public Vector3 RotationOriginPosition;

    private ComRef<Transform> mainCamera;
    private Vector3 LastMousePosition;
    private Quaternion DefaultRotation;


    private void Start()
    {
        mainCamera = new ComRef<Transform>(() =>
         {
             return Camera.main.transform;
         });

        RotationOriginPosition = mainCamera.Ref.position;
        LastMousePosition = Input.mousePosition;
        DefaultRotation = Camera.main.transform.rotation;
    }
	
	private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            LastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            DetectMouseAxis();
        }
	}

    private void DetectMouseAxis()
    {
        Vector3 MouseDelta = Input.mousePosition - LastMousePosition;
        LastMousePosition = Input.mousePosition;
        mainCamera.Ref.RotateAround(RotationOriginPosition, new Vector3(0, 1, 0), MouseDelta.x * RotateSensitive * Time.deltaTime);
        mainCamera.Ref.RotateAround(RotationOriginPosition, new Vector3(1, 0, 0), -MouseDelta.y * RotateSensitive * Time.deltaTime);
    }

    public void SetCameraPosition(Vector3 TargetObjPosition)
    {
        mainCamera.Ref.position = TargetObjPosition + MoveDistance;
        RotationOriginPosition = TargetObjPosition;
        mainCamera.Ref.rotation = Quaternion.LookRotation(-MoveDistance) * DefaultRotation;
    }
}
