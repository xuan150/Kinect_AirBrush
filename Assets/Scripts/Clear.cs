using UnityEngine;
using System.Collections.Generic;

public class Clear : MonoBehaviour
{
    [SerializeField] Transform allStrokes;

    public void ClearAllStrokes()
    {
        Debug.Log("CLEAR CLICKED");
        Debug.Log("children = " + allStrokes.childCount);

        foreach (Transform child in allStrokes)
            Destroy(child.gameObject);
    }
}
