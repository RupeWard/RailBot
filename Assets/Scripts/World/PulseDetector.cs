using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class PulseDetector : MonoBehaviour 
{
//	static private readonly bool DEBUG_LOCAL = true;
	static private readonly bool DEBUG_PULSE = false;

#region inspector hooks

	public PulseGenerator pulseGenerator;

#endregion inspector hooks

#region public settings

	public Color normalColour = Color.white;
	public Color pulseColour = Color.red;

	public int killsPulseFrequency = 0;
	public int pulseActionFrequency = 0;

#endregion public settings

#region private hooks

	private Collider collider_ = null;

#endregion private hooks

#region private data

	private int pulsesSinceKill = 0;
	private int pulsesSinceAction = 0;

#endregion private data

#region SetUp

	public void Awake()
	{
		collider_ = GetComponent< Collider >();
	}

#endregion SetUp

#region Unity event handler

	public void OnTriggerEnter(Collider other)
	{
		Pulse pulse = other.GetComponent< Pulse >();
		if (pulse != null)
		{
			if (!pulse.ShouldIgnoreCollider(this.gameObject))
			{
				if (DEBUG_PULSE)
				{
					Debug.Log(other.gameObject.name+" triggered "+this.gameObject.name);
				}
				if (killsPulseFrequency > 0)
				{
					pulsesSinceKill++;
					if (pulsesSinceKill >= killsPulseFrequency)
					{
						GameObject.Destroy(other.gameObject);
						pulsesSinceKill = 0;
					}
				}
				if (pulseActionFrequency > 0)
				{
					pulsesSinceAction++;
					if (pulsesSinceAction >= pulseActionFrequency)
					{
						if (pulseGenerator != null)
						{
							Pulse p = pulseGenerator.GetPulse();
							p.AddIgnoreCollider(this.gameObject);
						}
						pulsesSinceAction = 0;
					}
				}
			}
		}
	}
#endregion Unity event handler

}
