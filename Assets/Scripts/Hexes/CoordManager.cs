using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoordManager 
{
	public class CoordComparer : IEqualityComparer<HexCoords>
	{
		public bool Equals(HexCoords a, HexCoords b)
		{
			if((a == null && b != null) || (a != null && b == null))
				return false;

			if(a.GetType() != b.GetType())
				return false;

			return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
		}

		public int GetHashCode(HexCoords coords)
		{
			return System.Convert.ToString(coords.Id).GetHashCode();
		}
	}

	public static Vector3 GetWorldCoordinates(HexCoords coords, float elevation = 0f)
	{
		float x = (HexDimensions.BridgeWidth + HexDimensions.EdgeLength) * (Mathf.Sqrt(3) * coords.X  +  Mathf.Sqrt(3)/2 * coords.Z);
    	float z = (HexDimensions.BridgeWidth + HexDimensions.EdgeLength) * ( 3f/2 * coords.Z);   
		return new Vector3(x, elevation * HexDimensions.ElevationStep, z);
	}

	public static Vector3 GetWorldCoordinates(HexCell cell)
	{
		float x = (HexDimensions.BridgeWidth + HexDimensions.EdgeLength) * (Mathf.Sqrt(3) * cell.Coords.X  +  Mathf.Sqrt(3)/2 * cell.Coords.Z);
    	float z = (HexDimensions.BridgeWidth + HexDimensions.EdgeLength) * ( 3f/2 * cell.Coords.Z);   
		return new Vector3(x, 0f/*cell.Elevation * HexDimensions.ElevationStep */, z);
	}
}


