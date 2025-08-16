using UnityEngine;

namespace StarterAssets
{
    public class CameraHandler
    {
        private readonly ThirdPersonController _controller;

        private float _yaw;
        private float _pitch;
        public float sensitivity = 200f;

        private const float _threshold = 0.01f;

        public CameraHandler(ThirdPersonController controller)
        {
            _controller = controller;
            _yaw = _controller.CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        }

        public void RotateCamera()
        {
            if (_controller.Input.look.sqrMagnitude >= _threshold)
            {
                float deltaTime = Time.deltaTime;
                _yaw += _controller.Input.look.x * deltaTime * sensitivity;
                _pitch += _controller.Input.look.y * deltaTime * sensitivity;
            }

            _pitch = Mathf.Clamp(_pitch, _controller.BottomClamp, _controller.TopClamp);

            _controller.CinemachineCameraTarget.transform.rotation =
                Quaternion.Euler(_pitch + _controller.CameraAngleOverride, _yaw, 0f);
        }
    }
}