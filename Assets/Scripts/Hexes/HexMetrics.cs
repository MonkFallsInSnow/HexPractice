using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexDimensions 
{
	public static readonly float EdgeLength = 20f;
	public static readonly float OuterRadius = EdgeLength;
	public static readonly float InnerRadius = EdgeLength * (Mathf.Sqrt(3) * 0.5f);
	public static readonly float BridgeWidth = 1.25f;
	public static readonly float HorizontalSpacing = (InnerRadius * 2f) + BridgeWidth;
	public static readonly float VerticalSpacing = (OuterRadius * 1.5f) + BridgeWidth;
	public static readonly float ElevationStep = 1f;
}

public static class HexCorners
{
	public static readonly int NumCorners = 6;
	private static float cornerOffset = 0.5f;

	public static readonly Vector3 N = new Vector3(0f, 0f, HexDimensions.OuterRadius);
	public static readonly Vector3 NE = new Vector3(HexDimensions.InnerRadius, 0f, HexDimensions.OuterRadius * cornerOffset);
	public static readonly Vector3 SE = new Vector3(HexDimensions.InnerRadius, 0f, -(HexDimensions.OuterRadius * cornerOffset));
	public static readonly Vector3 S = new Vector3(0f, 0f, -HexDimensions.OuterRadius);
	public static readonly Vector3 SW = new Vector3(-HexDimensions.InnerRadius, 0f, -(HexDimensions.OuterRadius * cornerOffset));
	public static readonly Vector3 NW = new Vector3(-HexDimensions.InnerRadius, 0f, HexDimensions.OuterRadius * cornerOffset);

	public static readonly Vector3[] Corners = { N, NE, SE, S, SW, NW };
}

public static class HexNeighbors
{
	public static readonly Vector3 NE = new Vector3(HexDimensions.InnerRadius, 0f, HexDimensions.VerticalSpacing) + new Vector3(HexDimensions.BridgeWidth, 0f, HexDimensions.BridgeWidth);
	public static readonly Vector3 E = new Vector3(HexDimensions.HorizontalSpacing, 0f, 0f) + new Vector3(HexDimensions.BridgeWidth, 0f, 0f);
	public static readonly Vector3 SE = new Vector3(HexDimensions.InnerRadius, 0f, -HexDimensions.VerticalSpacing) + new Vector3(HexDimensions.BridgeWidth, 0f, -HexDimensions.BridgeWidth);
	public static readonly Vector3 SW = new Vector3(-HexDimensions.InnerRadius, 0f, -HexDimensions.VerticalSpacing) + new Vector3(-HexDimensions.BridgeWidth, 0f, -HexDimensions.BridgeWidth);
	public static readonly Vector3 W = new Vector3(-HexDimensions.InnerRadius, 0f, 0f) + new Vector3(-HexDimensions.BridgeWidth, 0f, 0f);

	//public static readonly Vector3[] Neighbors = { NE, E, SE, SW, W };
}



