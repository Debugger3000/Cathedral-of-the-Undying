using UnityEngine;
public class LockHealthBar : MonoBehaviour
{
    Quaternion fixedRotation;

    void Awake()
    {
        // Capture the rotation we want (usually 0,0,0) at the start
        fixedRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Force the rotation to stay exactly as it was
        transform.rotation = fixedRotation;
    }
}