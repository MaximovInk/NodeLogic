using UnityEngine;

namespace MaximovInk
{
    public class Pulse : EditableNode
    {
        public float Delay => value1;
        private float timer;
        
        private bool lastEnabled = false;
        
        public override void OnCircuitChange()
        {
            base.OnCircuitChange();

            if (!lastEnabled && InPoints[0].value)
            {
                lastEnabled = true;

                OutPoints[0].value = true;
                OutPoints[0].OnCircuitChanged();
            }

            lastEnabled = InPoints[0].value;
        }

        private void Update()
        {
            if (OutPoints[0].value)
            {
                timer += Time.deltaTime;

                if (timer > Delay)
                {
                    timer = 0;
                    OutPoints[0].value = false;
                    OutPoints[0].OnCircuitChanged();
                }
            }
        }
    }
}