using UnityEngine;

public static class ElementExtension
{
    public static void SetLocalPositionXY(this Transform transform, float x, float y)
    {
        var pos = transform.position;
        pos.x = x;
        pos.y = y;
        transform.position = pos;
    }

    public static void SetRotationZ(this Transform transform, float z)
    {
        var rot = transform.rotation;
        rot.z = z;
        transform.rotation = rot;
    }
}