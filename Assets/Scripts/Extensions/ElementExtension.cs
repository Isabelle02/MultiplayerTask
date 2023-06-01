using UnityEngine;

public static class ElementExtension
{
    public static void SetPositionXY(this Transform transform, float x, float y)
    {
        var pos = transform.position;
        pos.x = x;
        pos.y = y;
        transform.position = pos;
    }
    
    public static void LookAt2D(this Transform transform, Vector3 vector)
    {
        var angle = Mathf.Atan2(vector.y,vector.x) * Mathf.Rad2Deg + 270;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    public static bool IsInBounds(this Bounds target, Bounds other)
    {
        return target.min.x <= other.max.x && target.max.x >= other.min.x &&
               target.min.y <= other.max.y && target.max.y >= other.min.y;
    }
}