using UnityEngine;
using System.Collections;

public class PulseDetector : MonoBehaviour 
{
	static private readonly bool DEBUG_LOCAL = true;

#region inspector hooks


#endregion inspector hooks

#region public settings

	public Color normalColour = Color.white;
	public Color pulseColour = Color.red;

	public int killsPulseFrequency = 0;

#endregion public settings

#region private hooks


#endregion private hooks

#region private data

	private int pulsesSinceKill = 0;

#endregion private data

#region Unity event handler

	public void OnTriggerEnter(Collider other)
	{
		if (DEBUG_LOCAL)
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
	}
#endregion Unity event handler

}
