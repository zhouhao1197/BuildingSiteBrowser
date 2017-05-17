using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComRef<T>
{
    public delegate T GetRefDelegate();
    GetRefDelegate _getRef;

    private T _ref;

    public ComRef(GetRefDelegate getRef)
    {
        _getRef = getRef;
    }

    public T Ref
    {
        get
        {
            if (_ref == null)
            {
                if (_getRef != null)
                {
                    _ref = _getRef();
                }
            }
            return _ref;
        }
    }
}
