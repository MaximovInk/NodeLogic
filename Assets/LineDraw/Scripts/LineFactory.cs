using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores a pool of Line. Allows retrieval and initialisation of pooled line.
/// </summary>
public class LineFactory : MonoBehaviour
{
	/// <summary>
	/// The line prefab.
	/// </summary>
	public GameObject linePrefab;

	/// <summary>
	/// The number to pool. This is the maximum number of lines that can be retrieve from this factory.
	/// When a number of lines greater that this number is requested previous lines are overwritten.
	/// </summary>
	public int maxLines = 50;

	private Line[] pooledLines;
	private int currentIndex = 0;

	void Start ()
	{
		pooledLines = new Line[maxLines];
		
		for (int i = 0; i < maxLines; i++) {
			var line = Instantiate (linePrefab);
			line.SetActive (false);
			line.transform.SetParent (transform);
			pooledLines[i] = line.GetComponent<Line> ();
		}
	}

	/// <summary>
	/// Gets an initialised and active line. The line is retrieved from the pool and set active.
	/// </summary>
	/// <returns>The active line.</returns>
	/// <param name="start">Start position in world space.</param>
	/// <param name="end">End position in world space.</param>
	/// <param name="width">Width of line.</param>
	/// <param name="color">Color of line.</param>
	public Line GetLine (Vector2 start, Vector2 end, float width, Color color)
	{
		var line = pooledLines [currentIndex];
		
		line.Initialise (start, end, width, color);
		line.gameObject.SetActive (true);

		currentIndex = (currentIndex + 1) % pooledLines.Length;

		return line;
	}

	/// <summary>
	/// Returns all active lines.
	/// </summary>
	/// <returns>The active lines.</returns>
	public List<Line> GetActive()
	{
		var activeLines = new List<Line> ();

		foreach (var line in pooledLines) {
			if (line.gameObject.activeSelf) {
				activeLines.Add (line);
			}
		}

		return activeLines;
	}

}
