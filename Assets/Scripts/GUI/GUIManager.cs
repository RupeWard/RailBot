using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
#region inspector hooks

	public GameObject[] startActiveObjects = new GameObject[0];
	public GameObject[] startInactiveObjects = new GameObject[0];

#endregion inspector hooks

#region Event Handlers

	public void OnQuitButtonClicked()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			Application.Quit();
		}
	}

#endregion Event Handlers

#region SetUp

	public void Awake()
	{
		foreach (GameObject go in startActiveObjects)
		{
			go.SetActive(true);
		}
		foreach (GameObject go in startInactiveObjects)
		{
			go.SetActive(false);
		}
	}

#endregion SetUp

}
