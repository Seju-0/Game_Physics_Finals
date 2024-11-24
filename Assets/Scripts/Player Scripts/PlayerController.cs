using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header ("Player Movement")]
    public float MoveSpeed = 15f;
    public float dashSpeed = 20f;
    public float dashDuration = 1f;

    //boundaries
    public Vector2 xRange = new Vector2(-7, 7); 
    public Vector2 zRange = new Vector2(-2, 8);

    private Vector3 moveDirection;


    private float cooldownTime = 3f;
    public float Countdown = 60f;

    private bool isDashing = false;
    private bool isCooldown = false;
    private bool isGameOver = false;
    private bool enemiesSpawned = false;

    [Header("Texts")]
    public TextMeshProUGUI CDtext;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;

    private Rigidbody rb;

    public EnemySpawner EnemySpawner;
    public ScoreSystem ScoreSystem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (CDtext != null)
        {
            CDtext.text = "";
        }
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && !isCooldown)
        {
            StartCoroutine(Dash());
        }

        //cooldown timer

        if (isCooldown)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0f)
            {
                isCooldown = false;
                cooldownTime = 3f;
            }
        }

        if (CDtext != null)
        {
            CDtext.text = "Dash Cooldown: " + Mathf.Max(cooldownTime, 0f).ToString("F1") + "s";
        }

        //countdown timer

        if (Countdown > 0)
        {
            Countdown -= Time.deltaTime;

            if (!isGameOver && ScoreSystem.CurrentScore >= 50)
            {
                EndGame("You Lose");
            }
        }
        else
        {
            Countdown = 0;

            if (!isGameOver)
            {
                if (ScoreSystem.CurrentScore < 50)
                {
                    EndGame("You Win!");
                }
                else
                {
                    EndGame("You Lose");
                }
            }
        }

        timerText.text = "Time Left: " + Mathf.Max(Countdown, 0).ToString("F1") + "s";

        if (!enemiesSpawned && Countdown <= 10f)
        {
            enemiesSpawned = true;
            EnemySpawner.SpawnEnemy();
        }

        float currentSpeed = isDashing ? dashSpeed : MoveSpeed;
        Vector3 newPosition = transform.position + moveDirection * currentSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, xRange.x, xRange.y);
        newPosition.z = Mathf.Clamp(newPosition.z, zRange.x, zRange.y);

        rb.MovePosition(newPosition);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        isCooldown = true;
    }

    private void EndGame(string message)
    {
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(false);
        CDtext.gameObject.SetActive(false);
        ScoreSystem.scoreText.gameObject.SetActive(false);
        gameOverText.text = message;

        Time.timeScale = 0f;
    }
}