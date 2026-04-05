using UnityEngine;
public class LockHealthBar : MonoBehaviour
{

    void LateUpdate()
    {
        // Directly cancel the parent's Z rotation
        float parentZ = transform.parent.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(0f, 0f, -parentZ);
    }
}