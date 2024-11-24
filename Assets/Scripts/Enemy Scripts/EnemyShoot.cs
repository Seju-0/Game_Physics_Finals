using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Transforms")]
    public Transform orangeBallPrefab;
    public Transform blackBallPrefab;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;

    private Transform currentBall;
    public ScoreSystem scoreManager;

    public float shootInterval = 3f;
    private float dribbleHeight = 0.5f;
    private float shootTimer;
    private float arcTime;

    public float missChance = 0.3f;
    private bool isMissedShot;
    private Vector3 missOffset;

    private bool IsBallInHands = true;
    private bool IsBallFlying = false;
    private bool isBallReadyToScore = false;

    private BallType currentBallType;

    void Start()
    {
        shootTimer = shootInterval;
        SpawnNewBall();
    }

    void Update()
    {
        Dribble();
        Shoot();
    }

    private void Dribble()
    {
        if (IsBallInHands && currentBall != null)
        {
            currentBall.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 6) * dribbleHeight);
            Arms.localEulerAngles = Vector3.right * 0;

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootBall();
            }
        }
    }

    private void Shoot()
    {
        if (!IsBallFlying) return;

        arcTime += Time.deltaTime;
        float duration = 0.66f;
        float t01 = arcTime / duration;

        Vector3 startPos = PosOverHead.position;
        Vector3 endPos = Target.position;

        Vector3 targetPosition = isMissedShot ? endPos + missOffset : endPos;
        Vector3 position = Vector3.Lerp(startPos, targetPosition, t01);

        Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * Mathf.PI);
        currentBall.position = position + arc;

        if (t01 >= 1f)
        {
            IsBallFlying = false;
            currentBall.GetComponent<Rigidbody>().isKinematic = false;

            if (isBallReadyToScore)
            {
                Debug.Log($"Ball scored by {currentBallType} ball! Scoring points...");
                int points = currentBallType == BallType.Orange ? 2 : 3;
                scoreManager.AddScore(points);
            }

            isBallReadyToScore = false;
            StartCoroutine(ResetAndSpawnNewBall());
        }
    }

    private void ShootBall()
    {
        currentBall.position = PosOverHead.position;
        Arms.localEulerAngles = Vector3.right * 180;
        transform.LookAt(Target.parent.position);

        IsBallInHands = false;
        IsBallFlying = true;
        arcTime = 0f;
        shootTimer = shootInterval;

        isMissedShot = Random.value < missChance;
        missOffset = MissOffset();
        isBallReadyToScore = !isMissedShot;
    }

    private IEnumerator ResetAndSpawnNewBall()
    {
        yield return new WaitForSeconds(1f);

        if (currentBall != null)
        {
            Destroy(currentBall.gameObject);
        }

        SpawnNewBall();
        IsBallInHands = true;
    }

    public void SpawnNewBall()
    {
        int ballTypeIndex = Random.Range(0, 2);
        currentBall = Instantiate(ballTypeIndex == 0 ? orangeBallPrefab : blackBallPrefab, PosDribble.position, Quaternion.identity);
        currentBallType = ballTypeIndex == 0 ? BallType.Orange : BallType.Black;

        if (currentBall != null)
        {
            currentBall.GetComponent<Rigidbody>().isKinematic = true;
        }

        isBallReadyToScore = false;
    }

    private Vector3 MissOffset()
    {
        Vector3 directionToTarget = (Target.position - PosOverHead.position).normalized;
        float missDistance = Random.Range(1f, 3f);
        return Vector3.Cross(directionToTarget, Vector3.up) * missDistance;
    }

    public void StartShootingBall()
    {
        isBallReadyToScore = true;
        currentBall.GetComponent<Rigidbody>().isKinematic = false;
    }

    public Transform GetCurrentBall() => currentBall;

    public BallType GetCurrentBallType() => currentBallType;

    public bool IsBallReadyToScore() => isBallReadyToScore;

    public void SetBallReadyToScore(bool ready) => isBallReadyToScore = ready;

    public enum BallType
    {
        Orange,
        Black
    }
}
