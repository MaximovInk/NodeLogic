namespace MaximovInk
{
    public class SRLatch : Node
    {
        private bool value;
        
        public override void OnCircuitChange()
        {
            base.OnCircuitChange();
            if (InPoints[0].value)
                value = true;
            else if (InPoints[1].value)
                value = false;
            OutPoints[1].value = !value;
            OutPoints[0].value = value;
            OutPoints[0].OnCircuitChanged();
            OutPoints[1].OnCircuitChanged();
        }
    }
}