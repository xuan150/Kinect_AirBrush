using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;
using System.Text;

public class PuppetAvatar : MonoBehaviour
{
    public TrackerHandler KinectDevice;
    Dictionary<JointId, Quaternion> absoluteOffsetMap;
    Animator PuppetAnimator;
    public GameObject RootPosition;
    public Transform CharacterRootTransform;
    public float OffsetY;
    public float OffsetZ;
    [Header("Mirror Setting")]
    public bool mirrorMode = true; //創建一個可以在unity打勾的選項
    private Quaternion TurnAround = Quaternion.Euler(0, 180, 0);
    private JointId GetMirrorTarget(JointId original)
    {
        switch (original)
        {
            case JointId.ClavicleLeft: return JointId.ClavicleRight;
            case JointId.ClavicleRight: return JointId.ClavicleLeft;
            case JointId.ShoulderLeft: return JointId.ShoulderRight;
            case JointId.ShoulderRight: return JointId.ShoulderLeft;
            case JointId.ElbowLeft: return JointId.ElbowRight;
            case JointId.ElbowRight: return JointId.ElbowLeft;
            case JointId.WristLeft: return JointId.WristRight;
            case JointId.WristRight: return JointId.WristLeft;
            case JointId.HandLeft: return JointId.HandRight;
            case JointId.HandRight: return JointId.HandLeft;
            case JointId.HandTipLeft: return JointId.HandTipRight;
            case JointId.HandTipRight: return JointId.HandTipLeft;
            case JointId.ThumbLeft: return JointId.ThumbRight;
            case JointId.ThumbRight: return JointId.ThumbLeft;
            case JointId.HipLeft: return JointId.HipRight;
            case JointId.HipRight: return JointId.HipLeft;
            case JointId.KneeLeft: return JointId.KneeRight;
            case JointId.KneeRight: return JointId.KneeLeft;
            case JointId.AnkleLeft: return JointId.AnkleRight;
            case JointId.AnkleRight: return JointId.AnkleLeft;
            case JointId.FootLeft: return JointId.FootRight;
            case JointId.FootRight: return JointId.FootLeft;
            default: return original;
        }
    }
    private static HumanBodyBones MapKinectJoint(JointId joint)
    {
        // https://docs.microsoft.com/en-us/azure/Kinect-dk/body-joints
        switch (joint)
        {
            case JointId.Pelvis: return HumanBodyBones.Hips;
            case JointId.SpineNavel: return HumanBodyBones.Spine;
            case JointId.SpineChest: return HumanBodyBones.Chest;
            case JointId.Neck: return HumanBodyBones.Neck;
            case JointId.Head: return HumanBodyBones.Head;
            case JointId.HipLeft: return HumanBodyBones.LeftUpperLeg;
            case JointId.KneeLeft: return HumanBodyBones.LeftLowerLeg;
            case JointId.AnkleLeft: return HumanBodyBones.LeftFoot;
            case JointId.FootLeft: return HumanBodyBones.LeftToes;
            case JointId.HipRight: return HumanBodyBones.RightUpperLeg;
            case JointId.KneeRight: return HumanBodyBones.RightLowerLeg;
            case JointId.AnkleRight: return HumanBodyBones.RightFoot;
            case JointId.FootRight: return HumanBodyBones.RightToes;
            case JointId.ClavicleLeft: return HumanBodyBones.LeftShoulder;
            case JointId.ShoulderLeft: return HumanBodyBones.LeftUpperArm;
            case JointId.ElbowLeft: return HumanBodyBones.LeftLowerArm;
            case JointId.WristLeft: return HumanBodyBones.LeftHand;
            case JointId.ClavicleRight: return HumanBodyBones.RightShoulder;
            case JointId.ShoulderRight: return HumanBodyBones.RightUpperArm;
            case JointId.ElbowRight: return HumanBodyBones.RightLowerArm;
            case JointId.WristRight: return HumanBodyBones.RightHand;
            default: return HumanBodyBones.LastBone;
        }
    }
    private void Start()
    {
        PuppetAnimator = GetComponent<Animator>();
        Transform _rootJointTransform = CharacterRootTransform;

        absoluteOffsetMap = new Dictionary<JointId, Quaternion>();
        for (int i = 0; i < (int)JointId.Count; i++)
        {
            HumanBodyBones hbb = MapKinectJoint((JointId)i);
            if (hbb != HumanBodyBones.LastBone)
            {
                Transform transform = PuppetAnimator.GetBoneTransform(hbb);
                Quaternion absOffset = GetSkeletonBone(PuppetAnimator, transform.name).rotation;
                // find the absolute offset for the tpose
                while (!ReferenceEquals(transform, _rootJointTransform))
                {
                    transform = transform.parent;
                    absOffset = GetSkeletonBone(PuppetAnimator, transform.name).rotation * absOffset;
                }
                absoluteOffsetMap[(JointId)i] = absOffset;
            }
        }
    }

    private static SkeletonBone GetSkeletonBone(Animator animator, string boneName)
    {
        int count = 0;
        StringBuilder cloneName = new StringBuilder(boneName);
        cloneName.Append("(Clone)");
        foreach (SkeletonBone sb in animator.avatar.humanDescription.skeleton)
        {
            if (sb.name == boneName || sb.name == cloneName.ToString())
            {
                return animator.avatar.humanDescription.skeleton[count];
            }
            count++;
        }
        return new SkeletonBone();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        for (int j = 0; j < (int)JointId.Count; j++)
        {
            if (MapKinectJoint((JointId)j) != HumanBodyBones.LastBone && absoluteOffsetMap.ContainsKey((JointId)j))
            {
                // get the absolute offset
                Quaternion absOffset = absoluteOffsetMap[(JointId)j];
                Transform finalJoint = PuppetAnimator.GetBoneTransform(MapKinectJoint((JointId)j));

                int sourceDataIndex = j;
                if (mirrorMode)
                {
                    sourceDataIndex = (int)GetMirrorTarget((JointId)j);
                }
                Quaternion kinectRot = KinectDevice.absoluteJointRotations[sourceDataIndex];

                if (mirrorMode)
                {
                    kinectRot = new Quaternion(-kinectRot.x, kinectRot.y, kinectRot.z, -kinectRot.w);
                    kinectRot = TurnAround * kinectRot;
                }
                finalJoint.rotation = kinectRot * absOffset;

                if (j == (int)JointId.Pelvis)
                {
                    // 讀取 Kinect 的位移
                    float rootPosX = RootPosition.transform.localPosition.x;
                    float rootPosY = RootPosition.transform.localPosition.y;
                    float rootPosZ = RootPosition.transform.localPosition.z;

                    if (mirrorMode)
                    {
                        rootPosX = -1 * rootPosX; // 左右移動反轉
                        rootPosZ = -1 * rootPosZ;
                    }

                    finalJoint.position = CharacterRootTransform.position + new Vector3(
                        rootPosX,
                        rootPosY + OffsetY,
                        rootPosZ - OffsetZ
                    );
                }
            }
        }
    }
}
