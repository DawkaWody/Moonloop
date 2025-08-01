using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Moon : MonoBehaviour
{
    public static Moon Instance { get; private set; }

    [SerializeField] private float _phaseDuration = 4.0f;
    [SerializeField] private int _maxPhases;
    [SerializeField] private PlatformManager _platformManager;
    [SerializeField] private Transform _center;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private bool cycle;
    [SerializeField] private Light2D _light;

    private int _currentPhase;
    private float _phaseTimer;
    private float _angle;
    private float _speed;

    // private Light2D _light;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentPhase = 0;
        _phaseTimer = 0.0f;
        _angle = 0.0f;
        _speed = 360.0f / (_maxPhases * _phaseDuration);

        _light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cycle) return;

        _angle += _speed * Time.deltaTime;

        float x = Mathf.Cos(_angle * Mathf.Deg2Rad) * _radius;
        float y = Mathf.Sin(_angle * Mathf.Deg2Rad) * _radius;

        transform.position = new Vector3(_center.position.x + x, _center.position.y + y, transform.position.z);
        Vector3 direction = _center.position - transform.position;
        float angleToCenter = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToCenter - 90));

        LightDetection();
    }

    private void LightDetection()
    {
        Collider2D[] platforms = Physics2D.OverlapCircleAll(transform.position, _light.pointLightOuterRadius, _platformLayer);
        foreach (Collider2D platform in platforms)
        {
            if (platform.name == "Persistent") continue;

            Vector3 direction = platform.transform.position - transform.position;
            if (Vector3.Angle(transform.up, direction) <= _light.pointLightOuterAngle / 2)
            {
                Debug.DrawRay(transform.position, direction.normalized * direction.magnitude, Color.green);
                bool blockedByObstacle = false;
                RaycastHit2D[] hitAll = Physics2D.RaycastAll(transform.position, direction.normalized, direction.magnitude, _platformLayer);
                foreach (RaycastHit2D hit in hitAll)
                {
                    Collider2D hitCollider = hit.collider;
                    if (hitCollider != null && hitCollider.transform != platform.transform) blockedByObstacle = true;
                }
                
                if (!blockedByObstacle)
                    _platformManager.Activate(platform.transform);
                else
                    _platformManager.Deactivate(platform.transform);
            }
            else 
                _platformManager.Deactivate(platform.transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_center.position, _radius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _light.pointLightOuterRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * _light.pointLightOuterRadius);
    }
}
