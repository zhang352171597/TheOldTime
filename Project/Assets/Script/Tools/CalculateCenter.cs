using UnityEngine;
using System.Collections;

public class CalculateCenter
{

    #region 射线检测
    static int _groundRayMask = -1;
    public static int groundRayMask
    {
        get
        {
            if (_groundRayMask == -1)
                _groundRayMask = LayerMask.GetMask("");
            return _groundRayMask;
        }
    }
    public static Vector3 Pos_ViewToGround(Vector2 fingerPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(fingerPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundRayMask))
        {
            return hit.point;
        }
        else
        {
            return fingerPos;
        }
    }
    #endregion

    #region 世界坐标转UI坐标
    
    public static Vector3 Pos_WorldToNGUI(Vector3 pos)
    {
        var screenPos = GameMgr.Instance._camera.WorldToScreenPoint(pos);
        screenPos.z = 0;
        return UIMgr.Instance.uiCamera.ScreenToWorldPoint(screenPos);
    }

    #endregion

}
