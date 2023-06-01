using UnityEngine;

public class BulletView : MonoBehaviour, IReleasable
{
    [SerializeField] private Transform _bulletContainer;
    
    private float _speed;
    private Collider2D _collider2D;

    public int DamageValue => 1;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        _bulletContainer.Translate(0, _speed * Time.deltaTime, 0, Space.Self);
        transform.position = _bulletContainer.position;
        _bulletContainer.localPosition = Vector3.zero;

        if (!_collider2D.bounds.IsInBounds(Camera.main.Bounds())) 
            Pool.Release(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Pool.Release(this);
    }

    public void Run(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        _speed = 10f;
    }

    public void Dispose()
    {
        _speed = 0;
    }
}