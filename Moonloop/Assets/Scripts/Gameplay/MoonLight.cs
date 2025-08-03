using System.Collections;
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
    public bool cycle;

    [Header("References")]
    [SerializeField] private PlatformManager _platformManager;
    [SerializeField] private MoonBar _moonBar;

    [Header("Light Detection")]
    [SerializeField] private Light2D _light;
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private Transform _excludedPlatforms;
    [SerializeField] private float _lightFadeDuration = 1.0f;
    [SerializeField] private Transform _groundLeftCorner;
    [SerializeField] private Transform _groundRightCorner;

    private int _currentPhase;
    private float _phaseTimer;
    private float _angle;
    private float _speed;
    private float _cycleProgress;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentPhase = 0; // 0 - 1st, 1 - full, 2 - 3rd, 4 - new
        _phaseTimer = 0.0f;
        _angle = transform.rotation.z - 45;
        _speed = 360.0f / (_maxPhases * _phaseDuration);

        if (!_light) _light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ActivateLit();
        LightCollision();
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

    private void ActivateLit()
    {
        _platformManager.ActivateDarkPlatforms();
        foreach (Collider2D platform in Physics2D.OverlapCircleAll(transform.position, _light.pointLightOuterRadius,
                     _platformLayer))
        {
            if (platform.transform == _excludedPlatforms || platform.transform.IsChildOf(_excludedPlatforms)) continue;
            
            Platform platformData = platform.GetComponent<Platform>();
            if (!platformData) continue;
            platformData.unstable = _currentPhase is 0 or 2;

            Vector3 direction = platform.transform.position - transform.position;
            if (Vector3.Angle(transform.up, direction) <= _light.pointLightOuterAngle / 2)
            {
                Debug.DrawRay(transform.position, direction.normalized * direction.magnitude, Color.green);
                
                bool blockedByObstacle = false;
                RaycastHit2D[] hitAll = Physics2D.RaycastAll(transform.position, direction.normalized, 
                    direction.magnitude, _platformLayer);
                foreach (RaycastHit2D hit in hitAll)
                {
                    Collider2D hitCollider = hit.collider;
                    if (!hitCollider || hitCollider.transform == platform.transform) continue;
                    blockedByObstacle = !hitCollider.TryGetComponent(out Platform pd) || pd.isActive;
                    if (blockedByObstacle) break;
                }
                
                if (!blockedByObstacle) // Lit
                {
                    if (platformData.type == PlatformType.Light)
                        _platformManager.Activate(platform.transform);
                    else
                        _platformManager.Deactivate(platform.transform);
                }
                else // Unlit
                {
                    if (platformData.type == PlatformType.Light)
                        _platformManager.Deactivate(platform.transform);
                    else
                        _platformManager.Activate(platform.transform);
                }
            }
            else // Unlit
            {
                if (platformData.type == PlatformType.Light)
                    _platformManager.Deactivate(platform.transform);
                else
                    _platformManager.Activate(platform.transform);
            }
        }
    }

    private void LightCollision()
    {
        if (transform.position.x > _groundLeftCorner.position.x &&
            transform.position.x < _groundRightCorner.position.x &&
            transform.position.y < _groundLeftCorner.position.y)
        {
            if (_light.intensity == 1f)
                StartCoroutine(LerpIntensityCo(_light, 0f));
        }
        else if (_light.intensity == 0)
            StartCoroutine(LerpIntensityCo(_light, 1f));
    }

    IEnumerator LerpIntensityCo(Light2D light, float to)
    {
        float elapsedTime = 0f;
        float baseIntensity = light.intensity;
        while (elapsedTime < _lightFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            light.intensity = Mathf.Lerp(baseIntensity, to, elapsedTime);
            yield return null;
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
