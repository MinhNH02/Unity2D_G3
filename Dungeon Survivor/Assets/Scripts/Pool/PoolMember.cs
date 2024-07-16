using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMember : MonoBehaviour
{
    ObjectPool pool;
    public void Set(ObjectPool pool)
    {
        this.pool = pool;
        GetComponent<IPoolMember>().SetPoolMember(this);
    }
    public void ReturnToPool()
    {
        pool.ReturnToPool(gameObject);
    }
}
