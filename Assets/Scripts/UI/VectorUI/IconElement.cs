using UnityEngine;
using UnityEngine.UIElements;


public class IconElement : VisualElement
{
     public new class UxmlFactory : UxmlElementAttribute { }

        public IconElement()
        {
            generateVisualContent += GenerateVisualContent;
        }
        
        private void GenerateVisualContent(MeshGenerationContext mgc)
        {
            var top = 0;
            var left = 0f;
            var right = contentRect.width;
            var bottom = contentRect.height;
            
            var painter2D = mgc.painter2D;
            painter2D.lineWidth = 10.0f;
            painter2D.strokeColor = Color.white;
            painter2D.lineJoin = LineJoin.Bevel;
            painter2D.lineCap = LineCap.Round;
            

            painter2D.BeginPath();
            
            painter2D.MoveTo(new Vector2((float)(left + (contentRect.width * 0.2)), (float)(top + contentRect.height * 0.1)));
            painter2D.LineTo(new Vector2((float)(left + (contentRect.width * 0.2)), (float)(bottom * 0.7)));
            
            painter2D.MoveTo(new Vector2((float)(right - (contentRect.width * 0.2)), (float)(top + contentRect.height * 0.1)));
            painter2D.LineTo(new Vector2((float)(right - (contentRect.width * 0.2)), (float)(bottom * 0.7)));
            
            painter2D.MoveTo(new Vector2((float)(left + (contentRect.width * 0.3)), (float)(bottom * 0.8)));
            painter2D.LineTo(new Vector2((float)(right - (contentRect.width * 0.3)), (float)(bottom * 0.8)));
            
            painter2D.Stroke();

       }
}
