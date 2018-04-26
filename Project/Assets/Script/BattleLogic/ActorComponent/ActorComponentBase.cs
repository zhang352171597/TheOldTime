using UnityEngine;
using System.Collections;

public class ActorComponentBase{

    ActorBase _actor;
    public ActorBase actor
    {
        get { return _actor; }
        set { _actor = value; }
    }

    public virtual void OnAwake()
    {

    }

    public virtual void OnStart()
    {

    }

    public virtual void OnDestroy()
    {

    }

	public virtual void LogicUpdate(float dt)
    {

    }

	protected T GetActorComponent<T>() where T : ActorComponentBase
	{
		return actor.GetActorComponent<T> ();
	}
    


}
