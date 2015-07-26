using UnityEngine;
using System.Text.RegularExpressions;

public static class Version 
{
	// Change this to match whenever updating version number in build settings
	public static VersionNumber versionNumber = new VersionNumber("0.0.1 (1)");
	
	private static readonly bool DEBUG_VERSION = true;
	
	public class VersionNumber : IDebugDescribable
	{
		public int[] subNumbers_ = new int[4];
		
		public VersionNumber(int first, int second, int third, int build)
		{
			subNumbers_[0] = first;
			subNumbers_[1] = second;
			subNumbers_[2] = third;
			subNumbers_[3] = build;
		}
		
		public VersionNumber(string s)
		{
			setFromString(s);
		}
		
		private VersionNumber(){}
		
		public override string ToString()
		{
			return this.DebugDescribe ();
		}
		
		private void setFromString(string s)
		{
			Regex regex = new Regex(@"(\d+)\.(\d+)\.(\d+) \((\d+)\)");
			Match match = regex.Match (s);
			if (match.Success)
			{
				for (int i = 0; i<4; i++)
				{
					subNumbers_[i] = int.Parse(match.Groups[i+1].Value);
				}
				
				if (Version.DEBUG_VERSION)
				{
					//					Debug.Log ("Version read from '"+s+"' is "+this.DebugDescribe());
				}
			}
			else
			{
				Debug.LogError("Failed to get version from '"+s+"'");
			}
		}
		
		public void DebugDescribe(System.Text.StringBuilder sb)
		{
			sb.Append (subNumbers_[0].ToString())
				.Append(".").Append (subNumbers_[1].ToString ())
					.Append (".").Append (subNumbers_[2].ToString ())
					.Append (" (").Append (subNumbers_[3].ToString()).Append(")");
		}
		
		public bool Differs(VersionNumber other)
		{
			return Differs (other, 4);
		}
		
		public bool DiffersExceptBuildNumber(VersionNumber other)
		{
			return Differs (other, 3);
		}
		
		public bool Differs(VersionNumber other, int depth)
		{
			bool result = false;
			
			if (depth < 1 || depth > 4)
			{
				throw new System.ArgumentOutOfRangeException("Depth must be in [1,4]");
			}
			for (int i=0; i<depth; i++)
			{
				if ( subNumbers_[i] != other.subNumbers_[i] )
				{
					result = true;
					break;
				}
			}
			return result;
		}
		
		public bool Before(VersionNumber other)
		{
			return Before (other, 4);
		}
		
		public bool BeforeExceptBuildNumber(VersionNumber other)
		{
			return Before (other, 3);
		}	
		
		public bool Before(VersionNumber other, int depth)
		{
			bool result = false;
			
			if (depth < 1 || depth > 4)
			{
				throw new System.ArgumentOutOfRangeException("Depth must be in [1,4]");
			}
			for (int i=0; i<depth; i++)
			{
				if ( subNumbers_[i] < other.subNumbers_[i] )
				{
					result = true;
					break;
				}
			}
			return result;
		}
		
	};


}
