using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
	private const int MAX_OCTAVES = 10;
	private static readonly float Frequency = 0.1f;
	private static readonly float Amplitude = 0.5f;


	public static float Generate(Vector2 coords, float scale, float seed)
	{
		float noise = 0f;
		float frequency = Frequency;
		float amplitude = Amplitude;

		for(int i = 0; i < MAX_OCTAVES; i++)
		{
			noise += Mathf.PerlinNoise(frequency * ((coords.x)/scale), frequency * ((coords.y)/scale)) * amplitude;

			frequency *= 2f;
			amplitude /= 2f;
		}
		
		//return noise/MAX_OCTAVES;
		return Mathf.Pow(noise, -2f);
		

	}
}
