using UnityEngine;

namespace  MaximovInk
{	
	public class Node : MonoBehaviour
	{

		public PointIn[] InPoints { get; private set; }

		public PointOut[] OutPoints { get; private set; }

		public int instanceId;

		private void Awake()
		{

			InPoints = GetComponentsInChildren<PointIn>();
			OutPoints = GetComponentsInChildren<PointOut>();
			for (var i = 0; i < OutPoints.Length; i++)
			{
				OutPoints[i].id = i;
			}
		}

		public void RemoveNode()
		{
			if (InPoints != null)
			{
				foreach (var InPoint in InPoints)
				{
					if(InPoint.Input != null)
						InPoint.Input.Outs.Remove(InPoint);
				}
			}

			if (OutPoints != null)
			{
				foreach (var OutPoint in OutPoints)
				{
					foreach (var Out in OutPoint.Outs)
					{
						Out.Input= null;
						Out.value = false;
						Out.UpdateLine();
						Out.OnCircuitChanged();
					}
				}
			}
			Destroy(gameObject);
		}

		public void PositionChanged()
		{
			transform.position = new Vector3(
				Mathf.Round(transform.position.x/MainManager.instance.NodeSnap)*MainManager.instance.NodeSnap
				,
				Mathf.Round(transform.position.y/MainManager.instance.NodeSnap)*MainManager.instance.NodeSnap
			);

			if (InPoints != null)
			{
				foreach (var InPoint in InPoints)
				{
					InPoint.UpdateLine();
				}
			}

			if (OutPoints != null)
			{
				foreach (var OutPoint in OutPoints)
				{
					foreach (var Out in OutPoint.Outs)
					{
						Out.UpdateLine();
					}
				}
			}
		}

		public virtual void OnCircuitChange()
		{
			if(OutPoints == null)
				return;
			
			foreach (var Out in OutPoints)
			{
				Out.OnCircuitChanged();
			}
		}
	}
}
