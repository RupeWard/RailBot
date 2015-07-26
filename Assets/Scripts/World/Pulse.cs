using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class Pulse : MonoBehaviour 
{

#region public settings

	public bool useGravity = false;

	public Vector3 velocity = Vector3.zero;
	
#endregion public settings

#region private Hooks

	private Rigidbody rigidbody_;

#endregion private Hooks


#region SetUp

	public void Init(Vector3 pVelocity, bool pUseGravity)
	{
		gameObject.SetActive(true);

		velocity = pVelocity;
		useGravity = false;
		rigidbody_.useGravity = useGravity;
		rigidbody_.velocity = velocity;
	}
	
	void Awake()
	{
		rigidbody_ = GetComponent< Rigidbody >();
	}

	void Start () 
	{
	}

#endregion SetUp

#region Flow

	void FixedUpdate () 
	{
		transform.localPosition = transform.localPosition + velocity * Time.fixedDeltaTime;
	}

#endregion Flow

}
