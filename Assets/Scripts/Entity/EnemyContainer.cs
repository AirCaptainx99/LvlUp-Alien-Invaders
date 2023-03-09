using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    private List<Enemy> enemies;
    private Transform leftmostEnemyPos, rightmostEnemyPos;
    public int EnemyCount { get; private set; }
    public float LeftmostPosition { get { return (leftmostEnemyPos != null) ? leftmostEnemyPos.position.x : 0f; } }
    public float RightmostPosition { get { return (rightmostEnemyPos != null) ? rightmostEnemyPos.position.x : 0f; } }

    public int ScoreAccumulated { get; private set; }

    public static EnemyContainer Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        enemies = new List<Enemy>(GetComponentsInChildren<Enemy>());
        EnemyCount = enemies.Count;

        leftmostEnemyPos = rightmostEnemyPos = enemies[0].transform;

        foreach (Enemy enemy in enemies)
        {
            if (enemy.transform.position.x < leftmostEnemyPos.position.x)
            {
                leftmostEnemyPos = enemy.transform;
            }
            if (enemy.transform.position.x > rightmostEnemyPos.position.x)
            {
                rightmostEnemyPos = enemy.transform;
            }
        }
    }

    public void EnemyDie(Enemy _enemy)
    {
        enemies.Remove(_enemy);
        EnemyCount--;
        ScoreAccumulated += _enemy.scoreValue;

        GameManager.Instance.UpdateScore();

        if (EnemyCount <= 0)
        {
            return;
        }

        if (leftmostEnemyPos == _enemy.transform || rightmostEnemyPos == _enemy.transform)
        {
            leftmostEnemyPos = rightmostEnemyPos = enemies[0].transform;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.transform.position.x < leftmostEnemyPos.position.x)
                {
                    leftmostEnemyPos = enemy.transform;
                }
                if (enemy.transform.position.x > rightmostEnemyPos.position.x)
                {
                    rightmostEnemyPos = enemy.transform;
                }
            }
        }
    }
}
