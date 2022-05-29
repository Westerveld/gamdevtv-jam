using System.Collections;
using System.Collections.Generic;
using Maze;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Final : GameManager
{
	public MazeController player;

	public Volume finalVolume;
	public MinMax lensIntensity;
	public LensDistortion distortion;
	public override void StartGame(float value1 = 0, float value2 = 0)
	{
		player.Setup(0.1f,0f);
		finalVolume.profile.TryGet<LensDistortion>(out distortion);
		StartCoroutine(AdjustPostProcessing());
	}

	IEnumerator AdjustPostProcessing()
	{
		while (true)
		{
			distortion.intensity.value = Mathf.LerpUnclamped(lensIntensity.min, lensIntensity.max, Mathf.Sin(Time.time));
				yield return null;
		}
	}
}
