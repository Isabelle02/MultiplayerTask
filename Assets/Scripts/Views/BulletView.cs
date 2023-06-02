using UnityEngine;

public class BulletView : MonoBehaviour, IReleasable
{
    [SerializeField] private Transform _bulletContainer;
    
    private float _speed;
    private Collider2D _collider2D;

    public int DamageValue => 1;
    public string Id { get; private set; }

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        _bulletContainer.Translate(0, _speed * Time.deltaTime, 0, Space.Self);
        transform.position = _bulletContainer.position;
        _bulletContainer.localPosition = Vector3.zero;

        if (!_collider2D.bounds.IsInBounds(CameraManager.GameCamera.Bounds())) 
            Pool.Release(this);
    }
    
    private void OnTriggerEnter2D(Collider2D collider2d)
    {
        Debug.Log("trigger bullet");
        if (collider2d.TryGetComponent(out CharacterView characterView))
            if (Id == characterView.Id || PlayerView.CharacterView == characterView && Id != characterView.Id)
                return;
        
        Debug.Log($"bullet {Id} triggered");
        Pool.Release(this);
    }

    public void Run(string id, Vector3 position, Quaternion rotation)
    {
        Id = id;
        transform.position = position;
        transform.rotation = rotation;
        _speed = 10f;
    }

    public void Dispose()
    {
        _speed = 0;
    }
}