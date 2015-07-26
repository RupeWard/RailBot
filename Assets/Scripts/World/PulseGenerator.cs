using UnityEngine;
using System.Collections;

public class PulseGenerator : MonoBehaviour 
{
#region Inspector hooks

	public GameObject pulseTemplate;
	public GameObject pulseHolder;
	public Transform pulseStartPosition;

	public AudioClip pulseGenSound;


#endregion Inspector hooks

#region public settings

	public float frequencyHz = 2f;

	public bool doPulseGenSound = true;

	public bool useGravity = false;
	public Vector3 pulseVelocity = Vector3.left;
	public float pulseSpeed = 1f;

#endregion public settings
	
#region private hooks

	private AudioSource audioSource_;

#endregion private hooks

#region private data

	private float timeTillPulse_ = 0f;

	private uint nextPulseNumber_ = 0u;

#endregion private data

#region SetUp

	public void Awake()
	{
		pulseVelocity = pulseVelocity / pulseVelocity.magnitude;
		pulseVelocity *= pulseSpeed;

		audioSource_ = GetComponent< AudioSource >();
		if (audioSource_ != null )
		{
			if (pulseGenSound != null)
			{
				audioSource_.clip = pulseGenSound;
			}
		}
		else
		{
			doPulseGenSound = false;
		}
	}

#endregion SetUp

#region Flow

	public void Update()
	{
		timeTillPulse_ -= Time.deltaTime;
		if (timeTillPulse_ <= 0f)
		{
			CreatePulse();
			timeTillPulse_ = 1f / frequencyHz;
		}
	}

#endregion Flow

#region Pulse
	
	private void CreatePulse()
	{
		GameObject newPulseGO = Instantiate(pulseTemplate) as GameObject;
		newPulseGO.name = "Pulse_" + nextPulseNumber_.ToString();
		newPulseGO.transform.parent = pulseHolder.transform;
		newPulseGO.transform.position = pulseStartPosition.position;
		newPulseGO.transform.localScale = Vector3.one;

		nextPulseNumber_++;

		Pulse newPulse = newPulseGO.GetComponent< Pulse >();
		newPulse.Init(pulseVelocity, useGravity);

		if (doPulseGenSound)
		{
			if (audioSource_.isPlaying)
			{
				audioSource_.Stop();
			}
			audioSource_.Play();
		}
	}
			
#endregion Pulse
			

}
