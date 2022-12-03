using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class CameraManager : Singleton<CameraManager>
    {
        private Camera _camera;

        public float FarClipPlane { get { return _camera.farClipPlane; } }

        RaycastHit[] hits = new RaycastHit[1];

        LayerMask layerMask;

        public static float DEFAILTCAMERASIZE = 11.5f;

        public override void Awake()
        {
            base.Awake();

            _camera = GetComponent<Camera>();

            layerMask = 1 << LevelManager.GROUNDLAYER;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 rayCastStartWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = transform.localRotation * Vector3.forward;

                Debug.DrawRay(rayCastStartWorldPosition, direction * FarClipPlane, Color.red, 20.0f);

                if (Physics.RaycastNonAlloc(rayCastStartWorldPosition, direction, hits, FarClipPlane, layerMask) > 0)
                    LevelManager.Instance.SetObject(hits[0].point);
            }
        }

        public void UpdateCamera(float ratio)
        {
            _camera.orthographicSize = ratio * DEFAILTCAMERASIZE;
        }
    }
}