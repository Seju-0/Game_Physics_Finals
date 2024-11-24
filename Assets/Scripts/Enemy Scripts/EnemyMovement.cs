using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;
    public Vector3 areaSize;
    public Vector3 centerPosition;
    private Vector3 targetPosition;

    void Start()
    {
        SetNewTargetPosition();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetNewTargetPosition();
        }
    }

    private void SetNewTargetPosition()
    {
        targetPosition = new Vector3(
            Random.Range(centerPosition.x - areaSize.x / 2, centerPosition.x + areaSize.x / 2),
            transform.position.y,
            Random.Range(centerPosition.z - areaSize.z / 2, centerPosition.z + areaSize.z / 2)
        );
    }
}
