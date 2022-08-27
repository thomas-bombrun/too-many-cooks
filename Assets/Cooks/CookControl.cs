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
		GameObject closestIngredient = ClosestInLayer(6);
		HUD.Singleton.isGrabPossible = closestIngredient != null && grabbedIngredient == null;
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

	void HandleGrab()
	{
		if(!Input.GetButtonDown("Grab"))
		{
			return;
		}
		if(grabbedIngredient == null)
		{
			GameObject closestIngredient = ClosestInLayer(6); //layer 6 is Ingredients
			if(closestIngredient != null)
			{
				if(closestIngredient.tag == "Untagged")
				{
					return;
				}
				if(closestIngredient.tag != tag)
				{
					HUD.Singleton.DisplayText("You can't grab that !\n" + closestIngredient.name + " is " + closestIngredient.tag + " but cook is " + tag);
					return;
				}
				else
				{
					Station activatedStation = closestIngredient.transform.parent.GetComponent<Station>();
					if (activatedStation != null)
						activatedStation.Operate(this);
					GrabIngredient(closestIngredient);
				}
			}
		}
		else
		{
			GameObject plate = ClosestInLayer(7); //layer 7 is Plates
			if(plate != null)
			{
				Recipe recipe = plate.GetComponent<Recipe>();
				if(recipe != null)
				{
					bool ingredientwasAdded = recipe.AddIngredient(grabbedIngredient);
					if(ingredientwasAdded)
					{
						UnlinkGrabbedIngredient();
					}
				}
				else
				{
					if(plate.transform.childCount > 0)
					{
						HUD.Singleton.DisplayText("Plate is already full !");
					}
					else
					{
						grabbedIngredient.transform.parent = plate.transform;
						grabbedIngredient.transform.localPosition = Vector3.zero;
						grabbedIngredient.transform.localRotation = Quaternion.identity;
						UnlinkGrabbedIngredient();
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
			Debug.Log("didn't find a station to work");
			return false;
		}
		if(station.tag != tag && station.tag != "Untagged")
		{
			if(!dryRun)
			{
				HUD.Singleton.DisplayText("You can't do this task !\n" + station.name + " is " + station.tag + " but cook is " + tag);
			}
			return false;
		}
		if(dryRun)
		{
			return true;
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
