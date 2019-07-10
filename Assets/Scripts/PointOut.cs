using System.Collections.Generic;

namespace MaximovInk
{
    public class PointOut : Point
    {
        public List<PointIn> Outs = new List<PointIn>();
        public int id;
        public override void OnCircuitChanged()
        {
            base.OnCircuitChanged();
            
            foreach (var Out in Outs)
            {
                if (Out.value == value)
                    continue;
                Out.value = value;
                    
                Out.OnCircuitChanged();
            }
        }
    }
}