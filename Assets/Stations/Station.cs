using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour
{
	protected AudioSource operatingAudio;
	public float workDuration;
	protected abstract void WorkDone(CookControl cook);
	protected virtual bool WorkCanBeStarted(CookControl cook)
	{
		return true;
	}

	public void Awake()
	{
		operatingAudio = GetComponent<AudioSource>();
	}

	public float Operate(CookControl cook)
	{
		if(WorkCanBeStarted(cook))
		{
			if(operatingAudio != null)
			{
				operatingAudio.loop = true;
				operatingAudio.Play();
			}
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
		if(operatingAudio != null)
		{
			operatingAudio.Stop();
		}
		WorkDone(cook);
	}
}
