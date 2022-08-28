using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class CookControl : MonoBehaviour
{
	public float inputThreshold;
	public float speed;
	public GameObject rightHand;
	public float grabRadius;
	public GameObject activeIndicator;
	public GameObject countdown;
	public Image countdownImage;

	private AudioSource wordDoneAudio;
	private Animator animator;
	private CharacterController characterController;
	private GameObject grabbedIngredient;

	private bool isActive = false;

	public static bool isCheering;

	void Awake()
	{
		activeIndicator.SetActive(false);
	}
	void Start()
	{
		wordDoneAudio = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
		characterController = GetComponent<CharacterController>();
		countdownImage.fillMethod = Image.FillMethod.Radial360;
		countdownImage.fillAmount = 0;
		countdown.SetActive(false);
	}


	// Update is called once per frame
	void Update()
	{
		if(isCheering)
		{
			animator.SetTrigger("cheer");
		}
		else if(isActive)
		{
			HandleMovement();
			HandleGrab();
			HandleWork();
			CheckPossibleActions();
		}
	}

	void CheckPossibleActions()
	{
		HUD.Singleton.isWorkPossible = HandleWork(dryRun: true);
		HUD.Singleton.isGrabPossible = HandleGrab(dryRun: true);
	}

	void HandleMovement()
	{
		if(animator.GetBool("isWorking"))
		{
			return;
		}

		float verticalInput = Input.GetAxis("Vertical");
		float horizontalInput = Input.GetAxis("Horizontal");
		if(Mathf.Abs(verticalInput) > inputThreshold || Mathf.Abs(horizontalInput) > inputThreshold)
		{
			animator.SetBool("isRunning", true);
			Vector3 moveVector = new Vector3(verticalInput, 0, -horizontalInput) * speed * Time.deltaTime;
			characterController.Move(moveVector);
			transform.LookAt(this.transform.position + moveVector);
		}
		else
		{
			animator.SetBool("isRunning", false);
		}
	}

	bool HandleGrab(bool dryRun = false)
	{
		if(!dryRun && !Input.GetButtonDown("Grab"))
		{
			return false;
		}
		if(grabbedIngredient == null)
		{
			GameObject closestIngredient = ClosestInLayer(6); //layer 6 is Ingredients
			if(closestIngredient == null)
			{
				return false;
			}

			if(closestIngredient.tag == "Untagged")
			{
				return false;
			}
			if(closestIngredient.tag != tag)
			{
				if(!dryRun)
				{
					HUD.Singleton.DisplayText("You can't grab that !\n" + HUD.UppercaseFirst(HUD.PrettyName(closestIngredient.name)) + " is " + closestIngredient.tag.ToLower() + " but cook is " + tag.ToLower());
				}
				return false;
			}
			if(!dryRun)
			{
				Station activatedStation = closestIngredient.transform.parent.GetComponent<Station>();
				if(activatedStation != null)
				{
					Debug.Log("HEEEEEEEEEEEEEEEERE");
					activatedStation.Operate(this);
				}
				GrabIngredient(closestIngredient);
			}
			return true;
		}
		else
		{
			GameObject plate = ClosestInLayer(7); //layer 7 is Plates
			if(plate == null)
			{
				return false;
			}
			else
			{
				Recipe recipe = plate.GetComponent<Recipe>();
				if(recipe != null)
				{
					if(!dryRun)
					{
						bool ingredientwasAdded = recipe.AddIngredient(grabbedIngredient);
						if(ingredientwasAdded)
						{
							UnlinkGrabbedIngredient();
						}
					}
					return true;
				}
				else
				{
					if(plate.transform.childCount > 0)
					{
						if(!dryRun)
						{
							HUD.Singleton.DisplayText("There is already something there");
						}
						return false;
					}
					else
					{
						if(!dryRun)
						{
							grabbedIngredient.transform.parent = plate.transform;
							grabbedIngredient.transform.localPosition = Vector3.zero;
							grabbedIngredient.transform.localRotation = Quaternion.identity;
							UnlinkGrabbedIngredient();
						}
						return true;
					}
				}
			}
		}

	}
	public void GrabIngredient(GameObject ingredient)
	{
		animator.SetBool("isCarrying", true);
		grabbedIngredient = ingredient;
		grabbedIngredient.transform.parent = rightHand.transform;
		grabbedIngredient.transform.localPosition = Vector3.zero;
		grabbedIngredient.transform.localRotation = Quaternion.identity;
	}

	public GameObject GetGrabbedIngredient()
	{
		return grabbedIngredient;
	}

	// Caller must handle what happens to the ingredient before calling UnlinkGrabbedIngredient
	public void UnlinkGrabbedIngredient()
	{
		grabbedIngredient = null;
		animator.SetBool("isCarrying", false);
	}


	bool HandleWork(bool dryRun = false)
	{
		if(!dryRun && !Input.GetButtonDown("Work"))
		{
			return false;
		}
		GameObject station = ClosestInLayer(8);
		if(station == null)
		{
			return false;
		}
		if(station.tag != tag && station.tag != "Untagged")
		{
			if(!dryRun)
			{
				HUD.Singleton.DisplayText("You can't do this task !\n" + HUD.UppercaseFirst(HUD.PrettyName(station.name)) + " is " + station.tag.ToLower() + " but cook is " + tag.ToLower());
			}
			return false;
		}

		if(dryRun)
		{
			return station.GetComponent<Station>().WorkCanBeStarted(this);
		}
		else
		{
			float workTime = station.GetComponent<Station>().Operate(this);
			if(workTime > 0f)
			{
				animator.SetBool("isRunning", false);
				animator.SetBool("isWorking", true);
				StartCoroutine(WaitForWork(workTime));
			}
			return true;

		}

	}

	IEnumerator WaitForWork(float waitTime)
	{
		countdown.SetActive(true);
		float startTime = Time.time;
		while(Time.time < startTime + waitTime)
		{
			yield return 0; // wait 1 frame
			countdownImage.fillAmount = (Time.time - startTime) / (waitTime);
		}
		wordDoneAudio.Play();
		countdownImage.fillAmount = 0;
		countdown.SetActive(false);
		animator.SetBool("isWorking", false);
	}

	public GameObject ClosestInLayer(int layerIndex)
	{
		Collider[] hits = Physics.OverlapSphere(transform.position, grabRadius, 1 << layerIndex); // 1<<layerIndex means layer number layerIndex. Great API, Unity !
		float minDistance = Mathf.Infinity;
		GameObject closestObject = null;
		foreach(Collider hit in hits)
		{
			float distance = Vector3.Distance(hit.gameObject.transform.position, transform.position);
			if(distance < minDistance)
			{
				minDistance = distance;
				closestObject = hit.gameObject;
			}
		}
		return closestObject;
	}

	public void SetActive()
	{
		activeIndicator.SetActive(true);
		isActive = true;
	}

	public void SetInactive()
	{
		activeIndicator.SetActive(false);
		if(animator != null)
			animator.SetBool("isRunning", false);
		isActive = false;
	}
}
