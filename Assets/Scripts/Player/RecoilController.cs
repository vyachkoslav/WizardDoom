using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public struct Recoil
    {
        public Vector2 Target;
        public float Time;
    }
    public class RecoilController : MonoBehaviour
    {
        [SerializeField] private float accelerationFactor = 0.5f;
        [SerializeField] private CharacterControls characterControls;
        
        private List<Recoil> pendingRecoils = new();
        private List<Recoil> pendingResets = new();

        private void Awake()
        {
            pendingRecoils.Capacity = 50;
            pendingResets.Capacity = 50;
        }
        
        public void AddRecoil(Vector2 delta, float recoilSmoothTime)
        {
            AddRecoil(new Recoil(){ Target = delta, Time = recoilSmoothTime });
        }
        public void AddRecoil(Recoil recoil)
        {
            pendingRecoils.Add(recoil);
            recoil.Target = -recoil.Target;
            recoil.Time *= 5;
            pendingResets.Add(recoil);
        }

        private void Update()
        {
            for (int i = 0; i < pendingRecoils.Count; i++)
            {
                if(ApplyRecoil(pendingRecoils[i], accelerationFactor, out var recoil))
                    pendingRecoils.RemoveAt(i--);
                else
                    pendingRecoils[i] = recoil;
            }
            for (int i = 0; i < pendingResets.Count; i++)
            {
                if(ApplyRecoil(pendingResets[i], 1, out var recoil))
                    pendingResets.RemoveAt(i--);
                else
                    pendingResets[i] = recoil;
            }
        }

        private bool ApplyRecoil(Recoil recoil, float acceleration, out Recoil result)
        {
            var delta = recoil.Target * Mathf.Min(1, Time.deltaTime / (recoil.Time * acceleration));
            characterControls.AddCameraDelta(delta);
            recoil.Target -= delta;
            recoil.Time -= Time.deltaTime;
            result = recoil;
            return recoil.Time <= 0 || recoil.Target == Vector2.zero;
        }
    }
}