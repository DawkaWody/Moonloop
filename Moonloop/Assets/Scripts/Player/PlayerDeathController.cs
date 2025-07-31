using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerDeathController : MonoBehaviour
{
    [SerializeField] private float _respawnTime;
    [SerializeField] private Transform _killBound;

    private Vector3 _lastCheckpoint;

    private SpriteRenderer _spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lastCheckpoint = transform.position;

        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        _lastCheckpoint = checkpoint.position;
    }

    public void Die()
    {
        StartCoroutine(RespawnCo());
    }

    IEnumerator RespawnCo()
    {
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(_respawnTime);
        _spriteRenderer.enabled = true;
        transform.position = _lastCheckpoint;
    }
}
