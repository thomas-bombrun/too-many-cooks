using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour
{
	public float workDuration;
	protected abstract void WorkDone(CookControl cook);
	protected virtual bool WorkCanBeStarted(CookControl cook)
	{
		return true;
	}

	public float Operate(CookControl cook)
	{
		if(WorkCanBeStarted(cook))
		{
			StartCoroutine(WaitForWork(cook));
			return workDuration;
		}
		else
		{
			Debug.Log("work could not be started");
			// TODO Return complex type of workWasStarted/worktime ?
			return 0f;
		}
	}

	private IEnumerator WaitForWork(CookControl cook)
	{
		yield return new WaitForSeconds(workDuration);
		WorkDone(cook);
	}
}
