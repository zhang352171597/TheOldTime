using UnityEngine;
using System.Collections;

public class DespawnByTime : MonoBehaviour {

	public void Begin(float lifeTime)
    {
        Invoke("_Despawn", lifeTime);
    }

    void _Despawn()
    {
        ObjManager.Instance.Despawn(gameObject);
    }

}
