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
		//float x = (cell.Coords.X + cell.Coords.Z / 2) * HexDimensions.HorizontalSpacing + (HexDimensions.InnerRadius * (cell.Coords.Z % 2));
		//float z = cell.Coords.Z * HexDimensions.VerticalSpacing;
		float x = (HexDimensions.BridgeWidth+HexDimensions.EdgeLength) * (Mathf.Sqrt(3) * cell.Coords.X  +  Mathf.Sqrt(3)/2 * cell.Coords.Z);
    	float z = (HexDimensions.BridgeWidth+HexDimensions.EdgeLength) * ( 3f/2 * cell.Coords.Z);   
		return new Vector3(x, 0f/*cell.Elevation * HexDimensions.ElevationStep */, z);
	}
}
