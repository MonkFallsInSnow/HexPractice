using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell 
{
	public AxialCoords Coords { get; private set; }
	public byte Type { get; private set; }
	public int Elevation { get; private set; }


	public HexCell(AxialCoords coords, int elevation)
	{
		this.Coords = coords;
		this.Elevation = elevation;
	}

	public Vector3 GetNeighborWorldPosition(Vector3 neighborCoords)
	{
		AxialCoords localPosition = this.Coords + AxialCoords.ConvertToAxialCoords(neighborCoords);
		return AxialCoords.ConvertToWorldCoords(localPosition + AxialCoords.ConvertToAxialCoords(neighborCoords));
	}

	public AxialCoords GetNeighborLocalPosition(Vector3 neighborCoords)
	{
		return this.Coords + AxialCoords.ConvertToAxialCoords(neighborCoords);
	}

}
