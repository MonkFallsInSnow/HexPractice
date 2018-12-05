using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxialCoords
{
	public int X { get; private set; }
	public int Z { get; private set; }
	public int Y { get { return -X - Z; } }

	public AxialCoords(int x, int z)
	{
		this.X = x ;//- z / 2;
		this.Z = z;
	}

	public static AxialCoords ConvertToAxialCoords(int row, int col)
	{
		int x = col - row / 2;
		int z = row;
		
		return new AxialCoords(x, z);
	}

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

	public static AxialCoords operator+(AxialCoords left, AxialCoords right)
	{
		return new AxialCoords(left.X + right.X, left.Z + right.Z);
	}

	public override string ToString()
	{
		return string.Format("({0}, {1})", this.X, this.Z);
	}
	
}
