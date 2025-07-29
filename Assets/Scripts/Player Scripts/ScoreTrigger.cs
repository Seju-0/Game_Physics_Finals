using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public List<EnemyShoot> enemyShoots = new List<EnemyShoot>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            foreach (EnemyShoot enemy in enemyShoots)
            {
                if (enemy != null && enemy.IsBallReadyToScore() && other.gameObject == enemy.GetCurrentBall())
                {
                    Debug.Log("✅ Scored! Ball fully entered.");
                    int points = enemy.GetCurrentBallType() == EnemyShoot.BallType.Orange ? 2 : 3;
                    enemy.scoreManager.AddScore(points);

                    // Prevent duplicate scoring
                    enemy.SetBallReadyToScore(false);
                    break; // stop checking other enemies
                }
            }
        }
    }
}
