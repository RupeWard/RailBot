using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(MeshRenderer))]
public class Pulse : MonoBehaviour 
{
	static private readonly bool DEBUG_LOCAL = true;

#region inspector hooks

	public MeshRenderer meshRenderer;
	public PulseGenerator pulseGenerator;

#endregion inspector hooks

#region public settings

	public bool useGravity = false;

	public Vector3 velocity = Vector3.zero;
	
#endregion public settings

#region private Hooks

	private Rigidbody rigidbody_;
	private HashSet < GameObject > ignoreColliders_ = new HashSet<GameObject>();

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

	public void AddIgnoreCollider(GameObject c)
	{
		ignoreColliders_.Add(c);
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

	public bool ShouldIgnoreCollider(GameObject g)
	{
		bool result = false;
		if (ignoreColliders_.Count > 0 && ignoreColliders_.Contains(g))
		{
			result=true;
		}
		return result;
	}
}
