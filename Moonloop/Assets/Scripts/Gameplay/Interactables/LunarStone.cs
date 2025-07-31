using UnityEngine;

public class LunarStone : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Lunar Stone Interacted with!");
        Moon.Instance.NextPhase();
    }
}
