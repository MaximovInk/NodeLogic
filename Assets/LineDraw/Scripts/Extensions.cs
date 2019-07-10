using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions 
{
	/// <summary>
	/// Returns vector with specified y.
	/// </summary>
	/// <returns>The y.</returns>
	/// <param name="a">Input vector.</param>
	/// <param name="y">The y coordinate.</param>
	public static Vector3 WithY(this Vector3 a, float y)
	{
		return new Vector3 (a.x, y, a.z);
	}
}
