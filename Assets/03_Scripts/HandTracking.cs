using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour{

    [Header("Public Variables")]
    public Camera sceneCam;
    public OVRHand leftHand, rightHand;
    public OVRSkeleton skeleton;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float step;
    private bool isIndexFingerPinching;

    private LineRenderer line;
    //For further customization, mess with p1 and assign diff values relating to hand/cube
    private Transform p0, p1, p2;

    private Transform handIndexTipTransform;

    private void Start(){
        Init();
    }

    private void Update(){
        step = 5 * Time.deltaTime;
        if (leftHand.IsTracked){
            isIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            if (isIndexFingerPinching){
                line.enabled = true;
                PinchCube();
                GetLeftHandData();
                DrawCurve(p0.position, p1.position, p2.position);
            }
            else line.enabled = false;
        }
    }

    #region private methods

    private void Init(){
        transform.position = sceneCam.transform.position + sceneCam.transform.forward * 1f;
        line = GetComponent<LineRenderer>();
    }

    void GetLeftHandData(){
        foreach (var a in skeleton.Bones){
            if(a.Id == OVRSkeleton.BoneId.Hand_IndexTip){
                handIndexTipTransform = a.Transform;
                break;
            }
        }

        p0 = transform;
        p2 = handIndexTipTransform;
        p1 = sceneCam.transform;
        p1.position += sceneCam.transform.forward * .5f;
    }

    void PinchCube(){
        //Gets target rotation and position (left hand)
        targetPosition = leftHand.transform.position - leftHand.transform.position * .4f;
        targetRotation = Quaternion.LookRotation(transform.position - leftHand.transform.position);

        //Lerps to new position/rotation
        transform.position = Vector3.Lerp(transform.position, targetPosition, step);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    }


    //Quadratic Bzier Curve for LineRenderer ( B(t) = (1-t)^2 * p0 + 2 * (1-t) * t * p1 + t^2 * p2 )
    void DrawCurve(Vector3 point_0, Vector3 point_1, Vector3 point_2){
        line.positionCount = 200;
        Vector3 B = new Vector3(0,0,0);
        float t = 0f;

        for(int LinePositionIndex = 0; LinePositionIndex < line.positionCount; LinePositionIndex++){
            t += 0.005f;
            B = ( (1 - t) * (1 - t) ) * point_0 + 2 * (1 - t) * t * point_1 + (t * t) * point_2;
            line.SetPosition(LinePositionIndex, B);
        }
    }

    #endregion


}
