using System;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _icon;

    private Collider2D _collider2D;
    
    public event Action<int> Damaged;

    public Vector2 Size => _collider2D.bounds.size;
    public string Id { get; private set; }

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out BulletView bullet) && PlayerView.CharacterView == this && Id != bullet.Id)
        {
            Debug.Log($"character {Id} triggered bullet {bullet.Id}");

            Damaged?.Invoke(bullet.DamageValue);
            Pool.Release(bullet);
        }

        if (other.TryGetComponent(out CoinView coin))
        {
            if (PlayerView.CharacterView == this)
                CurrencyManager.Coins += coin.Value;
            
            Pool.Release(coin);
        }
    }

    public void Init(string id, Color color)
    {
        Id = id;
        _icon.color = color;
    }
}