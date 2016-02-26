using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class K2MBody : MonoBehaviour {

    public GameObject jointPrefab;
    public GameObject bonePrefab;

    [Range(.01f, 10f)]
    public float jointScale = 1;

    [Range(.01f,10f)]
    public float boneScale = 1;
    [Range(.01f, 10f)]
    public float boneWidth = 1;

    public bool boneIsCentered;

    [HideInInspector]
    public Transform[] joints;
    [HideInInspector]
    public Transform[] bones;

    [HideInInspector]
    public int numJoints = 25;
   
    public enum JointType : int
    {
        SpineBase = 0,
        SpineMid = 1,
        Neck = 2,
        Head = 3,
        ShoulderLeft = 4,
        ElbowLeft = 5,
        WristLeft = 6,
        HandLeft = 7,
        ShoulderRight = 8,
        ElbowRight = 9,
        WristRight = 10,
        HandRight = 11,
        HipLeft = 12,
        KneeLeft = 13,
        AnkleLeft = 14,
        FootLeft = 15,
        HipRight = 16,
        KneeRight = 17,
        AnkleRight = 18,
        FootRight = 19,
        SpineShoulder = 20,
        HandTipLeft = 21,
        ThumbLeft = 22,
        HandTipRight = 23,
        ThumbRight = 24,
    }

    [HideInInspector]
    public int numBones = 20;

    private Dictionary<JointType, JointType> boneMap = new Dictionary<JointType, JointType>()
    {
        { JointType.FootLeft, JointType.AnkleLeft },
        { JointType.AnkleLeft, JointType.KneeLeft },
        { JointType.KneeLeft, JointType.HipLeft },
        { JointType.HipLeft, JointType.SpineBase },

        { JointType.FootRight, JointType.AnkleRight },
        { JointType.AnkleRight, JointType.KneeRight },
        { JointType.KneeRight, JointType.HipRight },
        { JointType.HipRight, JointType.SpineBase },

        
        //{ JointType.HandTipLeft, JointType.HandLeft },
        //{ JointType.ThumbLeft, JointType.HandLeft },
        { JointType.HandLeft, JointType.WristLeft },
        { JointType.WristLeft, JointType.ElbowLeft },
        { JointType.ElbowLeft, JointType.ShoulderLeft },
        { JointType.ShoulderLeft, JointType.SpineShoulder },
        

        //{ JointType.HandTipRight, JointType.HandRight },
        //{ JointType.ThumbRight, JointType.HandRight },
        { JointType.HandRight, JointType.WristRight },
        { JointType.WristRight, JointType.ElbowRight },
        { JointType.ElbowRight, JointType.ShoulderRight },
        { JointType.ShoulderRight, JointType.SpineShoulder },

        { JointType.Head ,JointType.Neck },
        { JointType.Neck , JointType.SpineShoulder },
        { JointType.SpineShoulder , JointType.SpineMid },
        { JointType.SpineMid, JointType.SpineBase }
       

    };

    // Use this for initialization
    void Awake () {
        joints = new Transform[25];
        bones = new Transform[24];

        for(int i=0;i< numJoints;i++)
        {
            joints[i] = GameObject.Instantiate(jointPrefab).transform;
            joints[i].parent = transform;
        }

        for(int i=0;i< numBones;i++)
        {
            bones[i] = GameObject.Instantiate(bonePrefab).transform;
            bones[i].parent = transform;
        }
	}

    void Start()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        for (int i = 0; i < numJoints; i++)
        {
            joints[i].rotation = Quaternion.identity; //force
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateJoint(int index,Vector3 pos)
    {
        for (int i = 0; i < numJoints; i++)
        {
            
            Transform jt = joints[index];
            jt.localPosition = pos;
            jt.localScale = new Vector3(jointScale, jointScale, jointScale);
        }


        for (int i = 0; i < numJoints; i++)
        {

            JointType t1 = (JointType)(i + 1); //start after spineBase

            if (!boneMap.ContainsKey(t1)) continue;
            JointType t2 = boneMap[t1];

            Transform jt1 = joints[(int)t1];
            Transform jt2 = joints[(int)t2];

            Transform bone = bones[i];

            if (boneIsCentered)
            {
                bone.localPosition = Vector3.Lerp(jt1.localPosition, jt2.localPosition, .5f);
            }
            else
            {
                bone.localPosition = jt2.localPosition;
            }

            bone.LookAt(jt1);

            float dist = Vector3.Distance(jt1.position, jt2.position);
            bone.localScale = new Vector3(boneScale*boneWidth, boneScale*boneWidth, dist* boneScale);
        }
    }
}
