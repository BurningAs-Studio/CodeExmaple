using UnityEngine;
using VInspector;

namespace FAS
{
	public enum BonesSkeletonType
	{
		Main,
		Firing
	}
	
    public class BonesHolder : MonoBehaviour
    {
        [field: SerializeField] public Transform Hips { get; private set; }
        [field: SerializeField] public Transform LeftUpLeg { get; private set; }
        [field: SerializeField] public Transform LeftLeg { get; private set; }
        [field: SerializeField] public Transform LeftFoot { get; private set; }
        [field: SerializeField] public Transform RightUpLeg { get; private set; }
        [field: SerializeField] public Transform RightLeg { get; private set; }
        [field: SerializeField] public Transform RightFoot { get; private set; }
        [field: SerializeField] public Transform Spine { get; private set; }
        [field: SerializeField] public Transform Spine1 { get; private set; }
        [field: SerializeField] public Transform Spine2 { get; private set; }
        [field: SerializeField] public Transform Neck { get; private set; }
        [field: SerializeField] public Transform Head { get; private set; }
        [field: SerializeField] public Transform LeftShoulder { get; private set; }
        [field: SerializeField] public Transform LeftArm { get; private set; }
        [field: SerializeField] public Transform LeftForeArm { get; private set; }
        [field: SerializeField] public Transform LeftHand { get; private set; }
        [field: SerializeField] public Transform RightShoulder { get; private set; }
        [field: SerializeField] public Transform RightArm { get; private set; }
        [field: SerializeField] public Transform RightForeArm { get; private set; }
        [field: SerializeField] public Transform RightHand { get; private set; }

#if UNITY_EDITOR
        [Button]
        private void TryFindBones()
        {
	        Hips = FindDeepChild(transform, "mixamorig:Hips");
	        LeftUpLeg = FindDeepChild(transform, "mixamorig:LeftUpLeg");
	        LeftLeg = FindDeepChild(transform, "mixamorig:LeftLeg");
	        LeftFoot = FindDeepChild(transform, "mixamorig:LeftFoot");
	        RightUpLeg = FindDeepChild(transform, "mixamorig:RightUpLeg");
	        RightLeg = FindDeepChild(transform, "mixamorig:RightLeg");
	        RightFoot = FindDeepChild(transform, "mixamorig:RightFoot");
	        Spine = FindDeepChild(transform, "mixamorig:Spine");
	        Spine1 = FindDeepChild(transform, "mixamorig:Spine1");
	        Spine2 = FindDeepChild(transform, "mixamorig:Spine2");
	        Neck = FindDeepChild(transform, "mixamorig:Neck");
	        Head = FindDeepChild(transform, "mixamorig:Head");
	        LeftShoulder = FindDeepChild(transform, "mixamorig:LeftShoulder");
	        LeftArm = FindDeepChild(transform, "mixamorig:LeftArm");
	        LeftForeArm = FindDeepChild(transform, "mixamorig:LeftForeArm");
	        LeftHand = FindDeepChild(transform, "mixamorig:LeftHand");
	        RightShoulder = FindDeepChild(transform, "mixamorig:RightShoulder");
	        RightArm = FindDeepChild(transform, "mixamorig:RightArm");
	        RightForeArm = FindDeepChild(transform, "mixamorig:RightForeArm");
	        RightHand = FindDeepChild(transform, "mixamorig:RightHand");
        }

        private Transform FindDeepChild(Transform parent, string name)
        {
	        foreach (Transform child in parent)
	        {
		        if (child.name == name)
			        return child;
        
		        Transform result = FindDeepChild(child, name);
		        if (result != null)
			        return result;
	        }
	        return null;
        }
#endif
    }
}