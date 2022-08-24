using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class CookControl : MonoBehaviour
{
	public float inputThreshold;
	public float speed;
	public GameObject rightHand;
	public float grabRadius;
	public GameObject activeIndicator;

	private Animator animator;
	private CharacterController characterController;
	private GameObject grabbedIngredient;

	private bool isActive = false;
	void Start()
	{
		animator = GetComponent<Animator>();
		characterController = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		if(isActive)
		{
			HandleMovement();
			HandleGrab();
			HandleWork();
		}
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
			Vector3 moveVector = new Vector3(verticalInput, 0, -horizontalInput) * speed;
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
		if(!Input.GetButtonDown("Fire1"))
		{
			return;
		}
		if(grabbedIngredient == null)
		{
			GameObject closestIngredient = ClosestInLayer(6); //layer 6 is Ingredients
			if(closestIngredient != null)
			{
				if(closestIngredient.tag != tag)
				{
					if(closestIngredient.tag != "Untagged")
					{
						HUD.Singleton.DisplayText("You can't grab that ! Ingredient is " + closestIngredient.tag + " but cook is " + tag);
					}
				}
				else
				{
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
						animator.SetBool("isCarrying", false);
						grabbedIngredient = null;
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
						animator.SetBool("isCarrying", false);
						grabbedIngredient.transform.parent = plate.transform;
						grabbedIngredient.transform.localPosition = Vector3.zero;
						grabbedIngredient.transform.localRotation = Quaternion.identity;
						grabbedIngredient = null;
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

	void HandleWork()
	{
		if(!Input.GetButtonDown("Work"))
		{
			return;
		}
		if(grabbedIngredient != null)
		{
			return;
		}
		GameObject machine = ClosestInLayer(8);
		if(machine == null)
		{
			return;
		}
		if(machine.tag != tag)
		{
			HUD.Singleton.DisplayText("You can't do this task ! " + machine.name + " is " + machine.tag + " but cook is " + tag);
			return;
		}
		animator.SetBool("isRunning", false);
		animator.SetBool("isWorking", true);
		float workTime = machine.GetComponent<Station>().Operate(this);
		Invoke("WorkDone", workTime);
	}

	void WorkDone()
	{
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
		activeIndicator.active = true;
		isActive = true;
	}

	public void SetInactive()
	{
		activeIndicator.active = false;
		if(animator != null)
			animator.SetBool("isRunning", false);
		isActive = false;
	}
}
