using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lengthX, startPosX, startPosY;
    [SerializeField] private bool isStatic;
    [SerializeField] private GameObject _camera;
    [SerializeField] private float parallaxEffectX;
    [SerializeField] private float parallaxEffectY;
    [SerializeField] private float manualLengthX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            lengthX = sr.bounds.size.x;
        }
        else if (manualLengthX > 0f)
        {
            lengthX = manualLengthX;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float tempX = _camera.transform.position.x * (1 - parallaxEffectX);
        float distX = isStatic ? 0f : _camera.transform.position.x * parallaxEffectX;
        float distY = _camera.transform.position.y * parallaxEffectY;

        transform.position = new Vector3((isStatic ? _camera.transform.position.x : startPosX + distX), startPosY + distY, transform.position.z);

        if (!isStatic)
        {
            if (tempX > startPosX + lengthX)
                startPosX += lengthX;
            else if (tempX < startPosX - lengthX)
                startPosX -= lengthX;
        }
    }
}
