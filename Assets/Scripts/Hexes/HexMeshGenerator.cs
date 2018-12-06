using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMeshGenerator
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

	public static Mesh CreateHexMap(HexChunk.Data mapData)
	{
		Reset();

		Mesh mesh = new Mesh();

		foreach(var cell in mapData.Cells)
		{
			Vector3 cellWorldCoords = CoordManager.GetWorldCoordinates(cell.Value);
			Debug.Log(cell.Key.ToString() + " -> " + cellWorldCoords.ToString());
			//
			Vector3 currentCellElevation = Vector3.zero;
			Vector3 northEastElevation = Vector3.zero;
			Vector3 southEastElevation = Vector3.zero;
			Vector3 eastElevation = Vector3.zero;
			//
			Hex hex = new Hex(
				cellWorldCoords + HexCorners.N + currentCellElevation,
				cellWorldCoords + HexCorners.NE + currentCellElevation,
				cellWorldCoords + HexCorners.SE + currentCellElevation,
				cellWorldCoords + HexCorners.S + currentCellElevation,
				cellWorldCoords + HexCorners.SW + currentCellElevation,
				cellWorldCoords + HexCorners.NW + currentCellElevation
			);
/* 
			Quad northEastBridge = new Quad (
				cellWorldCoords + HexCorners.N  + currentCellElevation,
				cellWorldCoords + HexNeighbors.NE + HexCorners.SW + currentCellElevation + northEastElevation,
				cellWorldCoords + HexNeighbors.NE + HexCorners.S + currentCellElevation + northEastElevation,
				cellWorldCoords + HexCorners.NE  + currentCellElevation
			);

			Quad southEastBridge = new Quad (
				cellWorldCoords + HexCorners.SE  + currentCellElevation,
				cellWorldCoords + HexNeighbors.SE + HexCorners.N + currentCellElevation + southEastElevation,
				cellWorldCoords + HexNeighbors.SE + HexCorners.NW + currentCellElevation + southEastElevation,
				cellWorldCoords + HexCorners.S  + currentCellElevation
			);

			Quad eastBridge = new Quad (
				cellWorldCoords + HexCorners.NE  + currentCellElevation,
				cellWorldCoords + HexNeighbors.E + HexCorners.NW + currentCellElevation + eastElevation,
				cellWorldCoords + HexNeighbors.E + HexCorners.SW + currentCellElevation + eastElevation,
				cellWorldCoords + HexCorners.SE  + currentCellElevation
			);

			Triangle triangleUp = new Triangle (
				cellWorldCoords + HexCorners.NE  + currentCellElevation,
				cellWorldCoords + HexNeighbors.NE + HexCorners.S + currentCellElevation + northEastElevation,
				cellWorldCoords + HexNeighbors.E + HexCorners.NW + currentCellElevation + eastElevation
			);

			Triangle triangleDown = new Triangle (
				cellWorldCoords + HexCorners.SE  + currentCellElevation,
				cellWorldCoords + HexNeighbors.E + HexCorners.SW + currentCellElevation + eastElevation,
				cellWorldCoords + HexNeighbors.SE + HexCorners.N + currentCellElevation + southEastElevation
			);
*/
			BuildHex(hex);
			/*
			BuildQuad(northEastBridge);
				BuildQuad(eastBridge);
				BuildQuad(southEastBridge);
				BuildTriangle(triangleUp);
				BuildTriangle(triangleDown);
				*/
		}

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		//mesh.colors = colors.ToArray();
		mesh.RecalculateNormals();

		return mesh;

	}
	/*
	//refactor (split up)
	//mapLength breaks down if more than 10 rows
	//columns break down after about 50 of them
	public static Mesh CreateHexMap(HexChunk.Data mapData)
	{
		Reset();

		Mesh mesh = new Mesh();
		float mapLength = (mapData.Rows * HexDimensions.VerticalSpacing) - (HexDimensions.OuterRadius / 2); 

		foreach(var cell in mapData.Cells)
		{
			HexCell currentCell = cell.Value;
			Vector3 currentCellPos = cell.Key;
			Vector3 currentCellElevation = new Vector3(0f, currentCell.Elevation, 0f);
			
			Vector3 northEastElevation = Vector3.zero;
			Vector3 southEastElevation = Vector3.zero;
			Vector3 eastElevation = Vector3.zero;

			if(mapData.Cells.ContainsKey(currentCellPos + HexNeighbors.NE))
			{
				HexCell neighbor = mapData.Cells[currentCellPos + HexNeighbors.NE];
				northEastElevation = new Vector3(0f, neighbor.Elevation - currentCell.Elevation, 0f);
			}

			if(mapData.Cells.ContainsKey(currentCellPos + HexNeighbors.SE))
			{
				HexCell neighbor = mapData.Cells[currentCellPos + HexNeighbors.SE];
				southEastElevation = new Vector3(0f, neighbor.Elevation - currentCell.Elevation, 0f);
			}

			if(mapData.Cells.ContainsKey(currentCellPos + HexNeighbors.E))
			{
				HexCell neighbor = mapData.Cells[currentCellPos + HexNeighbors.E];
				eastElevation = new Vector3(0f, neighbor.Elevation - currentCell.Elevation, 0f);
			}			
			
			Hex hex = new Hex(
				currentCellPos + HexCorners.N + currentCellElevation,
				currentCellPos + HexCorners.NE + currentCellElevation,
				currentCellPos + HexCorners.SE + currentCellElevation,
				currentCellPos + HexCorners.S + currentCellElevation,
				currentCellPos + HexCorners.SW + currentCellElevation,
				currentCellPos + HexCorners.NW + currentCellElevation
			);

			Quad northEastBridge = new Quad (
				currentCellPos + HexCorners.N  + currentCellElevation,
				currentCellPos + HexNeighbors.NE + HexCorners.SW + currentCellElevation + northEastElevation,
				currentCellPos + HexNeighbors.NE + HexCorners.S + currentCellElevation + northEastElevation,
				currentCellPos + HexCorners.NE  + currentCellElevation
			);

			Quad southEastBridge = new Quad (
				currentCellPos + HexCorners.SE  + currentCellElevation,
				currentCellPos + HexNeighbors.SE + HexCorners.N + currentCellElevation + southEastElevation,
				currentCellPos + HexNeighbors.SE + HexCorners.NW + currentCellElevation + southEastElevation,
				currentCellPos + HexCorners.S  + currentCellElevation
			);

			Quad eastBridge = new Quad (
				currentCellPos + HexCorners.NE  + currentCellElevation,
				currentCellPos + HexNeighbors.E + HexCorners.NW + currentCellElevation + eastElevation,
				currentCellPos + HexNeighbors.E + HexCorners.SW + currentCellElevation + eastElevation,
				currentCellPos + HexCorners.SE  + currentCellElevation
			);

			Triangle triangleUp = new Triangle (
				currentCellPos + HexCorners.NE  + currentCellElevation,
				currentCellPos + HexNeighbors.NE + HexCorners.S + currentCellElevation + northEastElevation,
				currentCellPos + HexNeighbors.E + HexCorners.NW + currentCellElevation + eastElevation
			);

			Triangle triangleDown = new Triangle (
				currentCellPos + HexCorners.SE  + currentCellElevation,
				currentCellPos + HexNeighbors.E + HexCorners.SW + currentCellElevation + eastElevation,
				currentCellPos + HexNeighbors.SE + HexCorners.N + currentCellElevation + southEastElevation
			);

			BuildHex(hex);

			if(currentCellPos.z == 0)
			{
				BuildQuad(northEastBridge);
				BuildQuad(eastBridge);
				BuildTriangle(triangleUp);
				
			}
			else if(currentCellPos.z > 0 && currentCellPos.z < mapLength)
			{
				BuildQuad(northEastBridge);
				BuildQuad(eastBridge);
				BuildQuad(southEastBridge);
				BuildTriangle(triangleUp);
				BuildTriangle(triangleDown);
			}
			else if(currentCellPos.z == mapLength)
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
	*/

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
