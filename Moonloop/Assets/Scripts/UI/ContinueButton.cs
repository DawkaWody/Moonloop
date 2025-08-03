using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public void BackToMenu()
    {
        GameManager.Instance.ToMenuFromEnd();
    }
}
