namespace MaximovInk
{
    public class ToggleButton : Node
    {
        private void OnMouseDown()
        {
            OutPoints[0].value = !OutPoints[0].value;
            OutPoints[0].OnCircuitChanged();
        }
    }
}