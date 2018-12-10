using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//may want to change file name back to axialcoords and just make this a static class, since all it really does is convert coordinates to and from axial coords
public class HexCoords : IComparer
{
	public readonly System.Guid Id;
	public readonly int X;	
	public readonly int Z;
	public readonly int Y;

	public HexCoords(int x, int z)
	{
		this.Id = System.Guid.NewGuid();
		this.X = x - z / 2;
		//this.X = x;
		this.Y = -x - z;
		this.Z = z;

	}

	public static HexCoords operator+(HexCoords left, HexCoords right)
	{
		return new HexCoords(left.X + right.X, left.Z + right.Z);
	}

	public static bool operator==(HexCoords a, HexCoords b)
	{
		return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
	}

	public override bool Equals(object obj)
	{
		if(obj == null)
			return false;

		if(this.GetType() != obj.GetType())
			return false;

		HexCoords coords = (HexCoords)obj;

		return this.X == coords.X && this.Y == coords.Y && this.Z == coords.Z;
	}

	public override int GetHashCode()
	{
		return System.Convert.ToString(this.Id).GetHashCode();
	}

	public static bool operator!=(HexCoords a, HexCoords b)
	{
		return a.X != b.X && a.Y != b.Y && a.Z != b.Z;
	}

	public override string ToString()
	{
		return string.Format("({0}, {1})", this.X, this.Z);
	}

	public int Compare(object obj1, object obj2)
	{
		HexCoords a = (HexCoords)obj1;
		HexCoords b = (HexCoords)obj2;

		if (a.X.CompareTo(b.X) != 0)
		{
			return a.X.CompareTo(b.X);
		}
		else if (a.Y.CompareTo(b.Y) != 0)
		{
			return a.Y.CompareTo(b.Y);
		}
		else if (a.Z.CompareTo(b.Z) != 0)
		{
			return a.Z.CompareTo(b.Z);
		}
		else
		{
			return 0;
		}
	}
}

	
/*
	public static AxialCoords ConvertToAxialCoords(Vector3 cubeCoords)
	{
		return new AxialCoords(
			(int)cubeCoords.x, 
			(int)cubeCoords.z
		);
	}

	public static Vector3 ConvertToCubeCoords(AxialCoords axialCoords)
	{
		return new Vector3(
			axialCoords.X,
			axialCoords.Y,
			axialCoords.Z
		);
	}

	public static AxialCoords ConvertToLocalCoords(Vector3 worldCoords)
	{
		return new AxialCoords(
			(int)(worldCoords.x / HexDimensions.HorizontalSpacing),
			(int)(worldCoords.z / HexDimensions.VerticalSpacing)
		);
	}

	public static Vector3 ConvertToWorldCoords(AxialCoords localCoords)
	{
		float x = (localCoords.X + localCoords.Z * 0.5f - localCoords.Z / 2) * (HexDimensions.InnerRadius * 2f);
		float z = localCoords.Z * (HexDimensions.OuterRadius * 1.5f);
		return new Vector3(x, 0f, z);
	}
*/
