using UnityEngine;

namespace MaximovInk
{
    public class PointIn : Point
    {
        public Line line;
        public PointOut Input;

        public void UpdateLine()
        {
            line.gameObject.SetActive(Input != null);
            line.width = 0.05f;
            if(Input!=null)
                line.start = Input.transform.position;
            
            line.end = transform.position;
        }

        public override void OnCircuitChanged()
        {
          

            if (Input == null)
                value = false;
            
            base.OnCircuitChanged();
            
            GetComponentInParent<Node>().OnCircuitChange();
            
            line.color = value ? Color.white : Color.black;
            
        }
    }
}