using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class MoonLight : MonoBehaviour
{
    [Header("Orbiting Settings")]
    [SerializeField] private Transform _center;
    [SerializeField] private float _radius;
    [SerializeField] private float _phaseDuration = 4.0f;
    [SerializeField] private int _maxPhases;
    [SerializeField] private bool cycle;

    [Header("References")]
    [SerializeField] private PlatformManager _platformManager;
    [SerializeField] private MoonBar _moonBar;

    [Header("Light Detection")]
    [SerializeField] private Light2D _light;
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private Transform _excludedPlatforms;

    private int _currentPhase;
    private float _phaseTimer;
    private float _angle;
    private float _speed;
    private float _cycleProgress;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentPhase = 0;
        _phaseTimer = 0.0f;
        _angle = transform.rotation.z - 90;
        _speed = 360.0f / (_maxPhases * _phaseDuration);

        if (!_light) _light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LightDetection();
        if (!cycle) return;
        PhaseTimer();
        Orbit();
    }

    private void Orbit()
    {
        _angle += _speed * Time.deltaTime;

        float x = Mathf.Cos(_angle * Mathf.Deg2Rad) * _radius;
        float y = Mathf.Sin(_angle * Mathf.Deg2Rad) * _radius;

        transform.position = new Vector3(_center.position.x + x, _center.position.y + y, transform.position.z);
        Vector3 direction = _center.position - transform.position;
        float angleToCenter = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToCenter - 90));
    }

    private void PhaseTimer()
    {
        _phaseTimer += Time.deltaTime;
        _cycleProgress = Mathf.Clamp((_currentPhase * _phaseDuration + _phaseTimer) / (_phaseDuration * _maxPhases), 0, 1);
        _moonBar.SetFill(_cycleProgress);
        if (!(_phaseTimer >= _phaseDuration)) return;
        _phaseTimer = 0.0f;
        _currentPhase++;
        if (_currentPhase >= _maxPhases) _currentPhase = 0;
        _moonBar.SetIcon(_currentPhase);
    }

    private void LightDetection()
    {
        Collider2D[] platforms = Physics2D.OverlapCircleAll(transform.position, _light.pointLightOuterRadius, _platformLayer);
        foreach (Collider2D platform in platforms)
        {
            if (platform.transform == _excludedPlatforms || platform.transform.IsChildOf(_excludedPlatforms)) continue;

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
