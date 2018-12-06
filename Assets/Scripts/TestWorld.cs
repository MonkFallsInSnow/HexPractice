using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestWorld : MonoBehaviour {
	public int rows;
	public int columns;

	void Start () {
		CreateHexMap();
	}

	private void CreateHexMap()
	{
		GameObject map = new GameObject("Map");
		map.transform.parent = gameObject.transform;

		MeshFilter mf = map.AddComponent<MeshFilter>();
		MeshRenderer mr = map.AddComponent<MeshRenderer>();
		MeshCollider mc = map.AddComponent<MeshCollider>();

		mr.material = new Material(Shader.Find("Standard"));
		mr.material.color = new Color32(19, 114, 43, 255);

		HexChunk hexMap = new HexChunk(rows, columns, Vector3.zero);
		Mesh mesh = HexMeshGenerator.CreateHexMap(hexMap.MapData);
		mesh.name = "Hex Grid";
		mf.mesh = mesh;
		mc.sharedMesh = mesh;
	}

}
