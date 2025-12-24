//using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public GameObject strokePrefab;
    public Transform strokeParent;
    float MIN_DISTANCE = 0.005f;
    LineRenderer line;
    //儲存所有點
    List<Vector3> points = new List<Vector3>(); //參考 https://learn.microsoft.com/zh-tw/dotnet/api/system.collections.generic.list-1?view=net-8.0

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartDraw()
    {
        GameObject nowStroke = Instantiate(strokePrefab); //物件實例化(即在遊戲中生成原本不存在的物件)

        nowStroke.transform.SetParent(strokeParent); //=nowStroke.transform.parent=strokeParent.transform，子前父後
        line = nowStroke.GetComponent<LineRenderer>();

        points.Clear();
        line.positionCount = 0;
        line.useWorldSpace = true; //線條以世界座標為基礎，不論掛在誰下
    }

    // Update is called once per frame
    public void UpdateDraw(Vector3 nowPosition)
    {
        if (line == null) return;
        if (points.Count > 0 && Vector3.Distance(points[points.Count - 1], nowPosition) < MIN_DISTANCE) return;

        points.Add(nowPosition);
        line.positionCount = points.Count; //要更新現在共有幾個座標數值，才會根據此數字去畫對應的點
        line.SetPosition(points.Count - 1, nowPosition);
    }

    public void EndDraw()
    {
        line = null;
    }
}
