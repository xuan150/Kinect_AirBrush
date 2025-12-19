using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;
//using KinectJoint = Microsoft.Azure.Kinect.BodyTracking.Joint; //也可以不在上面簡化，直接寫全名

public class HandReader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public main mainScript;

    // Update is called once per frame
    void Update()
    {
        if (mainScript.m_lastFrameData.NumOfBodies > 0)
        {
            Body human = mainScript.m_lastFrameData.Bodies[0]; //抓畫面中的第一個人的數據
            var rightHandPos = human.JointPositions3D[(int)JointId.HandRight]; //Joint是關節;human.JointPositions3D是這個人的所有數據(32格的陣列)

            Debug.Log($"右手的x{rightHandPos.X}，右手的y{rightHandPos.Y}，右手的z{rightHandPos.Z}");
        }
        else Debug.Log("運作中但沒人");
    }
}
