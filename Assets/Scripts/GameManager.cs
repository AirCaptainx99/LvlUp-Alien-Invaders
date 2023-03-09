using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Enemy Movement")]
    public float movingSpeed = 1f;
    public GameObject container;
    public Player player;
    public float horizontalLimit = 3f;
    public float verticalMove = 0.1f;
    public Vector2 targetPosition;
    private float movingDirection = 1;
    private bool isNextDownDirection = false;

    [Header("Enemy Attack")]
    public float shootingInterval = 3f;
    public float shootingSpeed = 2f;
    private float shootingTimer;

    [Header("Score Manager")]
    public int score;
    public TextMeshProUGUI scoreText;

    [Header("Game Over & Victory Manager")]
    public GameObject timeStopPanel;
    private List<GameObject> panels;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        shootingTimer = shootingInterval;
        targetPosition = new Vector2(horizontalLimit, container.transform.position.y);
        UpdateScore();
        panels = timeStopPanel.GetComponent<TimeStopPanel>().panels;
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.isActiveAndEnabled)
            {
                EnemyMove();
                EnemyShoot();
            }
        }

        if (EnemyContainer.Instance.EnemyCount == 0)
        {
            Victory();
        }

        print(Time.timeScale);
    }

    private void EnemyShoot()
    {
        shootingTimer -= Time.deltaTime;
        if (EnemyContainer.Instance.EnemyCount > 0 && shootingTimer <= 0)
        {
            shootingTimer = shootingInterval;

            Enemy[] enemies = GetComponentsInChildren<Enemy>();
            Enemy randomEnemy = enemies[Random.Range(0, enemies.Length - 1)];

            GameObject enemyLaser = ObjectPool.Instance.GetGameObjectFromPool("Enemy Laser", 3f).gameObject;
            enemyLaser.transform.position = randomEnemy.transform.position;
            enemyLaser.GetComponent<Rigidbody2D>().velocity = Vector2.down * shootingSpeed;
        }
    }

    private void EnemyMove()
    {
        if (isNextDownDirection) // Go down
        {
            EnemyMoveDown();
            if (container.transform.position.y <= targetPosition.y)
            {
                isNextDownDirection = false;

                // Set the next targetPosition
                // Check whether go to the left or right based on movingDirection
                targetPosition.x = (movingDirection > 0) ? horizontalLimit : -horizontalLimit;
            }
        }
        else if (movingDirection > 0) // Go right
        {
            EnemyMoveRight();
            if (EnemyContainer.Instance.RightmostPosition >= targetPosition.x)
            {
                isNextDownDirection = true;
                movingDirection *= -1;

                // Set the next targetPosition
                targetPosition.y = container.transform.position.y - verticalMove;
            }
        }
        else if (movingDirection < 0) // Go left
        {
            EnemyMoveLeft();
            if (EnemyContainer.Instance.LeftmostPosition <= targetPosition.x)
            {
                isNextDownDirection = true;
                movingDirection *= -1;

                // Set the next targetPosition
                targetPosition.y = container.transform.position.y - verticalMove;
            }
        }
    }

    private void EnemyMoveLeft()
    {
        container.transform.position = Vector2.MoveTowards(container.transform.position, new Vector2(container.transform.position.x - EnemyContainer.Instance.LeftmostPosition + (-horizontalLimit) - 0.1f, targetPosition.y), Time.deltaTime * movingSpeed);
    }

    private void EnemyMoveRight()
    {
        container.transform.position = Vector2.MoveTowards(container.transform.position, new Vector2(container.transform.position.x - EnemyContainer.Instance.RightmostPosition + (horizontalLimit) + 0.1f, targetPosition.y), Time.deltaTime * movingSpeed);
    }


    private void EnemyMoveDown()
    {
        container.transform.position = Vector2.MoveTowards(container.transform.position, new Vector2(container.transform.position.x, targetPosition.y - 0.1f), Time.deltaTime * movingSpeed);
    }

    public void UpdateScore()
    {
        score = EnemyContainer.Instance.ScoreAccumulated;
        scoreText.SetText("Score: " + score);
    }

    public void Victory()
    {
        scoreText.gameObject.SetActive(false);
        timeStopPanel.SetActive(true);

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        GameObject victoryPanel = panels[1];

        victoryPanel.GetComponent<VictoryScript>().SetScoreText("Score: " + score);
        victoryPanel.SetActive(true);
    }

    public void GameOver(ParticleSystem explosionParticle)
    {
        StartCoroutine(WaitForParticleAndGameOver(explosionParticle));
    }

    private IEnumerator WaitForParticleAndGameOver(ParticleSystem particle)
    {
        yield return new WaitUntil(() => particle.isStopped);
        GameOver();
    }

    public void GameOver()
    {
        scoreText.gameObject.SetActive(false);
        timeStopPanel.SetActive(true);

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        GameObject gameOverPanel = panels[0];

        gameOverPanel.SetActive(true);
    }

    
}
