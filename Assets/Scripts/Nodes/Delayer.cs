using UnityEngine;

namespace MaximovInk
{
    public class Delayer : EditableNode
    {
        public float Delay => value1;

        private bool value;
        private float timer;
        private bool dir;

        private void Update()
        {
            dir = InPoints[0].value;
            
            
            
            if (dir)
            {
                if (timer > Delay)
                {
                    OutPoints[0].value = true;
                    OutPoints[0].OnCircuitChanged();
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
            else
            {
                if (timer < 0)
                {
                    OutPoints[0].value = false;
                    OutPoints[0].OnCircuitChanged();
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }

        }
    }
}