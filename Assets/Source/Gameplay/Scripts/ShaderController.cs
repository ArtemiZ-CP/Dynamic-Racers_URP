using UnityEngine;

public class ShaderController : MonoBehaviour
{
	[SerializeField] private Gradient _gradient;
	[SerializeField] private Material _material;
	[SerializeField] private int _textureWidth = 128;

	private void Update()
	{
		_material.SetTexture("_GradientTexture", GenerateGradientTexture(_gradient, _textureWidth));
	}

	private Texture2D GenerateGradientTexture(Gradient gradient, int width)
	{
		Texture2D texture = new(width, 1);
		for (int i = 0; i < width; i++)
		{
			float t = (float)i / (width - 1);
			texture.SetPixel(i, 0, gradient.Evaluate(t));
		}
		texture.Apply();
		return texture;
	}
}
