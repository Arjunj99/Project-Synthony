using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ClippingScript : MonoBehaviour
{
	private Vector2 LineStart, LineEnd;
	public float crossScale = 0.05f;

	Vector2[] polygonPoints;
	PolygonCollider2D polygon;
	List<Vector2> clipPoints = new List<Vector2> ();

	
	// Update is called once per frame
	public List<Vector2> clips(Vector2 inLineStart, Vector2 inLineEnd, PolygonCollider2D inPolygon)
	{
        LineStart = inLineStart;
        LineEnd = inLineEnd;
        polygon = inPolygon;
		// Fetch the polygon points in world-space (not the most efficient way of doing things)!
		polygonPoints = polygon.points;
		for (var i = 0; i < polygonPoints.Length; ++i)
			polygonPoints[i] = polygon.gameObject.transform.TransformPoint(polygonPoints[i]);
		// Perform clipping against the polygon.
		clipPoints.Clear();
		var pointCount = polygonPoints.Length;
		for (int i = 0, j = pointCount - 1; i < pointCount; j = i++)
		{
			var edgeStart = polygonPoints[i];
			var edgeEnd = polygonPoints[j];
            Debug.DrawLine(edgeStart, edgeEnd, Color.cyan, 2f);

			Vector2 clipPoint;
			if (LineSegmentIntersection (LineStart, LineEnd, edgeStart, edgeEnd, out clipPoint))
            {
                clipPoints.Add(clipPoint);
            }
        }

		// Draw the clipping line.
		Debug.DrawLine (LineStart, LineEnd, Color.green, 2f);
	    
		// Draw the clip points.
		foreach (var v in clipPoints)
		{
			var clipPoint = (Vector3)v;
			Debug.DrawLine (clipPoint + (Vector3.left * crossScale), clipPoint + (Vector3.right * crossScale), Color.white);
			Debug.DrawLine (clipPoint + (Vector3.up * crossScale), clipPoint + (Vector3.down * crossScale), Color.white);
		}

        return clipPoints;
	}

	bool LineSegmentIntersection (Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 point)
	{
		// Sign of areas correspond to which side of ab points c and d are.
		float area1 = SignedTriangleArea (a, b, d);
		float area2 = SignedTriangleArea (a, b, c);

		// If c and d are on different sides of ab, areas have different signs.
		if (area1 * area2 < 0.0f)
		{
			// Compute signs for a and b with respect to segment cd.
			float area3 = SignedTriangleArea (c, d, a);
			float area4 = area3 + area2 - area1;

			// Points a and b on different sides of cd if areas have different signs.
			if (area3 * area4 < 0.0f)
			{
				float time = area3 / (area3 - area4);
				point = a + time * (b - a);
				return true;
			}
		}

		// Segments are not intersecting or collinear.
		point = Vector2.zero;
		return false;
	}

	float SignedTriangleArea (Vector2 a, Vector2 b, Vector2 c)
	{
		return (a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x);
	}

}
