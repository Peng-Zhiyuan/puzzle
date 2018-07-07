using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour 
{

	void Start () {
		UnityEngine.Random.InitState(DateTime.UtcNow.Second);
		Puzzle.Instance.Init();
		var texture = Resources.Load<Texture2D>("mapbg");
		Puzzle.Instance.StartPuzzle(texture, 200);
	}

}
