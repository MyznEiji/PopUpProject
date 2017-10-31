using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;


public class PopupGenerator : MonoBehaviour {
	static public GameObject popUps,firstQuestion,secondQuestion,canvasStatic;
	static public Button yesButton,noButton,aButton,bButton,cButton;
	// Use this for initialization
	public static　void SetPopUp(GameObject popUp_a)
	{
		//各ボタンをstaticな変数を代入
		SetUpButton(popUp_a);

		Observable.FromCoroutine<string[]>(observer => WaitButtonClick(observer))
				　.Subscribe(
								result => print(result[0] + "と" + result[1] + "を選択されました。"),
								() => print("終了")
							);							
	}			
	public static IEnumerator WaitButtonClick(IObserver<string[]> observer)
	{
		print("Yes,Noどちらかを選択してください。");
		var yes = yesButton.OnClickAsObservable().First().Select(_ => yesButton);
		var no = noButton.OnClickAsObservable().First().Select(_ => noButton);
		var firstAnswer = yes.Amb(no).ToYieldInstruction();
		yield return firstAnswer;
		
		if(firstAnswer.Result.name == "NoButton")
		{
			Destroy(popUps);
			print(firstAnswer.Result.name.Replace("Button","") + "を選択されました。");
			observer.OnCompleted();
			yield break;
		}

		firstQuestion.SetActive(false);
		secondQuestion.SetActive(true);
		print("A,B,Cの中から１つ選択してください。");
		var a = aButton.OnClickAsObservable().First().Select(_ => aButton);
		var b = bButton.OnClickAsObservable().First().Select(_ => bButton);
		var c = cButton.OnClickAsObservable().First().Select(_ => cButton);


		var secondAnser = a.Amb(c).Amb(b).ToYieldInstruction();
		yield return secondAnser;
		Destroy(popUps);
		string[]  result= new string[]{firstAnswer.Result.name.Replace("Button",""), secondAnser.Result.name.Replace("Button","")};
		observer.OnNext(result);
		observer.OnCompleted();
	}

	public static void SetUpButton(GameObject popUp_a)
	{
		popUps = (GameObject)Instantiate(popUp_a);
		canvasStatic = GameObject.Find("Canvas").gameObject;
		firstQuestion = popUps.transform.Find("YesNoPopUp").gameObject;
		secondQuestion = popUps.transform.Find("AorBorCPopUp").gameObject;
		yesButton = popUps.transform.Find("YesNoPopUp/YesButton").gameObject.GetComponent<Button>();
		noButton = popUps.transform.Find("YesNoPopUp/NoButton").gameObject.GetComponent<Button>();
		aButton = popUps.transform.Find("AorBorCPopUp/AButton").gameObject.GetComponent<Button>();
		bButton = popUps.transform.Find("AorBorCPopUp/BButton").gameObject.GetComponent<Button>();
		cButton = popUps.transform.Find("AorBorCPopUp/CButton").gameObject.GetComponent<Button>();
		popUps.transform.SetParent(canvasStatic.transform,false);
	}
}
