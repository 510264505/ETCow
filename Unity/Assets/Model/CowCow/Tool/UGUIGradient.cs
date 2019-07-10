using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUIGradient : BaseMeshEffect
{
    public Color topColor, bottomColor;

    public override void ModifyMesh(VertexHelper vh)
    {
        var vertexList = new List<UIVertex>();
        vh.GetUIVertexStream(vertexList);
        int count = vertexList.Count;

        if (ApplyGradient(vertexList, 0, count))
        {
            vh.Clear();
            vh.AddUIVertexTriangleStream(vertexList);
        }
    }

    protected bool ApplyGradient(List<UIVertex> vertexList, int start, int end)
    {
        if (vertexList.Count == 0) return false;
        float bottomY = vertexList[0].position.y;
        float topY = vertexList[0].position.y;
        for (int i = start; i < end; ++i)
        {
            float y = vertexList[i].position.y;
            if (y > topY)
            {
                topY = y;
            }
            else if (y < bottomY)
            {
                bottomY = y;
            }
        }

        float uiElementHeight = topY - bottomY;
        for (int i = start; i < end; ++i)
        {
            UIVertex uiVertex = vertexList[i];
            uiVertex.color = Color.Lerp(bottomColor, topColor, (uiVertex.position.y - bottomY) / uiElementHeight);
            vertexList[i] = uiVertex;
        }
        return true;
    }
}
