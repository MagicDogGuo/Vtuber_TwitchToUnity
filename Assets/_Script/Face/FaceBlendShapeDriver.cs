using System;
using UnityEngine;

public class FaceBlendShapeDriver : MonoBehaviour
{
    [Header("Source of face parameters")]
    public Momose source; // Assign Momose in the scene

    [Header("Target renderer")]
    public SkinnedMeshRenderer skinnedMeshRenderer;

    [Header("Head transform control (optional)")]
    public Transform headTarget; // assign a head/neck bone transform
    public bool driveHeadRotation = true;
    public bool driveHeadPosition = false;
    [Tooltip("Degrees multiplier for AngleX/Y/Z -> localEulerAngles")]
    public Vector3 rotationMultiplier = new Vector3(1f, 1f, 1f);
    [Tooltip("Units multiplier for AngleX/Y/Z -> localPosition (X,Y,Z)")]
    public Vector3 positionMultiplier = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 headRotationOffsetDeg = Vector3.zero;
    public Vector3 headPositionOffset = Vector3.zero;

    [Header("BlendShape names (0-100)")]
    public string eyeCloseLeftName = "Face.M_F00_000_00_Fcl_EYE_Close_L";
    public string eyeCloseRightName = "Face.M_F00_000_00_Fcl_EYE_Close_R";
    public string eyebrowDownLeftName = "Face.M_F00_000_00_Fcl_BRW_Sorrow"; // example mapping
    public string eyebrowDownRightName = "Face.M_F00_000_00_Fcl_BRW_Sorrow"; // same shape both sides depending on your asset
    public string mouthOpenName = "Face.M_F00_000_00_Fcl_MTH_Open"; // adjust to your asset
    public string mouthWideName = "Face.M_F00_000_00_Fcl_MTH_Wide"; // adjust to your asset

    [Header("Smoothing")]
    [Range(0f, 1f)] public float smoothing = 0.5f; // 0: snappy, 1: very smooth

    // Cached indices
    private int eyeCloseLeftIdx = -1;
    private int eyeCloseRightIdx = -1;
    private int eyebrowDownLeftIdx = -1;
    private int eyebrowDownRightIdx = -1;
    private int mouthOpenIdx = -1;
    private int mouthWideIdx = -1;

    // Runtime state for smoothing
    private float eyeCloseLeftW;
    private float eyeCloseRightW;
    private float eyebrowDownLeftW;
    private float eyebrowDownRightW;
    private float mouthOpenW;
    private float mouthWideW;

    // Head smoothing state
    private Vector3 currentEuler; // degrees
    private Vector3 currentLocalPos; // units

    void Awake()
    {
        if (skinnedMeshRenderer != null)
        {
            eyeCloseLeftIdx = TryGetBlendShapeIndex(eyeCloseLeftName);
            eyeCloseRightIdx = TryGetBlendShapeIndex(eyeCloseRightName);
            eyebrowDownLeftIdx = TryGetBlendShapeIndex(eyebrowDownLeftName);
            eyebrowDownRightIdx = TryGetBlendShapeIndex(eyebrowDownRightName);
            mouthOpenIdx = TryGetBlendShapeIndex(mouthOpenName);
            mouthWideIdx = TryGetBlendShapeIndex(mouthWideName);
        }
    }

    void LateUpdate()
    {
        if (source == null || skinnedMeshRenderer == null) return;

        // Map parameters to 0..100 weights
        float targetEyeCloseL = Mathf.Clamp01(1f - source.EyeOpenLeft) * 100f;
        float targetEyeCloseR = Mathf.Clamp01(1f - source.EyeOpenRight) * 100f;

        // Eyebrow: map from [-1..1] sorrow-up to anger-down depending on asset; here we use down as positive
        float targetBrowDownL = Mathf.Clamp01((source.EyebrowLeft + 1f) * 0.5f) * 100f;
        float targetBrowDownR = Mathf.Clamp01((source.EyebrowRight + 1f) * 0.5f) * 100f;

        float targetMouthOpen = Mathf.Clamp01(source.MouthOpen) * 100f;
        float targetMouthWide = Mathf.Clamp01(source.MouthWidth) * 100f;

        // Smooth towards target
        float k = Mathf.Clamp01(1f - smoothing);
        eyeCloseLeftW = Mathf.Lerp(eyeCloseLeftW, targetEyeCloseL, k);
        eyeCloseRightW = Mathf.Lerp(eyeCloseRightW, targetEyeCloseR, k);
        eyebrowDownLeftW = Mathf.Lerp(eyebrowDownLeftW, targetBrowDownL, k);
        eyebrowDownRightW = Mathf.Lerp(eyebrowDownRightW, targetBrowDownR, k);
        mouthOpenW = Mathf.Lerp(mouthOpenW, targetMouthOpen, k);
        mouthWideW = Mathf.Lerp(mouthWideW, targetMouthWide, k);

        // Apply if indices are valid
        if (eyeCloseLeftIdx >= 0) skinnedMeshRenderer.SetBlendShapeWeight(eyeCloseLeftIdx, eyeCloseLeftW);
        if (eyeCloseRightIdx >= 0) skinnedMeshRenderer.SetBlendShapeWeight(eyeCloseRightIdx, eyeCloseRightW);
        if (eyebrowDownLeftIdx >= 0) skinnedMeshRenderer.SetBlendShapeWeight(eyebrowDownLeftIdx, eyebrowDownLeftW);
        if (eyebrowDownRightIdx >= 0) skinnedMeshRenderer.SetBlendShapeWeight(eyebrowDownRightIdx, eyebrowDownRightW);
        if (mouthOpenIdx >= 0) skinnedMeshRenderer.SetBlendShapeWeight(mouthOpenIdx, mouthOpenW);
        if (mouthWideIdx >= 0) skinnedMeshRenderer.SetBlendShapeWeight(mouthWideIdx, mouthWideW);

        // Head transform control
        if (headTarget != null)
        {
            if (driveHeadRotation)
            {
                Vector3 targetEuler = new Vector3(
                    -source.AngleY * rotationMultiplier.x,
                    source.AngleX * rotationMultiplier.y,
                    source.AngleZ * rotationMultiplier.z) + headRotationOffsetDeg;
                currentEuler = Vector3.Lerp(currentEuler, targetEuler, k);
                headTarget.localRotation = Quaternion.Euler(currentEuler);
            }

            if (driveHeadPosition)
            {
                Vector3 targetPos = new Vector3(
                    -source.AngleY * positionMultiplier.x,
                    source.AngleX * positionMultiplier.y,
                    source.AngleZ * positionMultiplier.z) + headPositionOffset;
                currentLocalPos = Vector3.Lerp(currentLocalPos, targetPos, k);
                headTarget.localPosition = currentLocalPos;
            }
        }
    }

    private int TryGetBlendShapeIndex(string shapeName)
    {
        if (string.IsNullOrEmpty(shapeName)) return -1;
        var mesh = skinnedMeshRenderer.sharedMesh;
        if (mesh == null) return -1;
        int idx = mesh.GetBlendShapeIndex(shapeName);
        if (idx < 0)
        {
            Debug.LogWarning($"[FaceBlendShapeDriver] BlendShape not found: {shapeName}");
        }
        return idx;
    }
}


