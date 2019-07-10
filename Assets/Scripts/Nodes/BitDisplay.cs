using UnityEngine;

namespace MaximovInk
{
    public class BitDisplay : Node
    {
        public TextMesh text;

        public override void OnCircuitChange()
        {
            base.OnCircuitChange();
            text.text = InPoints[0].value ? "1" : "0";
        }
    }
}