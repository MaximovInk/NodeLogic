using UnityEngine;

namespace MaximovInk
{
    public class Clock : EditableNode
    {
        public float Delay => value1;
        private float timer = 0;
        private void Update()
        {
            timer += Time.deltaTime;

            if (!InPoints[0].value)
            {
                return;
            }

            if (InPoints[1].value)
            {
                OutPoints[0].value = false;
                timer = 0;
            }




            if (timer > Delay)
            {
                OutPoints[0].value = !OutPoints[0].value;
                OutPoints[0].OnCircuitChanged();
                
                timer = 0;
            }
        }
    }
}