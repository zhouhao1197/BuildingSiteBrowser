using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewController:Singleton<CameraViewController>
{
    private enum CameraAction
    {
        rotate,
        translate
    }

    [Tooltip("The Vector which is the delta position of camera and target obj")]
    public Vector3 MoveDistance = new Vector3(0, 10, -20);

    public float RotateSensitive = 300.0f;
    public float ScrollSensitive = 100.0f;
    public float TranslateSensitive = 10.0f;

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
        if (Input.GetMouseButtonDown(1)|| Input.GetMouseButtonDown(0))// To avoid hop
        {
            LastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            DetectMouseAxis(CameraAction.rotate);
        }

        if (Input.GetMouseButton(0))
        {
            DetectMouseAxis(CameraAction.translate);
        }

        float scrolldelta = Input.mouseScrollDelta.y;
        if (scrolldelta != 0)
        {
            mainCamera.Ref.Translate(mainCamera.Ref.forward*scrolldelta*Time.deltaTime*ScrollSensitive);
        }

    }

    private void DetectMouseAxis(CameraAction action)
    {
        Vector3 MouseDelta = Input.mousePosition - LastMousePosition;
        LastMousePosition = Input.mousePosition;
        if (action == CameraAction.rotate)
        {
            mainCamera.Ref.RotateAround(RotationOriginPosition, new Vector3(0, 1, 0), MouseDelta.x * RotateSensitive * Time.deltaTime);
            mainCamera.Ref.RotateAround(RotationOriginPosition, new Vector3(1, 0, 0), -MouseDelta.y * RotateSensitive * Time.deltaTime);
        }
        else if(action==CameraAction.translate)
        {
            mainCamera.Ref.Translate(-MouseDelta* TranslateSensitive*Time.deltaTime);
        }
    }

    public void SetCameraPosition(Vector3 TargetObjPosition)
    {
        mainCamera.Ref.position = TargetObjPosition + MoveDistance;
        RotationOriginPosition = TargetObjPosition;
        mainCamera.Ref.rotation = Quaternion.LookRotation(-MoveDistance) * DefaultRotation;
    }
}
