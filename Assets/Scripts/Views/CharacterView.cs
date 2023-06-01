using System;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    public event Action<int> Damaged;
    
    public Collider2D Collider2D { get; private set; }

    private void Awake()
    {
        Collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out BulletView bullet))
        {
            Damaged?.Invoke(bullet.DamageValue);
        }

        if (other.TryGetComponent(out CoinView coin))
        {
            CurrencyManager.Coins += coin.Value;
            Pool.Release(coin);
        }
    }
}