using UnityEngine;
using UnityEngine.UI;



namespace Player.UI
{
    public class LifeStealUI : MonoBehaviour
    {
        [SerializeField] private Image _lifeStealFlash;



        private void Start()
        {
            _lifeStealFlash.enabled = false;
        }


        private void Update()
        {
            if (DataManager.Instance.IsLifeStealActive == true)
            {
                _lifeStealFlash.enabled = true;
            }
            else 
            {
                _lifeStealFlash.enabled = false;
            }
        }
    }
}