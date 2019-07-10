using UnityEngine;

namespace MaximovInk
{
    public class ColorDisplay : Node
    {
       
        private SpriteRenderer sprite {
            get
            {
                if (sp == null)
                    sp = GetComponent<SpriteRenderer>();
                return sp;
            }
        }
        private SpriteRenderer sp;

        private float GetColorValue(bool value)
        {
            return value ? 1 : 0;
        }

        public override void OnCircuitChange()
        {
            base.OnCircuitChange();
            sprite.color = new Color(GetColorValue(InPoints[0].value),GetColorValue(InPoints[1].value),GetColorValue(InPoints[2].value) );
        }
    }
}