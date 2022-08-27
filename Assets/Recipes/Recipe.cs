using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Recipe : MonoBehaviour
{
	public List<GameObject> PrefabIngredientsInOrder;
	public List<Transform> SpotsForIngredients;

	[SerializeField] private int currentStep;

	[HideInInspector] public UnityEvent recipeDone;
	[HideInInspector] public UnityEvent<int> addIngredient;
	private AudioSource ingredientAddedAudio;

	public float flySpeed;
	private bool isFlying;

	void Update()
	{
		if(isFlying)
		{
			if(transform.position.y > 10)
			{
				Destroy(this.gameObject);
			}
			transform.position += new Vector3(0, Time.deltaTime * flySpeed, 0);
		}
	}

	void Awake()
	{
		ingredientAddedAudio = GetComponent<AudioSource>();
	}

	///<returns>Wheter the ingredient was accepted or not.</returns>
	public bool AddIngredient(GameObject ingredient)
	{
		if(ingredient.name == PrefabIngredientsInOrder[currentStep].name)
		{
			ingredient.transform.SetParent(SpotsForIngredients[currentStep]);
			ingredient.transform.localPosition = Vector3.zero;
			ingredient.transform.localRotation = Quaternion.identity;
			ingredient.tag = "Untagged";
			currentStep++;
			if(currentStep == SpotsForIngredients.Count)
			{
				recipeDone.Invoke();
				HUD.Singleton.AddScore();
				isFlying = true;
			}
			else
			{
				ingredientAddedAudio.Play();
				addIngredient.Invoke(currentStep);
			}
			return true;
		}
		else
		{
			HUD.Singleton.DisplayText("Wrong ingredient !\nExpected " + PrefabIngredientsInOrder[currentStep].name + " but got " + ingredient.name);
			return false;
		}
	}

	public void FillRecipe()
	{
		Clear();
		for(int i = 0; i < PrefabIngredientsInOrder.Count; i++)
		{
			Instantiate(PrefabIngredientsInOrder[i], SpotsForIngredients[i]);
		}
	}

	public void Clear()
	{
		foreach(Transform ingredientSpot in SpotsForIngredients)
		{
			for(int i = 0; i < ingredientSpot.childCount; i++)
			{
				SafeDestroy(ingredientSpot.GetChild(i).gameObject);
			}
		}
	}

	public static T SafeDestroy<T>(T obj) where T : Object
	{
		if(Application.isEditor)
			Object.DestroyImmediate(obj);
		else
			Object.Destroy(obj);

		return null;
	}

}
