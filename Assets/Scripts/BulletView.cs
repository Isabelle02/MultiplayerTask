using Photon.Pun;
using UnityEngine;

public class BulletView : MonoBehaviourPun
{
    [SerializeField] private Transform _bulletContainer;
    
    private float _speed;
    
    private void Update()
    {
        _bulletContainer.Translate(0, _speed * Time.deltaTime, 0, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        _speed = 0f;
        Pool.Release(this);
    }

    public void Run(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        _speed = 10f;
    }
}