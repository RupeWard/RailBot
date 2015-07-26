using UnityEngine;
using System.Collections;

public class TempoManager : SingletonSceneLifetime< TempoManager > 
{
	public float barLengthSeconds = 4f;
	public int beatsPerBar = 4;

	private float cachedBarLengthSeconds_= 0f;
	private float cachedBeatsPerBar_ =0f;

	private float timeTillBeat_ = 0f;
	private int numBeats_ = 0;

	public System.Action< float, int > tempoChangedAction;
	public System.Action< float > beatLengthChangedAction;
	public System.Action< float > barLengthChangedAction;
	public System.Action< float > beatFrequencyChangedAction;
	public System.Action< float > barFrequencyChangedAction;
	public System.Action onBeatAction;
	public System.Action onBarAction;

	public float BeatFrequency
	{
		get { return beatsPerBar / barLengthSeconds; }
	}

	public float BeatLengthSeconds
	{
		get { return barLengthSeconds/beatsPerBar; }
	}

	public float BarFrequency
	{
		get { return 1f / barLengthSeconds; }
	}

	public float BarLengthSeconds
	{
		get { return barLengthSeconds; }
	}



	protected override void PostAwake()
	{
		cachedBarLengthSeconds_ = barLengthSeconds;
		cachedBeatsPerBar_ = beatsPerBar;
	}

	public void Update()
	{
		bool bChanged = false;
		if (barLengthSeconds != cachedBarLengthSeconds_)
		{
			cachedBarLengthSeconds_ = barLengthSeconds;
			bChanged = true;
			if (barFrequencyChangedAction != null)
			{
				barFrequencyChangedAction(BarFrequency);
			}
			if (barLengthChangedAction != null)
			{
				barLengthChangedAction(barLengthSeconds);				
			}
		}
		if (bChanged || beatsPerBar != cachedBeatsPerBar_)
		{
			cachedBeatsPerBar_ = beatsPerBar;
			bChanged = true;
			if (beatFrequencyChangedAction != null)
			{
				beatFrequencyChangedAction(BeatFrequency);
			}
			if (beatLengthChangedAction != null)
			{
				beatLengthChangedAction(BeatLengthSeconds);				
			}
		}
		if (bChanged)
		{
			if (tempoChangedAction != null)
			{
				tempoChangedAction( barLengthSeconds, beatsPerBar);
			}
		}
		if (BeatFrequency > 0)
		{
//			Debug.Log("BEAT");
			timeTillBeat_ -= Time.deltaTime;
			if (timeTillBeat_ <= 0f)
			{
				if (onBeatAction != null)
				{
					onBeatAction();
				}
				timeTillBeat_ = 1f / BeatFrequency;
				numBeats_++;
				if (numBeats_ >= beatsPerBar)
				{
					numBeats_ = 0;
//					Debug.Log("BAR");
					if (onBarAction != null)
					{
						onBarAction();
					}
				}
			}
		}
		else
		{
//			Debug.Log("BeatFreq ="+BeatFrequency);
		}
	}


}
