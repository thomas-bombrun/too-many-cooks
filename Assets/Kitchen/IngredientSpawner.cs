using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
	public GameObject ingredientPrefab;


	void Start()
	{
		InvokeRepeating("SpawnIngredient", 0f, 5f);
	}



	// Update is called once per frame
	void SpawnIngredient()
	{
		if(transform.childCount == 0)
		{
			GameObject ingredient = Instantiate(ingredientPrefab, transform);
			ingredient.name = ingredientPrefab.name;
		}
	}
}
