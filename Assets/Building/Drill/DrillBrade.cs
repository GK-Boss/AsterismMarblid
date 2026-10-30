using UnityEngine;

public class DrillBrade : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    private float currentAngle;
    private void Update()
    {
        currentAngle -= rotateSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
