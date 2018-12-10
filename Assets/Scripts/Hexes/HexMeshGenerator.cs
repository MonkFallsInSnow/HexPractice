using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class HexMeshGenerator
{
	private interface TileComponent
	{
		Vector3 V1 { get; }
		Vector3 V2 { get; }
		Vector3 V3 { get; }
	}

	private enum ComponentName
	{
		Hex,
		TriangleUp,
		TriangleDown,
		NorthEastBridge,
		SouthEastBridge,
		EastBridge
	}


	private struct HexTile
	{
		public Dictionary<ComponentName, TileComponent> Components { get; private set; }

		public HexTile(Dictionary<ComponentName, TileComponent> components)
		{
			this.Components = components;
		}
	}

	private struct Triangle : TileComponent
	{
		public Vector3 V1 { get; private set;}
		public Vector3 V2 { get; private set;}
		public Vector3 V3 { get; private set;}

		public Triangle(Vector3 a, Vector3 b, Vector3 c)
		{
			this.V1 = a;
			this.V2 = b;
			this.V3 = c;
		}
	}

	private struct Quad : TileComponent
	{
		public Vector3 V1 { get; private set;}
		public Vector3 V2 { get; private set;}
		public Vector3 V3 { get; private set;}
		public Vector3 V4 { get; private set;}

		public Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			this.V1 = a;
			this.V2 = b;
			this.V3 = c;
			this.V4 = d;
		}
	}

	private struct Hex : TileComponent
	{
		public Vector3 V1 { get; private set;}
		public Vector3 V2 { get; private set;}
		public Vector3 V3 { get; private set;}
		public Vector3 V4 { get; private set;}
		public Vector3 V5 { get; private set;}
		public Vector3 V6 { get; private set;}

		public Hex(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e, Vector3 f)
		{
			this.V1 = a;
			this.V2 = b;
			this.V3 = c;
			this.V4 = d;
			this.V5 = e;
			this.V6 = f;
		}
	}

	

	private static List<Vector3> vertices = new List<Vector3>();
	private static List<int> triangles = new List<int>();

	public static Mesh CreateHexMap(HexChunk.Data mapData)
	{
		Reset();

		Mesh mesh = new Mesh();

		foreach(KeyValuePair<HexCoords, HexCell> cell in mapData.Cells)
		{
			//Vector3 cellWorldCoords = CoordManager.GetWorldCoordinates(cell.Value);
			//Debug.Log(cell.Key.ToString() + " NE-> " + HexNeighbors.NE.ToString() + " WNE-> " + CoordManager.GetWorldCoordinates(HexNeighbors.NE));
			
			HexTile tile = GenerateHexTileComponents(mapData, cell.Value);
			ConstructTile(tile);
		}

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();

		return mesh;
	}

	private static void Reset()
	{
		vertices.Clear();
		triangles.Clear();
	}

	private static HexTile GenerateHexTileComponents(HexChunk.Data mapData, HexCell cell)
	{
		float mapLength = (mapData.Rows * HexDimensions.VerticalSpacing) - (HexDimensions.OuterRadius / 2); 
		Vector3 coords = CoordManager.GetWorldCoordinates(cell.Coords);
		Dictionary<ComponentName, TileComponent> components = GetTileCompnents(mapData, cell);

			if(coords.z == 0)
			{
				return new HexTile(new Dictionary<ComponentName, TileComponent>()
				{
					{ ComponentName.Hex, components[ComponentName.Hex] },
					{ ComponentName.NorthEastBridge, components[ComponentName.NorthEastBridge] },
					{ ComponentName.EastBridge, components[ComponentName.EastBridge] },
					{ ComponentName.TriangleUp, components[ComponentName.TriangleUp] },
				});
			}
			else if(coords.z > 0 && coords.z < mapLength)
			{
				return new HexTile(new Dictionary<ComponentName, TileComponent>()
				{
					{ ComponentName.Hex, components[ComponentName.Hex] },
					{ ComponentName.NorthEastBridge, components[ComponentName.NorthEastBridge] },
					{ ComponentName.EastBridge, components[ComponentName.EastBridge] },
					{ ComponentName.SouthEastBridge, components[ComponentName.SouthEastBridge] },
					{ ComponentName.TriangleUp, components[ComponentName.TriangleUp] },
					{ ComponentName.TriangleDown, components[ComponentName.TriangleDown] },
				});
			}
			else
			{
				return new HexTile(new Dictionary<ComponentName, TileComponent>()
				{
					{ ComponentName.Hex, components[ComponentName.Hex] },
					{ ComponentName.EastBridge, components[ComponentName.NorthEastBridge] },
					{ ComponentName.SouthEastBridge, components[ComponentName.EastBridge] },
					{ ComponentName.TriangleDown, components[ComponentName.TriangleUp] },
				});
			}	
	}

	private static void ConstructTile(HexTile tile)
	{
		foreach(KeyValuePair<ComponentName, TileComponent> component in tile.Components)
		{
			if(component.Value is Hex)
			{
				BuildHex((Hex)component.Value);
			}
			else if(component.Value is Quad)
			{
				BuildQuad((Quad)component.Value);
			}
			else if(component.Value is Triangle)
			{
				BuildTriangle((Triangle)component.Value);
			}
		}
	}

	private static Dictionary<ComponentName, TileComponent> GetTileCompnents(HexChunk.Data mapData, HexCell cell)
	{
		Vector3 coords = CoordManager.GetWorldCoordinates(cell.Coords);
		Vector3 cellElevation = new Vector3(0, cell.Elevation, 0);

		HexCoords northEastNeighborHexCoords = cell.Coords + HexNeighbors.NE;
		HexCoords southEastNeighborHexCoords = cell.Coords + HexNeighbors.SE;
		HexCoords eastNeighborHexCoords = cell.Coords + HexNeighbors.E;

		Vector3 northEastNeighborWorldCoords = coords + CoordManager.GetWorldCoordinates(HexNeighbors.NE);
		Vector3 southeEastNeighborWorldCoords = coords + CoordManager.GetWorldCoordinates(HexNeighbors.SE);
		Vector3 eastNeighborWorldCoords = coords + CoordManager.GetWorldCoordinates(HexNeighbors.E);

		Vector3 northEastElevation = cellElevation + GetNeighborElevation(mapData, northEastNeighborHexCoords);
		Vector3 southEastElevation = cellElevation + GetNeighborElevation(mapData, southEastNeighborHexCoords);
		Vector3 eastElevation = cellElevation + GetNeighborElevation(mapData, eastNeighborHexCoords);

		Hex hex = new Hex(
			coords + HexCorners.N + cellElevation,
			coords + HexCorners.NE + cellElevation,
			coords + HexCorners.SE + cellElevation,
			coords + HexCorners.S + cellElevation,
			coords + HexCorners.SW + cellElevation,
			coords + HexCorners.NW + cellElevation
		);

		Quad northEastBridge = new Quad (
			coords + HexCorners.N + cellElevation,
			northEastNeighborWorldCoords + HexCorners.SW + northEastElevation,
			northEastNeighborWorldCoords + HexCorners.S + northEastElevation,
			coords + HexCorners.NE + cellElevation
		);

		Quad southEastBridge = new Quad (
			coords + HexCorners.SE  + cellElevation,
			southeEastNeighborWorldCoords + HexCorners.N + southEastElevation,
			southeEastNeighborWorldCoords + HexCorners.NW + southEastElevation,
			coords + HexCorners.S  + cellElevation
		);

		Quad eastBridge = new Quad (
			coords + HexCorners.NE  + cellElevation,
			eastNeighborWorldCoords + HexCorners.NW + eastElevation,
			eastNeighborWorldCoords + HexCorners.SW + eastElevation,
			coords + HexCorners.SE  + cellElevation
		);

		Triangle triangleUp = new Triangle (
			coords + HexCorners.NE  + cellElevation,
			northEastNeighborWorldCoords + HexCorners.S + northEastElevation,
			eastNeighborWorldCoords + HexCorners.NW + eastElevation
		);	

		Triangle triangleDown = new Triangle (
			coords + HexCorners.SE  + cellElevation,
			eastNeighborWorldCoords + HexCorners.SW + eastElevation,
			southeEastNeighborWorldCoords + HexCorners.N +southEastElevation
		);

		return new Dictionary<ComponentName, TileComponent>()
		{
			{ ComponentName.Hex, hex }, 
			{ ComponentName.NorthEastBridge, northEastBridge },
			{ ComponentName.EastBridge, eastBridge },
			{ ComponentName.SouthEastBridge, southEastBridge },
			{ ComponentName.TriangleUp, triangleUp },
			{ ComponentName.TriangleDown, triangleDown }
		};

	}

	private static Vector3 GetNeighborElevation(HexChunk.Data mapData, HexCoords neighbor)
	{
		if(mapData.Cells.ContainsKey(neighbor))
		{
			return new Vector3(0, mapData.Cells[neighbor].Elevation, 0);
		}
		else
		{
			return Vector3.zero;
		}
	}

	private static void BuildHex(Hex hex)
	{
		int vertexIndex = vertices.Count;

		vertices.AddRange(
			new Vector3[] 
			{
				hex.V1,
				hex.V2,
				hex.V3,
				hex.V4,
				hex.V5,
				hex.V6
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
				quad.V1,
				quad.V2,
				quad.V3,
				quad.V4
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
				triangle.V1,
				triangle.V2,
				triangle.V3
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
