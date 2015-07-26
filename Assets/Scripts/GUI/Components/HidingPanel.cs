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
	public bool IsMoving
	{
		get { return isMoving_; }
	}

	private Vector3 visiblePosition_ = Vector3.zero;
	private Vector3 hiddenPosition_ = Vector3.zero;

	private Coroutine movingCoroutine_ = null;

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
		if (movingCoroutine_ != null)
		{
			StopCoroutine(movingCoroutine_);
		}
		movingCoroutine_ = StartCoroutine(HideCR(immediate));
	}

	private void HandleShowHelper(bool immediate)
	{
		if (movingCoroutine_ != null)
		{
			StopCoroutine(movingCoroutine_);
		}
		movingCoroutine_ = StartCoroutine(ShowCR(immediate));
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

		hideButtonObject.SetActive(false);
		showButtonObject.SetActive(true);
		
		if (!immediate)
		{
			float elapsed = 0f;
			Vector3 start = hiddenArea.position;
			while (elapsed < duration)
			{
				hiddenArea.position = Vector3.Lerp( start, hiddenPosition_, elapsed/duration );
				elapsed += Time.deltaTime;
				yield return null;
			}
		}
		hiddenArea.position = hiddenPosition_;

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

		hideButtonObject.SetActive(true);
		showButtonObject.SetActive(false);
		
		if (!immediate)
		{
			float elapsed = 0f;
			Vector3 start = hiddenArea.position;
			while (elapsed < duration)
			{
				hiddenArea.position = Vector3.Lerp( start, visiblePosition_, elapsed/duration );
				elapsed += Time.deltaTime;
				yield return null;
			}
		}
		hiddenArea.position = visiblePosition_;

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
