using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMesh 
{
	private struct Triangle
	{
		public Vector3 v1;
		public Vector3 v2;
		public Vector3 v3;

		public Triangle(Vector3 a, Vector3 b, Vector3 c)
		{
			this.v1 = a;
			this.v2 = b;
			this.v3 = c;
		}
	}

	private struct Quad
	{
		public Vector3 v1;
		public Vector3 v2;
		public Vector3 v3;
		public Vector3 v4;

		public Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			this.v1 = a;
			this.v2 = b;
			this.v3 = c;
			this.v4 = d;
		}
	}

	private struct Hex
	{
		public Vector3 v1;
		public Vector3 v2;
		public Vector3 v3;
		public Vector3 v4;
		public Vector3 v5;
		public Vector3 v6;

		public Hex(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e, Vector3 f)
		{
			this.v1 = a;
			this.v2 = b;
			this.v3 = c;
			this.v4 = d;
			this.v5 = e;
			this.v6 = f;
		}
	}

	private static List<Vector3> vertices = new List<Vector3>();
	private static List<int> triangles = new List<int>();

	//refactor (split up)
	//mapLength breaks down if more than 10 rows
	//columns break down after about 50 of them
	public static Mesh CreateHexMap(HexMap.Data mapData)
	{
		Reset();

		Mesh mesh = new Mesh();
		float mapLength = (mapData.Rows * HexDimensions.VerticalSpacing) - (HexDimensions.OuterRadius / 2); 
		Debug.Log(mapLength);

		foreach(var hexCoord in mapData.Cells.Keys)
		{//new Vector3(0f, mapData.Cells[hexCoord].Elevation, 0f)
			HexCell currentCell = mapData.Cells[hexCoord];
			Vector3 currentCellElevation = new Vector3(0f, currentCell.Elevation, 0f);
			Vector3 northEastElevation = Vector3.zero;
			Vector3 southEastElevation = Vector3.zero;
			Vector3 eastElevation = Vector3.zero;

			if(mapData.Cells.ContainsKey(hexCoord + HexNeighbors.NE))
			{
				HexCell neighbor = mapData.Cells[hexCoord + HexNeighbors.NE];
				northEastElevation = new Vector3(0f, neighbor.Elevation - currentCell.Elevation, 0f);
			}

			if(mapData.Cells.ContainsKey(hexCoord + HexNeighbors.SE))
			{
				HexCell neighbor = mapData.Cells[hexCoord + HexNeighbors.SE];
				southEastElevation = new Vector3(0f, neighbor.Elevation - currentCell.Elevation, 0f);
			}

			if(mapData.Cells.ContainsKey(hexCoord + HexNeighbors.E))
			{
				HexCell neighbor = mapData.Cells[hexCoord + HexNeighbors.E];
				eastElevation = new Vector3(0f, neighbor.Elevation - currentCell.Elevation, 0f);
			}			
			
			Hex hex = new Hex(
				hexCoord + HexCorners.N + currentCellElevation,
				hexCoord + HexCorners.NE + currentCellElevation,
				hexCoord + HexCorners.SE + currentCellElevation,
				hexCoord + HexCorners.S + currentCellElevation,
				hexCoord + HexCorners.SW + currentCellElevation,
				hexCoord + HexCorners.NW + currentCellElevation
			);

			Quad northEastBridge = new Quad (
				hexCoord + HexCorners.N  + currentCellElevation,
				hexCoord + HexNeighbors.NE + HexCorners.SW + currentCellElevation + northEastElevation,
				hexCoord + HexNeighbors.NE + HexCorners.S + currentCellElevation + northEastElevation,
				hexCoord + HexCorners.NE  + currentCellElevation
			);

			Quad southEastBridge = new Quad (
				hexCoord + HexCorners.SE  + currentCellElevation,
				hexCoord + HexNeighbors.SE + HexCorners.N + currentCellElevation + southEastElevation,
				hexCoord + HexNeighbors.SE + HexCorners.NW + currentCellElevation + southEastElevation,
				hexCoord + HexCorners.S  + currentCellElevation
			);

			Quad eastBridge = new Quad (
				hexCoord + HexCorners.NE  + currentCellElevation,
				hexCoord + HexNeighbors.E + HexCorners.NW + currentCellElevation + eastElevation,
				hexCoord + HexNeighbors.E + HexCorners.SW + currentCellElevation + eastElevation,
				hexCoord + HexCorners.SE  + currentCellElevation
			);

			Triangle triangleUp = new Triangle (
				hexCoord + HexCorners.NE  + currentCellElevation,
				hexCoord + HexNeighbors.NE + HexCorners.S + currentCellElevation + northEastElevation,
				hexCoord + HexNeighbors.E + HexCorners.NW + currentCellElevation + eastElevation
			);

			Triangle triangleDown = new Triangle (
				hexCoord + HexCorners.SE  + currentCellElevation,
				hexCoord + HexNeighbors.E + HexCorners.SW + currentCellElevation + eastElevation,
				hexCoord + HexNeighbors.SE + HexCorners.N + currentCellElevation + southEastElevation
			);

			BuildHex(hex);

			if(hexCoord.z == 0)
			{
				BuildQuad(northEastBridge);
				BuildQuad(eastBridge);
				BuildTriangle(triangleUp);
				
			}
			else if(hexCoord.z > 0 && hexCoord.z < mapLength)
			{
				BuildQuad(northEastBridge);
				BuildQuad(eastBridge);
				BuildQuad(southEastBridge);
				BuildTriangle(triangleUp);
				BuildTriangle(triangleDown);
			}
			else if(hexCoord.z == mapLength)
			{
				BuildQuad(eastBridge);
				BuildQuad(southEastBridge);
				BuildTriangle(triangleDown);
			}
			//may also need to add a build condition for the last row in the map
			
		}

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		//mesh.colors = colors.ToArray();
		mesh.RecalculateNormals();

		return mesh;
	}

	private static void Reset()
	{
		vertices.Clear();
		triangles.Clear();
	}

	private static void BuildHex(Hex hex)
	{
		int vertexIndex = vertices.Count;

		vertices.AddRange(
			new Vector3[] 
			{
				hex.v1,
				hex.v2,
				hex.v3,
				hex.v4,
				hex.v5,
				hex.v6
			}
		);

		TriangulateHex(vertexIndex);
	}

	private static void TriangulateHex(int vertexIndex)
	{
		triangles.AddRange(
			new int[]
			{
				vertexIndex,
				vertexIndex + 1,
				vertexIndex + 2,
				vertexIndex,
				vertexIndex + 2,
				vertexIndex + 3,
				vertexIndex,
				vertexIndex + 3,
				vertexIndex + 4,
				vertexIndex,
				vertexIndex + 4,
				vertexIndex + 5
			}
		);
	}

	private static void BuildQuad(Quad quad)
	{
		int vertexIndex = vertices.Count;

		vertices.AddRange(
			new Vector3[] 
			{
				quad.v1,
				quad.v2,
				quad.v3,
				quad.v4
			}
		);

		TriangulateQuad(vertexIndex);
	}

	private static void TriangulateQuad(int vertexIndex)
	{
		triangles.AddRange(
			new int[] 
			{
				vertexIndex,
				vertexIndex + 1,
				vertexIndex + 2,
				vertexIndex,
				vertexIndex + 2,
				vertexIndex + 3
			}
		);
	}

	private static void BuildTriangle(Triangle triangle)
	{
		int vertexIndex = vertices.Count;

		vertices.AddRange(
			new Vector3[] 
			{
				triangle.v1,
				triangle.v2,
				triangle.v3
			}
		);

		TriangulateTriangle(vertexIndex);
	}

	private static void TriangulateTriangle(int vertexIndex)
	{
		triangles.AddRange(
			new int[] 
			{
				vertexIndex,
				vertexIndex + 1,
				vertexIndex + 2,
			}
		);
	}	
}
