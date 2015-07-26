using UnityEngine;
using System.Collections;
using GUIEnums;

[RequireComponent (typeof(RectTransform))]
public class HidingPanel : MonoBehaviour 
{
	static private readonly bool DEBUG_LOCAL = true;

#region inspector hooks

	public RectTransform hiddenArea;

	public GameObject hideButtonObject;
	public GameObject showButtonObject;

#endregion inspector hooks

#region private hooks

	private RectTransform rectTransform_;

#endregion private hooks

#region public settings

	public bool startHidden = false;

	public GUIDirection direction = GUIDirection.Down;

	public float duration = 1f;

#endregion public settings

#region Actions
	
	public System.Action preHideAction;
	public System.Action postHideAction;
	
	public System.Action preShowAction;
	public System.Action postShowAction;
	
#endregion Actions
	

#region private data

	private bool isHidden_ = false;
	public bool IsHidden
	{
		get { return isHidden_; }
	}

	private float hiddenDistance_;

	private bool isMoving_ = false;

	private Vector3 visiblePosition_ = Vector3.zero;
	private Vector3 hiddenPosition_ = Vector3.zero;

#endregion private data

#region Event handlers

	public void OnHideButtonClicked()
	{
		HandleHide();
	}

	public void OnShowButtonClicked()
	{
		HandleShow();
	}

	public void HandleHide()
	{
		HandleHideHelper(false);
	}

	public void HandleHideImmediate()
	{
		HandleHideHelper(true);
	}
	
	public void HandleShow()
	{
		HandleShowHelper(false);
	}
	
	public void HandleShowImmediate()
	{
		HandleShowHelper(true);
	}

	private void HandleHideHelper(bool immediate)
	{
		if (isMoving_)
		{
			Debug.LogWarning("HandleHide called on " + gameObject.name + " when already moving");
		}
		else
		{
			StartCoroutine(HideCR(immediate));
		}
	}

	private void HandleShowHelper(bool immediate)
	{
		if (isMoving_)
		{
			Debug.LogWarning("HandleShow called on " + gameObject.name + " when already moving");
		}
		else
		{
			StartCoroutine(ShowCR(immediate));
		}
	}

#endregion Event handlers

#region Movement

	private IEnumerator HideCR(bool immediate)
	{
		isMoving_ = true;

		if (preHideAction != null)
		{
			preHideAction();
		}

		if (!immediate)
		{
			float elapsed = 0f;
			Vector3 start = rectTransform_.position;
			while (elapsed < duration)
			{
				rectTransform_.position = Vector3.Lerp( start, hiddenPosition_, elapsed/duration );
				elapsed += Time.deltaTime;
				yield return null;
			}
		}
		rectTransform_.position = hiddenPosition_;

		hideButtonObject.SetActive(false);
		showButtonObject.SetActive(true);

		isHidden_ = true;
		isMoving_ = false;

		if (postHideAction != null)
		{
			postHideAction();
		}
	}

	private IEnumerator ShowCR(bool immediate)
	{
		isMoving_ = true;
		if (preShowAction != null)
		{
			preShowAction();
		}

		if (!immediate)
		{
			float elapsed = 0f;
			Vector3 start = rectTransform_.position;
			while (elapsed < duration)
			{
				rectTransform_.position = Vector3.Lerp( start, visiblePosition_, elapsed/duration );
				elapsed += Time.deltaTime;
				yield return null;
			}
		}
		rectTransform_.position = visiblePosition_;

		hideButtonObject.SetActive(true);
		showButtonObject.SetActive(false);

		isHidden_ = false;
		isMoving_ = false;

		if (postShowAction != null)
		{
			postShowAction();
		}

	}
	

#endregion Movement

#region SetUp

	void Awake()
	{
		rectTransform_ = GetComponent< RectTransform >();
	}

	void Start () 
	{
		if (hiddenArea == null)
		{
			Debug.LogError("NULL hidden Area");
		}
		else
		{
			visiblePosition_ = hiddenArea.position;
			hiddenPosition_ = hiddenArea.position;
			switch (direction)
			{
				case GUIDirection.Down:
				case GUIDirection.Up:
				{
					hiddenDistance_ = hiddenArea.GetHeight();
					if (direction == GUIDirection.Down)
					{
						hiddenDistance_ *= -1f;
					}
					hiddenPosition_.y = hiddenPosition_.y + hiddenDistance_;
					break;
				}
				case GUIDirection.Left:
				case GUIDirection.Right:
				{
					hiddenDistance_ = hiddenArea.GetWidth();
					if (direction == GUIDirection.Left)
					{
						hiddenDistance_ *= -1f;
					}
					hiddenPosition_.x = hiddenPosition_.x + hiddenDistance_;
					break;
				}
			}
			hideButtonObject.SetActive(true);
			showButtonObject.SetActive(false);
			if (startHidden)
			{
				StartCoroutine(HideCR(true));
			}
			if (DEBUG_LOCAL)
			{
				Debug.Log("HidingPanel '"+gameObject.name+" initialised with Direction="+direction
				          +" Distance="+hiddenDistance_);
			}
		}
	}

#endregion SetUp

}
