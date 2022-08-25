using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderBox : MonoBehaviour
{
	public List<GameObject> Recipes;
	private int recipeIndex = 0;
	public Transform recipePosition;
	public Transform showCaseRecipePosition;

	public Camera ShowcaseCamera;
	public RawImage ShowcaseProjection;
	void Start()
	{
		ShowcaseCamera.targetTexture = new RenderTexture(512, 512, 16);
		SpawnRecipe();
	}

	void Update()
	{
		if(recipePosition.childCount == 0)
		{
			SpawnRecipe();
		}
	}

	void SpawnRecipe()
	{
		GameObject recipePrefab = Recipes[recipeIndex];
		recipeIndex++;
		if(recipeIndex >= Recipes.Count)
		{
			recipeIndex = 0;
		}
		//HUD.Singleton.SetOrder(recipePrefab);
		if(showCaseRecipePosition.childCount > 0)
		{
			// If we "Destroy", the previous recipe is not yet deleted when we take the next screenshot
			DestroyImmediate(showCaseRecipePosition.GetChild(0).gameObject);
		}
		Instantiate(recipePrefab, recipePosition);
		GameObject recipeShowcase = Instantiate(recipePrefab, showCaseRecipePosition);
		recipeShowcase.GetComponent<Recipe>().FillRecipe();
		TakeScreenshot();
	}

	private void TakeScreenshot()
	{
		// The Render Texture in RenderTexture.active is the one
		// that will be read by ReadPixels.
		var currentRT = RenderTexture.active;
		RenderTexture.active = ShowcaseCamera.targetTexture;
		// Render the camera's view.
		ShowcaseCamera.Render();

		// Make a new texture and read the active Render Texture into it.
		Texture2D image = new Texture2D(ShowcaseCamera.targetTexture.width, ShowcaseCamera.targetTexture.height);
		image.ReadPixels(new Rect(0, 0, ShowcaseCamera.targetTexture.width, ShowcaseCamera.targetTexture.height), 0, 0);
		image.Apply();

		// Replace the original active Render Texture.
		RenderTexture.active = currentRT;

		ShowcaseProjection.texture = image;
	}
}
