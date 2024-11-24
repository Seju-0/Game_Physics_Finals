using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Michael;
    public GameObject Derrick;
    public Transform MichaelSpawnPoint;
    public Transform DerrickSpawnPoint;
    public Transform target;
    public ScoreSystem scoreManager;

    public void SpawnEnemy()
    {
        GameObject enemy1 = Instantiate(Michael, MichaelSpawnPoint.position, MichaelSpawnPoint.rotation);
        InitializeEnemy(enemy1);

        GameObject enemy2 = Instantiate(Derrick, DerrickSpawnPoint.position, DerrickSpawnPoint.rotation);
        InitializeEnemy(enemy2);
    }

    private void InitializeEnemy(GameObject enemy)
    {
        EnemyShoot enemyShoot = enemy.GetComponent<EnemyShoot>();
        if (enemyShoot != null)
        {
            enemyShoot.Target = target;
            enemyShoot.scoreManager = scoreManager;
        }
    }
}
