using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    private Transform building;

    private void Start()
    {
        building = GameObject.Find("Building").transform;
    }

    public void OnClickTitle(GameObject button)
    {
        if (button.transform.Find("SubTitle"))
        {
            GameObject _obj = button.transform.Find("SubTitle").gameObject;
            bool subTitleActive = _obj.activeSelf;
            _obj.SetActive(!subTitleActive);
        }
    }

    public void OnClickPlaceRoot(string PlaceName)
    {
        Transform _placePosition = building.transform.Find(PlaceName);
        CameraViewController.Instance.SetCameraPosition(_placePosition);
    }
}
