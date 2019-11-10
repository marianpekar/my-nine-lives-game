using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace IndiePixel.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class IP_UI_Screen : MonoBehaviour
    {
        #region Variables
        [Header("Main Properties")]
        public Selectable m_StartSelectable;

        [Header("Screen Events")]
        public UnityEvent onScreenStart = new UnityEvent();
        public UnityEvent onScreenClose = new UnityEvent();

        private Animator animator;
        #endregion

        #region Main Methods
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();

            if(m_StartSelectable)
            {
                EventSystem.current.SetSelectedGameObject(m_StartSelectable.gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Helper Methos
        public virtual void StartScreen() 
        {
            if(onScreenStart != null)
            {
                onScreenStart.Invoke();
            }

            HandleAnimator("show");
        }

        public virtual void CloseScreen()
        {
            if (onScreenClose != null)
            {
                onScreenClose.Invoke();
            }

            HandleAnimator("hide");
        }

        void HandleAnimator(string aTrigger)
        {
            if (animator)
            {
                animator.SetTrigger(aTrigger);
            }
        }
        #endregion
    }
}
