using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class Avatar : MonoBehaviour 
{
	private Collider collider_;

	public System.Action< RaycastHit > onClickedAction;

	public void Awake()
	{
		collider_ = GetComponent< Collider >();
	}

	public void Update()
	{
		if ( Input.GetMouseButtonDown (0 ) )
		{
			//				Debug.Log("Mouse");
			Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
			RaycastHit hit = new RaycastHit ( );
			
			if ( Physics.Raycast ( ray,  out hit ) )
			{
				//					Debug.Log("Hit");
				
				if ( hit.collider == collider_ )
				{
					if (onClickedAction != null)
					{
						onClickedAction ( hit );
					}
				}
			}
		}
	}
}
