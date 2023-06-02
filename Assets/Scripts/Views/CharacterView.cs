using System;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _icon;

    private Collider2D _collider2D;
    
    public event Action<int> Damaged;

    public Vector2 Size => _collider2D.bounds.size;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out BulletView bullet) && PlayerView.CharacterView != this)
        {
            Damaged?.Invoke(bullet.DamageValue);
        }

        if (other.TryGetComponent(out CoinView coin) && PlayerView.CharacterView == this)
        {
            CurrencyManager.Coins += coin.Value;
            Pool.Release(coin);
        }
    }

    public void SetColor(Color color)
    {
        _icon.color = color;
    }
}