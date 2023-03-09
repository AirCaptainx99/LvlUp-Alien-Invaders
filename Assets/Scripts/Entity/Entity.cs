using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public List<string> triggerTag;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerTag.Contains(collision.tag))
        {
            ObjectPool.Instance.DestroyGameObject(this.gameObject);
            ObjectPool.Instance.DestroyGameObject(collision.gameObject);
            
            OnDie();
        }
    }

    protected virtual GameObject OnDie()
    {
        GameObject explosion = ObjectPool.Instance.GetGameObjectFromPool("Explosion").gameObject;
        explosion.transform.position = transform.position;

        return explosion;
    }
}
