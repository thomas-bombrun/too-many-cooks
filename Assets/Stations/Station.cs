using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour
{
	public float workDuration;
	protected abstract void WorkDone(CookControl cook);

	public float Operate(CookControl cook)
	{
		StartCoroutine(WaitForWork(cook));
		return workDuration;
	}

	private IEnumerator WaitForWork(CookControl cook)
	{
		yield return new WaitForSeconds(workDuration);
		WorkDone(cook);
	}
}
