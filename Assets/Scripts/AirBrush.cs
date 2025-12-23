// using System.Collections.Specialized;
// using System.ComponentModel;
// using System.Data.SqlTypes;
// using System.Diagnostics.Contracts;
// //using System.Drawing;
// //using System.Numerics;
// using System.Runtime.CompilerServices;
// using System.Runtime.ExceptionServices;
// using System.Runtime.InteropServices;
// //using System.Runtime.Intrinsics.X86;
// using System.Security.Cryptography.X509Certificates;
// //using System.Threading.Tasks.Dataflow;
using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;

public class AirBrush : MonoBehaviour
{
    [Header("Target Setting")]
    public TrackerHandler KinectDevice;
    public Animator puppetAnimator;
    public bool isRightHand = true;

    [Header("Inspector")]
    public string currentHandStateString; //Open/Closed/Unknown
    bool isOpen;
    //public Material lineMaterial;

    // Transform indexJoint;
    // Transform thumbJoint;
    Transform drawPoint;
    //int lastStateValue = -1;   
    // LineRender drawLine;
    float OPENDIS = 0.05f; //內建距離是公尺
    public float currentDistance = 0f;
    //bool isDrawing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (!isRightHand) //鏡像要是左手
        {
            drawPoint = puppetAnimator.GetBoneTransform(HumanBodyBones.RightHand);

            //indexJoint = puppetAnimator.GetBoneTransform(HumanBodyBones.RightIndexDistal);
            //thumbJoint = puppetAnimator.GetBoneTransform(HumanBodyBones.RightThumbDistal);
        }
        else
        {
            drawPoint = puppetAnimator.GetBoneTransform(HumanBodyBones.LeftHand);

            //indexJoint = puppetAnimator.GetBoneTransform(HumanBodyBones.LeftIndexDistal);
            //thumbJoint = puppetAnimator.GetBoneTransform(HumanBodyBones.LeftThumbDistal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (KinectDevice == null || KinectDevice.currentBody.Length == 0 || KinectDevice.currentBody.JointPositions3D == null) return;
       // if (KinectDevice.currentBody.Length == 0) return;
       // if (KinectDevice.currentBody.JointPositions3D == null) return;

        int thumbIndex, tipIndex;
        if (!isRightHand)
        {
            thumbIndex = (int)JointId.ThumbRight;
            tipIndex = (int)JointId.HandTipRight;
        }
        else
        {
            thumbIndex = (int)JointId.ThumbLeft;
            tipIndex = (int)JointId.HandTipLeft;
        }

        var rawThumbPos = KinectDevice.currentBody.JointPositions3D[thumbIndex];
        var rawTipPos = KinectDevice.currentBody.JointPositions3D[tipIndex];

        // 3. 手動計算距離 (畢氏定理)
        // Distance = sqrt( (x1-x2)^2 + (y1-y2)^2 + (z1-z2)^2 )
        float dx = rawTipPos.X - rawThumbPos.X;
        float dy = rawTipPos.Y - rawThumbPos.Y;
        float dz = rawTipPos.Z - rawThumbPos.Z;

        currentDistance = Mathf.Sqrt(dx * dx + dy * dy + dz * dz);

        // 4. 判斷是否握拳 (距離小於門檻)
        if (currentDistance < OPENDIS)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
        // if (Vector3.Distance(indexJoint.position, thumbJoint.position) < OPENDIS) isOpen = true;
        // else isOpen = false;
    }

    void OnDrawGizmos()
    {
        if (drawPoint == null) return; //避免在程式未執行時一直讀這而報錯

        Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(indexJoint.position, thumbJoint.position);
        if (isOpen)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(drawPoint.position, 0.03f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(drawPoint.position, 0.02f);
        }
    }
}