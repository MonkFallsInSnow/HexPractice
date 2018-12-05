using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexMap
{
	public Data MapData { get; private set; }

	public struct Data
	{
		public Dictionary<Vector3, HexCell> Cells { get; private set; }
		public int Columns { get; private set; }
		public int Rows { get; private set; }
		public Vector3 Origin { get; private set; }

		public Data(int rows, int columns, Vector3 origin)
		{
			this.Cells = new Dictionary<Vector3, HexCell>(new Vector3CoordComparer());
			this.Rows = rows;
			this.Columns = columns;

			this.Origin = origin;
		}
	}

	public HexMap(int rows, int columns, Vector3 origin)
	{
		this.MapData = new Data(rows, columns, origin);

		this.GenerateMap(this.MapData);
	}

	private void GenerateMap(HexMap.Data data)
	{
		Vector3 coords = data.Origin;
		float rowOffset = HexDimensions.InnerRadius + HexDimensions.BridgeWidth;
		
		for(int row = 0; row < data.Rows; row++)
		{
			for(int col = 0; col < data.Columns; col++)
			{
				//AxialCoords hexCoords = new AxialCoords(col-row/2, row);
				AxialCoords hexCoords = AxialCoords.ConvertToAxialCoords(row, col);
				HexCell cell = new HexCell(hexCoords, this.GetElevation(coords));
				this.MapData.Cells.Add(coords, cell);

				coords = new Vector3(
					coords.x + HexDimensions.HorizontalSpacing + HexDimensions.BridgeWidth,
					coords.y,
					coords.z
				);
			}

			coords = new Vector3(
				//(row + 1) * rowOffset,
				(row + 1) % 2 == 0 ? data.Origin.x : rowOffset + data.Origin.x,
				coords.y,
				coords.z + HexDimensions.VerticalSpacing + HexDimensions.BridgeWidth

			);
		}
	}

	public int GetElevation(Vector3 hexCoords)
	{
		//elevation test
		return (int)(Mathf.PerlinNoise(hexCoords.x / 10, hexCoords.z / 30) * 10);
	}


}
