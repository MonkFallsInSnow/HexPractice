using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexChunk
{
	public Data MapData { get; private set; }

	public struct Data
	{
		//public Dictionary<Vector3, HexCell> Cells { get; private set; }
		public Dictionary<HexCoords, HexCell> Cells { get; private set; }
		public int Columns { get; private set; }
		public int Rows { get; private set; }
		public Vector3 Origin { get; private set; }

		public Data(int rows, int columns, Vector3 origin)
		{
			//this.Cells = new Dictionary<Vector3, HexCell>(new Vector3CoordComparer());
			this.Cells = new Dictionary<HexCoords, HexCell>();
			this.Rows = rows;
			this.Columns = columns;

			this.Origin = origin;
		}
	}

	public HexChunk(int rows, int columns, Vector3 origin)
	{
		this.MapData = new Data(rows, columns, origin);
		this.GenerateMap(this.MapData);
	}

	private void GenerateMap(HexChunk.Data data)
	{
		//Vector3 coords = data.Origin;
		//float rowOffset = HexDimensions.InnerRadius + HexDimensions.BridgeWidth;
		
		for(int row = 0; row < data.Rows; row++)
		{
			for(int col = 0; col < data.Columns; col++)
			{
				HexCoords hexCoords = new HexCoords(row, col);
				HexCell cell = new HexCell(hexCoords, this.GetElevation(hexCoords));
				this.MapData.Cells.Add(hexCoords, cell);
				/*
				this.MapData.Cells.Add(coords, cell);

				coords = new Vector3(
					coords.x + HexDimensions.HorizontalSpacing + HexDimensions.BridgeWidth,
					coords.y,
					coords.z
				);
				*/
			}

			/*
			coords = new Vector3(
				//(row + 1) * rowOffset,
				(row + 1) % 2 == 0 ? data.Origin.x : rowOffset + data.Origin.x,
				coords.y,
				coords.z + HexDimensions.VerticalSpacing + HexDimensions.BridgeWidth

			);
			*/
		}
	}

	private int GetElevation(HexCoords hexCoords)
	{
		//elevation test
		return (int)(Mathf.PerlinNoise(hexCoords.X / HexDimensions.OuterRadius, hexCoords.Z / HexDimensions.OuterRadius) * 10);
	}


}
