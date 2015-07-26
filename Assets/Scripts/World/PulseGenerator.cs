using UnityEngine;
using System.Collections;

public class PulseGenerator : MonoBehaviour 
{
//	static private readonly bool DEBUG_LOCAL = true;
	static private readonly bool DEBUG_GEN = false;

#region Inspector hooks

	public GameObject pulseTemplate;
	public GameObject pulseHolder;
	public Transform pulseStartPosition;

	public AudioClip pulseGenSound;

	public Color pulseColour = Color.magenta;
	public Material pulseMaterial;

	public Avatar avatar;

#endregion Inspector hooks

#region public settings

	public bool doPulseGenSound = true;

	public bool useGravity = false;
	public Vector3 pulseVelocity = Vector3.left;
	public float pulseSpeed = 1f;

	public bool getTempoFromManager = false;

#endregion public settings

	private float frequencyHz_ = 0f;

	public void SetFrequencyHz( float frequency )
	{
		frequencyHz_ = frequency;
	}

#region private hooks

	private AudioSource audioSource_;

#endregion private hooks

#region private data

//	private float timeTillPulse_ = 0f;

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

	public void Start()
	{
		if (avatar != null)
		{
			avatar.onClickedAction += OnClickedHandler;
		}
		if (getTempoFromManager)
		{
			SetFrequencyHz(TempoManager.Instance.BeatFrequency);
			TempoManager.Instance.beatFrequencyChangedAction += SetFrequencyHz;
			TempoManager.Instance.onBeatAction += CreatePulse;
		}
	}

#endregion SetUp

#region Flow
	
#endregion Flow

#region Pulse

	public void CreatePulse()
	{
		GetPulse();
	}

	public Pulse GetPulse()
	{
		GameObject newPulseGO = Instantiate(pulseTemplate) as GameObject;
		string newPulseName = "P_" + nextPulseNumber_.ToString();
		newPulseGO.name = newPulseName;
		newPulseGO.transform.parent = pulseHolder.transform;
		newPulseGO.transform.position = pulseStartPosition.position;
		newPulseGO.transform.localScale = Vector3.one;

		nextPulseNumber_++;

		Pulse newPulse = newPulseGO.GetComponent< Pulse >();

		Material newMaterial = new Material(pulseMaterial);
		newMaterial.SetColor("_Color", pulseColour);
		newPulse.Init(pulseVelocity, useGravity, newMaterial);

		if (doPulseGenSound)
		{
			if (audioSource_.isPlaying)
			{
				audioSource_.Stop();
			}
			audioSource_.Play();
			timeTillAudioEnd = AudioLengthInSeconds;
		}

		if (DEBUG_GEN)
		{
			Debug.Log("Created pulse "+newPulseName);
		}

		return newPulse;
	}
			
#endregion Pulse

	public float audioLengthInBeats = 1f;
	public float AudioLengthInSeconds
	{
		get { return audioLengthInBeats * TempoManager.Instance.BeatLengthSeconds;  } 
	}

	public float timeTillAudioEnd = 0f;

	public void Update()
	{
		if (timeTillAudioEnd > 0f)
		{
			timeTillAudioEnd -= Time.deltaTime;
			if (timeTillAudioEnd <= 0f)
			{
				audioSource_.Stop();
			}
		}
	}

	public void OnClickedHandler(RaycastHit hit)
	{
		Debug.Log("Clicked on " + gameObject.name);
	}

}
