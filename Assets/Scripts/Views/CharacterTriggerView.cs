using System;
using UnityEngine;

public class CharacterTriggerView : MonoBehaviour
{
    public event Action<int> Damaged;
    public event Action<int> CoinCollect; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out BulletView bullet))
        {
            Damaged?.Invoke(bullet.DamageValue);
        }

        if (other.TryGetComponent(out CoinView coin))
        {
            CoinCollect?.Invoke(coin.Value);
            Pool.Release(coin);
        }
    }
}