using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(MeshRenderer))]
public class Pulse : MonoBehaviour 
{
	static private readonly bool DEBUG_LOCAL = true;

#region inspector hooks

	public MeshRenderer meshRenderer;

#endregion inspector hooks

#region public settings

	public bool useGravity = false;

	public Vector3 velocity = Vector3.zero;
	
#endregion public settings

#region private Hooks

	private Rigidbody rigidbody_;

#endregion private Hooks

#region private data

	private float maxLifetime_ = 10f;
	private float lifetime_ = 0f;

#endregion private data

#region SetUp

	public void Init(Vector3 pVelocity, bool pUseGravity, Material material)
	{
		gameObject.SetActive(true);

		velocity = pVelocity;
		useGravity = pUseGravity;
		rigidbody_.useGravity = useGravity;
		rigidbody_.velocity = velocity;

		if (meshRenderer != null)
		{
			meshRenderer.sharedMaterial = material;
		}
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
		lifetime_ += Time.fixedDeltaTime;
		if (lifetime_ > 0f && lifetime_ > maxLifetime_)
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log("Killing "+gameObject.name+" because lifetime = "+lifetime_); 
			}
			GameObject.Destroy(this.gameObject);
		}
	}

#endregion Flow

}
