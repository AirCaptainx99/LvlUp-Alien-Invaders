using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public int scoreValue;
    protected override GameObject OnDie()
    {
        float powerUpDropChance = Random.Range(0f, 1f);
        if (powerUpDropChance <= 0.7f)
        {
            GameObject powerUp = ObjectPool.Instance.GetGameObjectFromPool("Power Up", 5f).gameObject;
            powerUp.transform.position = gameObject.transform.position;
        }

        base.OnDie();
        EnemyContainer.Instance.EnemyDie(this);

        return null;
    }
}
