using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeGroup : MonoBehaviour
{
    [SerializeField] private List<Image> _images = new();
    [SerializeField] private List<SpriteRenderer> _sprites = new();
    [SerializeField] private List<Text> _texts = new();
    [SerializeField] private List<FadeGroup> _fadeGroups = new();

    public void SetAlpha(float alpha)
    {
        foreach (var i in _images)
        {
            i.SetAlpha(alpha);
        }
        
        foreach (var s in _sprites)
        {
            s.SetAlpha(alpha);
        }

        foreach (var t in _texts)
        {
            t.SetAlpha(alpha);
        }

        foreach (var g in _fadeGroups)
        {
            g.SetAlpha(alpha);
        }
    }
    
    public UniTask DoFade(float alpha, float duration)
    {
        var tasks = new List<UniTask>();
        
        for (var i = 0; i < _images.Count; i++)
        {
            tasks.Add(_images[i].DOFade(alpha, duration).AsyncWaitForCompletion().AsUniTask());
        }
        
        for (var i = 0; i < _sprites.Count; i++)
        {
            tasks.Add(_sprites[i].DOFade(alpha, duration).AsyncWaitForCompletion().AsUniTask());
        }

        for (var i = 0; i < _texts.Count; i++)
        {
            tasks.Add(_texts[i].DOFade(alpha, duration).AsyncWaitForCompletion().AsUniTask());
        }

        for (var i = 0; i < _fadeGroups.Count; i++)
        {
            tasks.Add( _fadeGroups[i].DoFade(alpha, duration));
        }

        return UniTask.WhenAll(tasks);
    }

    [ContextMenu("Rebuild")]
    public void Rebuild()
    {
        var images = GetComponentsInChildren<Image>();
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        var texts = GetComponentsInChildren<Text>();
        
        _images.Clear();
        _sprites.Clear();
        _texts.Clear();
        
        _images.AddRange(images);
        _sprites.AddRange(sprites);
        _texts.AddRange(texts);
    }
}