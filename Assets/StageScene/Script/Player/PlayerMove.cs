using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private void FixedUpdate()
    {
        if (((Vector2)transform.position - (Vector2)Camera.main.transform.position).magnitude > 0.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, Camera.main.transform.position, moveSpeed * Time.fixedDeltaTime);
            Vector3 direction = Camera.main.transform.position - transform.position;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, moveSpeed * Time.fixedDeltaTime));
        }
    }
}
