using UnityEngine;

public abstract class Window : MonoBehaviour
{
    private void Awake()
    {
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = CameraManager.UiCamera;
    }

    public bool IsActive => gameObject.activeSelf;

    protected abstract void OnOpen(Window.ViewParam viewParam);
    protected abstract void OnClose();

    public void Open(Window.ViewParam viewParam)
    {
        gameObject.SetActive(true);
        OnOpen(viewParam);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
        OnClose();
    }
    
    public abstract class ViewParam
    {
    }
}