using UnityEngine;

namespace MaximovInk
{
    public class PushButton : Node
    {
        private void OnMouseDown()
        {
            OutPoints[0].value = true;
            OutPoints[0].OnCircuitChanged();
        }

        private void OnMouseUp()
        {
            OutPoints[0].value = false;
            OutPoints[0].OnCircuitChanged();
        }
    }
}