using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovementController))]
public class PlayerDeathController : MonoBehaviour
{
    [SerializeField] private float _respawnTime;
    [SerializeField] private float _respawnHeightOffset = 0.5f;
    [SerializeField] private Transform _killBound;

    private Vector3 _lastCheckpoint;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private PlayerMovementController _movementController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lastCheckpoint = transform.position;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _movementController = GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        if (transform.position.y < _killBound.position.y)
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
        StartCoroutine(RespawnCo());
    }

    IEnumerator RespawnCo()
    {
        _spriteRenderer.enabled = false;
        _movementController.enabled = false;
        _rigidbody.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(_respawnTime);
        _spriteRenderer.enabled = true;
        _movementController.enabled = true;
        transform.position = _lastCheckpoint;
    }
}
