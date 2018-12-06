using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoordManager 
{
	public static Vector3 GetWorldCoordinates(HexCoords coords)
	{
		float x = Mathf.Cos(60) * coords.X;
		float z = Mathf.Sin(60) * coords.Z;

		return new Vector3(x, 0f, z);
	}

	public static Vector3 GetWorldCoordinates(HexCell cell)
	{
		float x = (cell.Coords.X + cell.Coords.Z / 2) * HexDimensions.HorizontalSpacing + (HexDimensions.InnerRadius * (cell.Coords.Z % 2));
		float z = cell.Coords.Z * HexDimensions.VerticalSpacing;

		return new Vector3(x, cell.Elevation * HexDimensions.ElevationStep, z);
	}
}
