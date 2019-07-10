namespace MaximovInk
{
    public class JKFlipFlop : Node
    {
        private bool lastC = true;
        public override void OnCircuitChange()
        {
            base.OnCircuitChange();
            var j = InPoints[0].value;
            var c = InPoints[1].value;
            var k = InPoints[2].value;
            
            if (j && k == false)
            {
                if (c != lastC && c)
                    OutPoints[0].value = true;
            }
            else if (j == false && k)
            {
                if (c != lastC && c)
                    OutPoints[0].value = false;
            }
            else if (j && k)
            {
                if (c != lastC && c)
                    OutPoints[0].value = !OutPoints[0].value;
            }

            lastC = c;
        
            OutPoints[1].value = !OutPoints[0].value;
            OutPoints[0].OnCircuitChanged();
            OutPoints[1].OnCircuitChanged();
        }
    }
}