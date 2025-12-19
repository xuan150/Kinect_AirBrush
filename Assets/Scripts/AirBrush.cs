using System.ComponentModel;
using System.Diagnostics.Contracts;
//using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
//using System.Threading.Tasks.Dataflow;
using UnityEngine;

public class AirBrush : MonoBehaviour
{
    [Header("Target Setting")]
    public Animator puppetAnimator;
    public bool isRightHand = true;

    Transform indexJoint;
    Transform thumbJoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isRightHand) //鏡像要是左手
        {
            indexJoint = puppetAnimator.GetBoneTransform(HumanBodyBones.RightIndexDistal);
            thumbJoint = puppetAnimator.GetBoneTransform(HumanBodyBones.RightThumbDistal);
        }
        else
        {
            indexJoint = puppetAnimator.GetBoneTransform(HumanBodyBones.LeftIndexDistal);
            thumbJoint = puppetAnimator.GetBoneTransform(HumanBodyBones.LeftThumbDistal);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        if (indexJoint != null && thumbJoint != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(indexJoint.position, 0.02f);
            Gizmos.DrawWireSphere(thumbJoint.position, 0.02f);

            // 畫一條線連線兩指，方便觀察距離
            Gizmos.DrawLine(indexJoint.position, thumbJoint.position);
        }
    }
}
