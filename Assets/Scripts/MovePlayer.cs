using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    private Rigidbody rb = null;

    [SerializeField]
    private float torque = 100000, maxAngularVelocity = 7;

    public string Device { get; set; }

    public Transform CameraRig { get; set; }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
    }

    private void Update() {
        Quaternion originalCameraRotation = CameraRig.rotation;
        CameraRig.eulerAngles = new Vector3(0, CameraRig.localEulerAngles.y, 0);//moves camera rig back before a new frame is rendered
        rb.AddTorque(
            CameraRig.TransformDirection(
                new Vector3(
                    -Input.GetAxisRaw(InputMapper.Map(Device + "A2")) * torque * Time.deltaTime,
                    0,
                    -Input.GetAxisRaw(InputMapper.Map(Device + "A1")) * torque * Time.deltaTime
                )
            ),
            ForceMode.Acceleration
        );
        CameraRig.rotation = originalCameraRotation;

        if (transform.position.y < -1000)
            transform.position = Vector3.up * 10;
    }

}