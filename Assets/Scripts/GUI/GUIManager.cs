using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
#region Event Handlers

	public void OnQuitButtonClicked()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			Application.Quit();
		}
	}

#endregion Event Handlers

}
