using System;
using System.Collections;
using System.Collections.Generic;
using Maze;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Final : GameManager
{
	public MazeController player;

	public Volume finalVolume;
	public MinMax lensIntensity;
	public LensDistortion distortion;
	public Bloom bloom;
	private float distToPlayer;
	private float requiredBloom = 25f;
	public AnimationCurve bloomCurve;
	public Transform door;
	public override void StartGame(float value1 = 0, float value2 = 0)
	{
		player.Setup(this, 0.1f,0f);
		finalVolume.profile.TryGet<LensDistortion>(out distortion);
		finalVolume.profile.TryGet(out bloom);
		StartCoroutine(AdjustPostProcessing());
		AudioManager.instance?.PlayFinal();
	}

	private void FixedUpdate()
	{
		if (bloom != null)
		{
			distToPlayer = Vector3.Distance(door.position, player.transform.position);
			bloom.intensity.value = bloomCurve.Evaluate(distToPlayer);
			if (distToPlayer < 1.6f)
			{
				EndGame();
			}
		}
	}

	IEnumerator AdjustPostProcessing()
	{
		while (true)
		{
			distortion.intensity.value = Mathf.LerpUnclamped(lensIntensity.min, lensIntensity.max, Mathf.Sin(Time.time));
			yield return null;
		}
	}

	public override void EndGame()
	{
		SceneManager.LoadScene("Winner");
	}
}
