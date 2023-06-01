using UnityEngine;
using UnityEngine.UI;

public static class ImageExtension
{
    public static void SetAlpha(this Image image, float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
    
    public static void SetAlpha(this SpriteRenderer sprite, float alpha)
    {
        var color = sprite.color;
        color = new Color(color.r, color.g, color.b, alpha);
        sprite.color = color;
    }
}