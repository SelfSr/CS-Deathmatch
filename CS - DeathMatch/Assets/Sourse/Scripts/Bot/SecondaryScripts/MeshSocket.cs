using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSockets.SocketId socketId;
    public HumanBodyBones bone;
    private Transform attachPoint;


    public Vector3 offset;
    public Vector3 rotation;
    private void Start()
    {
        Animator animator = GetComponentInParent<Animator>();
        attachPoint = new GameObject("socket" + socketId).transform;
        attachPoint.SetParent(animator.GetBoneTransform(bone));
        attachPoint.localPosition = offset;
        attachPoint.localRotation = Quaternion.Euler(rotation);
    }

    public void Attach(Transform objectTransform)
    {
        objectTransform.SetParent(attachPoint, false);
    }
}