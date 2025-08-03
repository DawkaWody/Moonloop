using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerFxController))]
[RequireComponent(typeof(PlayerSoundController))]
public class PlayerDeathController : MonoBehaviour
{
    [SerializeField] private float _respawnTime;
    [SerializeField] private float _respawnHeightOffset = 0.5f;
    [SerializeField] private Transform _killBound;
    [SerializeField] private Light2D _light;

    private Vector3 _lastCheckpoint;
    private bool _dead;

    private Rigidbody2D _rigidbody;
    private PlayerMovementController _movementController;
    private PlayerFxController _fxController;
    private PlayerSoundController _soundController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lastCheckpoint = transform.position;
        _dead = false;

        _rigidbody = GetComponent<Rigidbody2D>();
        _movementController = GetComponent<PlayerMovementController>();
        _fxController = GetComponent<PlayerFxController>();
        _soundController = GetComponent<PlayerSoundController>();
    }

    void Update()
    {
        if (transform.position.y < _killBound.position.y && !_dead)
        {
            Die();
        }
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        _lastCheckpoint = new Vector3(checkpoint.position.x, checkpoint.position.y + _respawnHeightOffset, checkpoint.position.z);
    }

    public void Die()
    {
        _dead = true;
        _fxController.Dissolve();
        _fxController.Shake();
        _soundController.PlayDeathSound();
        _light.enabled = false;
        _movementController.enabled = false;
        _rigidbody.linearVelocity = Vector2.zero;
        StartCoroutine(RespawnCo());
    }

    IEnumerator RespawnCo()
    {
        yield return new WaitForSeconds(_respawnTime);
        _fxController.Respawn();
        _soundController.PlayRespawnSound();
        _light.enabled = true;
        _movementController.enabled = true;
        transform.position = _lastCheckpoint;
        _dead = false;
    }
}
