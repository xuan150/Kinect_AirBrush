using UnityEngine;
using System.Collections.Generic;

public class MovingSmooth : MonoBehaviour
{
    [Range(1, 10)]
    public int smoothFrame = 5;
    Queue<Vector3> historyPositions = new Queue<Vector3>(); //佇列,https://learn.microsoft.com/zh-tw/dotnet/api/system.collections.generic.queue-1?view=net-8.0#code-try-4
    Vector3 sumPosition;

    public void ResetSmooth()
    {
        historyPositions.Clear();
        sumPosition = Vector3.zero;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 GetSmoothPosition(Vector3 rawPosition)
    {
        historyPositions.Enqueue(rawPosition);
        sumPosition += rawPosition;

        if (historyPositions.Count > smoothFrame)
        {
            Vector3 oldPos = historyPositions.Dequeue();
            sumPosition -= oldPos;
        }

        return sumPosition / historyPositions.Count;
    }
}
