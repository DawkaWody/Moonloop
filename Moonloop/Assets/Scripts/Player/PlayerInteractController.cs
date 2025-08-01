using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerInteractController : MonoBehaviour
{
    [SerializeField] private Transform _interactPoint;
    [SerializeField] private float _interactRadius = 1.0f;
    [SerializeField] private float _platformDetectionHeight = 0.5f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private LayerMask _groundLayer;

    private PlayerInputHandler _inputHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputHandler.InteractWasPressed)
        {
            Vector2 interactPoint = new Vector2(_interactPoint.position.x, _interactPoint.position.y);
            foreach (Collider2D hit in Physics2D.OverlapCircleAll(interactPoint, _interactRadius))
            {
                if (hit.TryGetComponent(out IInteractable interaction)) interaction.Interact(transform);
            }
        }

        RaycastHit2D platform = Physics2D.Raycast(transform.position, Vector2.down, _platformDetectionHeight, _groundLayer);
        if (platform.collider && platform.collider.CompareTag("Breakable"))
        {
            platform.collider.GetComponent<Platform>().Break();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_interactPoint.position, _interactRadius);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Vector2.down * _platformDetectionHeight));
    }
}
