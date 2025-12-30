using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;
//using System.ComponentModel.DataAnnotations;
//using System.Numerics;


public class AirControl : MonoBehaviour
{
    [Header("Target Settings")]
    public TrackerHandler KinectDevice;
    public Animator puppetAnimator;
    public bool isRightHand = true;
    public Brush brushControler;

    [Header("Smooth Settings")]
    [Range(1f, 30f)]
    public float smoothIndex = 10f;
    public MovingSmooth smoother;
    bool isOpen;
    Transform drawPoint;
    float OPENDIS = 0.05f; //內建距離是公尺
    float heightDiff = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //鏡像要是左手
        drawPoint = (!isRightHand) ? puppetAnimator.GetBoneTransform(HumanBodyBones.RightIndexDistal) : puppetAnimator.GetBoneTransform(HumanBodyBones.LeftIndexDistal);
        //nowSmoothPos = drawPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (KinectDevice == null || KinectDevice.currentBody.Length == 0 || KinectDevice.currentBody.JointPositions3D == null) return;

        Vector3 targetPos = drawPoint.position;
        Vector3 nowSmoothPos = smoother.GetSmoothPosition(targetPos);

        int switchIndex;
        int spineIndex = (int)JointId.SpineChest; //基準點胸口
        switchIndex = (!isRightHand) ? (int)JointId.HandRight : (int)JointId.HandLeft;

        var rawSwitchPos = KinectDevice.currentBody.JointPositions3D[switchIndex];
        var rawSpinePos = KinectDevice.currentBody.JointPositions3D[spineIndex];

        heightDiff = rawSpinePos.Y - rawSwitchPos.Y;

        if (heightDiff > OPENDIS)
        {
            if (!isOpen)
            {
                //nowSmoothPos = targetPos;
                smoother.ResetSmooth();

                isOpen = true;
                for (int i = 0; i < 5; i++) smoother.GetSmoothPosition(targetPos);
                brushControler.StartDraw();
            }
            else brushControler.UpdateDraw(nowSmoothPos);
        }
        else
        {
            isOpen = false;
            brushControler.EndDraw();
            smoother.ResetSmooth();
        }
    }

    void OnDrawGizmos()
    {
        if (drawPoint == null) return; //避免在程式未執行時一直讀這而報錯

        Gizmos.color = isOpen ? Color.green : Color.red;
        Gizmos.DrawSphere(drawPoint.position, 0.03f);
    }
}