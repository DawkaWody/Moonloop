using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animator Parameters")]
    [SerializeField] private string _movingBoolName = "isMoving";
    [SerializeField] private string _landTriggerName = "land";
    [SerializeField] private string _jumpTriggerName = "jump";

    [Header("Animator States")]
    [SerializeField] private string _jumpStateName = "Jump";
    [SerializeField] private string _inAirStateName = "InAir";

    private int _movingBoolId;
    private int _landTriggerId;
    private int _jumpTriggerId;

    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();

        _movingBoolId = Animator.StringToHash(_movingBoolName);
        _landTriggerId = Animator.StringToHash(_landTriggerName);
        _jumpTriggerId = Animator.StringToHash(_jumpTriggerName);
    }

    public void AnimateMovement(bool isMoving)
    {
        _animator.SetBool(_movingBoolId, isMoving);
    }

    public void InitiateJump()
    {
        _animator.SetTrigger(_jumpTriggerId);
    }

    public void Land()
    {
        _animator.SetTrigger(_landTriggerId);
    }

    public bool IsInAirOrJumping()
    {
        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName(_jumpStateName) || state.IsName(_inAirStateName);
    }
}
