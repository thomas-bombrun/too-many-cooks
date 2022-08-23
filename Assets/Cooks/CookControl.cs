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
		if (isActive)
		{
			HandleControl();
			HandleControlGrab();
		}
    }

    void HandleControl()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(verticalInput) > inputThreshold || Mathf.Abs(horizontalInput) > inputThreshold)
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

    void HandleControlGrab()
    {
        if (!Input.GetButtonDown("Fire1"))
        {
            return;
        }
        Debug.Log(grabbedIngredient);
        if (grabbedIngredient == null)
        {
            Collider[] ingredients = Physics.OverlapSphere(transform.position, grabRadius, 1 << 6); // 1<<6 means layer number 6, layer number 6 is Ingredients. Great API, Unity !
            foreach (Collider ingredient in ingredients)
            {
                grabbedIngredient = ingredient.gameObject;
                grabbedIngredient.transform.parent = rightHand.transform;
                grabbedIngredient.transform.localPosition = Vector3.zero;
                grabbedIngredient.transform.rotation = Quaternion.identity;
                return;
            }
        }
        else
        {
            Collider[] plates = Physics.OverlapSphere(transform.position, grabRadius, 1 << 7); // 1<<7 means layer number 7, layer number 7 is Plates. Great API, Unity !
            foreach (Collider plate in plates)
            {
                grabbedIngredient.transform.parent = plate.transform;
                grabbedIngredient.transform.localPosition = Vector3.zero;
                grabbedIngredient.transform.rotation = Quaternion.identity;
                grabbedIngredient = null;
                return;
            }
        }
    }

	public void SetActive()
	{
		isActive = true;
	}

	public void SetInactive()
	{
		if (animator != null)
			animator.SetBool("isRunning", false);
		isActive = false;
	}
}
