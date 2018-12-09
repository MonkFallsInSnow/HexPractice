using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoordManager 
{
	public static Vector3 GetWorldCoordinates(HexCoords coords, float elevation = 0f)
	{
		float x = (HexDimensions.BridgeWidth+HexDimensions.EdgeLength) * (Mathf.Sqrt(3) * coords.X  +  Mathf.Sqrt(3)/2 * coords.Z);
    	float z = (HexDimensions.BridgeWidth+HexDimensions.EdgeLength) * ( 3f/2 * coords.Z);   
		return new Vector3(x, elevation * HexDimensions.ElevationStep, z);
	}

	public static Vector3 GetWorldCoordinates(HexCell cell)
	{
		float x = (HexDimensions.BridgeWidth+HexDimensions.EdgeLength) * (Mathf.Sqrt(3) * cell.Coords.X  +  Mathf.Sqrt(3)/2 * cell.Coords.Z);
    	float z = (HexDimensions.BridgeWidth+HexDimensions.EdgeLength) * ( 3f/2 * cell.Coords.Z);   
		return new Vector3(x, 0f/*cell.Elevation * HexDimensions.ElevationStep */, z);
	}
}
