using UnityEngine;

public class StarFlickerOffset : MonoBehaviour
{
    [SerializeField] private string animationStateName;
    [SerializeField] private Vector2 offsetRange = new Vector2(0f, 1f);

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            float randomOffset = Random.Range(offsetRange.x, offsetRange.y);
            animator.Play(animationStateName, 0, randomOffset);
        }
    }
}
