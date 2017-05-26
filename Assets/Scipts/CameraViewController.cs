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

    //[Tooltip("The Vector which is the delta position of camera and target obj")]
    //public Vector3 MoveDistance = new Vector3(0, 100f, -200f);

    public float RotateSensitive = 300.0f;
    public float ScrollSensitive = 100.0f;
    public float TranslateSensitive = 10.0f;

    private Vector3 RotationOriginPosition;
    private Transform TargetObj;

    private ComRef<Transform> mainCamera;
    private Vector3 LastMousePosition;
    private Quaternion DefaultRotation;


    private void Start()
    {
        mainCamera = new ComRef<Transform>(() =>
         {
             return Camera.main.transform;
         });

        //RotationOriginPosition = mainCamera.Ref.position;
        TargetObj = GameObject.Find("Building").transform;
        RotationOriginPosition = TargetObj.position;
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
            //Zoom in/out should make by camera's fov not the distance
            Camera.main.fieldOfView += -scrolldelta * Time.deltaTime * ScrollSensitive;
        }
    }

    private void DetectMouseAxis(CameraAction action)
    {
        Vector3 MouseDelta = Input.mousePosition - LastMousePosition;
        LastMousePosition = Input.mousePosition;
        if (action == CameraAction.rotate)
        {
            if (Mathf.Abs(MouseDelta.x) > (Mathf.Abs(MouseDelta.y)))
            {
                //Rotate by the y axis should always be the target's up
                //here be the forward because of the wrong axis of the obj
                if (TargetObj.name == "Building")
                {
                    mainCamera.Ref.RotateAround(RotationOriginPosition, TargetObj.up, MouseDelta.x * RotateSensitive * Time.deltaTime);
                }
                else
                {
                    mainCamera.Ref.RotateAround(RotationOriginPosition, TargetObj.forward, MouseDelta.x * RotateSensitive * Time.deltaTime);
                }
            }
            else
            {
                
                //rotate by the y axis should be the camera's forward cross the target's up
                //here be the forward because of the wrong axis of the obj
                if (TargetObj.name == "Building")
                {
                    float angle = Vector3.Angle(mainCamera.Ref.forward, TargetObj.up);
                    //when the forward is close to the tartget axis ,return to avoid reversal
                    if ((Mathf.Abs(angle) <=5f && MouseDelta.y<=0) || (Mathf.Abs(angle) >= 175f&&MouseDelta.y>=0)) 
                        return;
                    mainCamera.Ref.RotateAround(RotationOriginPosition, Vector3.Cross(mainCamera.Ref.forward, TargetObj.up), -MouseDelta.y * RotateSensitive * Time.deltaTime);
                }
                else
                {
                    float angle = Vector3.Angle(mainCamera.Ref.forward, TargetObj.forward);
                    if ((Mathf.Abs(angle) < 0.1f && MouseDelta.y <= 0) || (Mathf.Abs(angle) > 179.9f && MouseDelta.y >= 0))
                        return;
                    mainCamera.Ref.RotateAround(RotationOriginPosition, Vector3.Cross(mainCamera.Ref.forward, TargetObj.forward), -MouseDelta.y * RotateSensitive * Time.deltaTime);
                }
            }
        }
        else if(action==CameraAction.translate)
        {
            mainCamera.Ref.Translate(-MouseDelta* TranslateSensitive*Time.deltaTime);
        }
    }

    public void SetCameraPosition(Transform _TargetObj)
    {
        TargetObj = _TargetObj;
		Vector3 _position;
		if (_TargetObj.GetComponent<MeshCollider> () != null) {
			_position = _TargetObj.GetComponent<MeshCollider> ().bounds.center;
		}
		else
		{
			_position = _TargetObj.position;
		}
		mainCamera.Ref.position = _position+ TargetObj.up*50f;
		mainCamera.Ref.position += TargetObj.forward * 10f;
        RotationOriginPosition = _position;
    }
}
