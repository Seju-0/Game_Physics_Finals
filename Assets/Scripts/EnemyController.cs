using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Ball;
    public Transform Arms;
    public Transform OverHeadPos;
    public Transform DribblePos;
    public Transform Target;

    private bool isHoldBall = true;
    private bool isBallinAir = false;
    private float T = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isHoldBall)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Ball.position = OverHeadPos.position;
                Arms.localEulerAngles = Vector3.right * 180;
                transform.LookAt(Target.parent.position);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isHoldBall = false;
                isBallinAir = true;
                T = 0;
            }

            else
            {
                Ball.position = DribblePos.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 4) * 0.5f);
                Arms.localEulerAngles = Vector3.right * 0;
            }
        }

        if (isBallinAir)
        {
            T += Time.deltaTime;
            float duration = 0.5f;
            float t01 = T / duration;

            Vector3 A = OverHeadPos.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            Vector3 arc = Vector3.up * Mathf.Sin(t01 * 3.14f);
            Ball.position = pos + arc;

            if (t01 >= 1)
            {
                isBallinAir = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ball hit the ground");
            Destroy(Ball.gameObject);
        }
    }
}
