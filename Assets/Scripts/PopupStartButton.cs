using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class PopupStartButton : MonoBehaviour {
	[SerializeField] private Button startButton;
	[SerializeField] private GameObject popups;
	void Start () 
	{
		startButton.OnClickAsObservable()
				   .Subscribe(_ => PopupGenerator.SetPopUp(popups));
	}
}
