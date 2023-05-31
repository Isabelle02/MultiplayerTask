using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    private void Awake()
    {
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    protected abstract void OnOpen(ViewParam viewParam);

    protected abstract void OnClose();

    public void Open(ViewParam viewParam)
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

