using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycaster : Singleton<MouseRaycaster> {

	public Transform HitTransform;
	private float _lastClickTime;

	// Update is called once per frame
	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitinfo;
		if (Physics.Raycast (ray, out hitinfo))
		{
			HitTransform = hitinfo.transform;
		}
		else
		{
			HitTransform = null;
		}

		if (Input.GetMouseButtonDown (0))
		{
			if (Time.time - _lastClickTime < 0.3f)
			{
				if (HitTransform != null)
				{
					CameraViewController.Instance.SetCameraPosition (HitTransform);
				}
			}
			_lastClickTime = Time.time;
		}
	}
}
