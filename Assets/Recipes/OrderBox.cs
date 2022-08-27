using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LevelManager))]
public class OrderBox : MonoBehaviour
{
	public List<GameObject> Recipes;
	private int nextRecipeIndex = 0;
	public Transform recipePosition;
	public Transform showCaseRecipePosition;
	public Camera ShowcaseCamera;
	public RawImage ShowcaseProjection;
	[SerializeField] private AudioClip winClip;

	private AudioSource audioSource;
	void Start()
	{
		ShowcaseCamera.targetTexture = new RenderTexture(512, 512, 16);
		audioSource = GetComponent<AudioSource>();
		SpawnNextRecipe();
	}

	void Update()
	{
	}

	void SpawnNextRecipe()
	{
		GameObject recipePrefab = Recipes[nextRecipeIndex];
		nextRecipeIndex++;

		if(showCaseRecipePosition.childCount > 0)
		{
			DestroyImmediate(showCaseRecipePosition.GetChild(0).gameObject); // If we "Destroy", the previous recipe is not yet deleted when we take the next screenshot
		}
		GameObject recipe = Instantiate(recipePrefab, recipePosition);
		recipe.GetComponent<Recipe>().recipeDone.AddListener(RecipeIsDone);
		audioSource.Play();
		// Showcase
		GameObject recipeShowcase = Instantiate(recipePrefab, showCaseRecipePosition);
		recipeShowcase.GetComponent<Recipe>().FillRecipe();
		TakeScreenshot();
	}

	void RecipeIsDone()
	{
		if(nextRecipeIndex >= Recipes.Count)
		{
			CookControl.isCheering = true;
			audioSource.PlayOneShot(winClip);
			Countdown.paused = true;
			Invoke("LoadNextLevel", winClip.length);
		}
		else
		{
			SpawnNextRecipe();
		}
	}

	void LoadNextLevel()
	{
		Countdown.paused = false;
		CookControl.isCheering = false;
		LevelManager.LoadNextLevel();
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
