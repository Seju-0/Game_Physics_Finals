using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArea : MonoBehaviour
{
    public ScoreSystem scoreManager;
    public List<EnemyShoot> enemies; 

    private void OnTriggerEnter(Collider other)
    {
        foreach (EnemyShoot enemyShoot in enemies)
        {
            if (other.CompareTag("Ball") && enemyShoot != null && other.transform == enemyShoot.GetCurrentBall() && enemyShoot.IsBallReadyToScore())
            {
                if (enemyShoot.GetCurrentBallType() == EnemyShoot.BallType.Orange)
                {
                    scoreManager.AddScore(2);
                }
                else if (enemyShoot.GetCurrentBallType() == EnemyShoot.BallType.Black)
                {
                    scoreManager.AddScore(3);
                }

                enemyShoot.SetBallReadyToScore(false);

                break;
            }
        }
    }
}