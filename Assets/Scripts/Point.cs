using UnityEngine;

namespace MaximovInk
{
    public class Point : MonoBehaviour
    {
        public bool value;
        protected Color deactivated = Color.black;
        protected Color activated = Color.white;
        
        
        [HideInInspector]
        public Node node;

        private SpriteRenderer sp;

        private void Awake()
        {
            sp = GetComponent<SpriteRenderer>();
        }

        public virtual void OnCircuitChanged()
        {
            sp.color = value ? activated : deactivated;
        }

        private void Start()
        {
            transform.localPosition = new Vector3(
                Mathf.Round(transform.localPosition.x/MainManager.instance.PointsSnap)*MainManager.instance.PointsSnap
                ,
                Mathf.Round(transform.localPosition.y/MainManager.instance.PointsSnap)*MainManager.instance.PointsSnap,
                1
            );

            node = GetComponentInParent<Node>();
            OnCircuitChanged();
        }
        
        
    }
}