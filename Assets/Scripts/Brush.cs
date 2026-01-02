using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;

public class Brush : MonoBehaviour
{
    public GameObject strokePrefab;
    public Transform strokeParent;
    float MIN_DISTANCE = 0.005f;
    StrokeFader fader;
    Vector3 lastPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartDraw()
    {
        GameObject nowStroke = Instantiate(strokePrefab); //物件實例化(即在遊戲中生成原本不存在的物件)
        nowStroke.transform.SetParent(strokeParent); //=nowStroke.transform.parent=strokeParent.transform，子前父後

        LineRenderer line = nowStroke.GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.useWorldSpace = true; //線條以世界座標為基礎，不論掛在誰下

        fader = nowStroke.AddComponent<StrokeFader>();
        float lifeTime = 7f;
        fader.StartNewTimer(lifeTime);
    }

    // Update is called once per frame
    public void UpdateDraw(Vector3 nowPosition)
    {
        if (fader == null || Vector3.Distance(lastPos, nowPosition) < MIN_DISTANCE) return;

        fader.AddPoint(nowPosition);
        lastPos = nowPosition;
    }

    public void EndDraw()
    {
        if (fader == null) return;
        fader.EndFade();
        fader = null;
    }
}

public class StrokeFader : MonoBehaviour //新增一個僅內部可呼叫的component，管理每個筆畫的壽命

{
    class PointData
    {
        public Vector3 position;
        public float birthTime;
        public PointData(Vector3 pos, float time)
        {
            position = pos;
            birthTime = time;
        }
    }
    LineRenderer line;
    float lifeTime;
    bool isFadeEnd = false;
    Queue<PointData> points = new Queue<PointData>(); //有關List參考 https://learn.microsoft.com/zh-tw/dotnet/api/system.collections.generic.list-1?view=net-8.0

    public void StartNewTimer(float duration)
    {
        line = GetComponent<LineRenderer>();
        lifeTime = duration;
    }
    public void AddPoint(Vector3 point)
    {
        points.Enqueue(new PointData(point, Time.time));
        UpdateLineRenderer();
    }
    public void EndFade()
    {
        isFadeEnd = true;
    }
    void Update()
    {
        if (points.Count == 0 && isFadeEnd == true)
        {
            Destroy(gameObject);
            return;
        }
        while (points.Count > 0 && Time.time - points.Peek().birthTime > lifeTime)
        {
            points.Dequeue();
            if (points.Count > 0) UpdateLineRenderer();
        }
    }
    void UpdateLineRenderer()
    {
        Vector3[] pos = points.Select(p => p.position).ToArray();

        line.positionCount = pos.Length; //要更新現在共有幾個座標數值，才會根據此數字去畫對應的點
        line.SetPositions(pos); //setposition有加s的話是把所有點都重畫一次，所以放點集合的位置
    }
}