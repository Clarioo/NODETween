namespace AnimationSystem.Runtime.Logic.Animation
{
    using Interfaces;
    using System;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [Serializable][ExecuteAlways]
    public class AnimableObject
    {
        public string GraphParameterName;
        public GameObject ObjectToAnimate;
        #if ODIN_INSPECTOR
        [InlineEditor]
        #endif
        public ObjectAnimator ObjectAnimator;

        #if ODIN_INSPECTOR
        [Button("Generate Animator")]
        #endif
        public void GenerateAnimator()
        {
            if (ObjectToAnimate != null)
            {
                if(ObjectAnimator == null || ObjectAnimator.gameObject != ObjectToAnimate)
                {
                    if (!ObjectToAnimate.GetComponent<ObjectAnimator>())
                    {
                        ObjectAnimator = ObjectToAnimate.AddComponent<ObjectAnimator>();
                    }
                    else
                    {
                        ObjectAnimator = ObjectToAnimate.GetComponent<ObjectAnimator>();
                    }
                }
            }
        }

        #if ODIN_INSPECTOR
        [Button("Sync Objects")]
        #endif
        public void SetObjectToAnimables()
        {
            if(ObjectAnimator != null)
            {
                foreach (var animable in ObjectAnimator.Animables)
                {
                    CheckIsTypeMatching(animable);
                }
            }

        }

        public void CheckIsTypeMatching(IAnimable animable)
        {
            var animType = animable.GetAnimableType();
            AddComponentWithType(animType);
            animable.SetAnimableObject(ObjectToAnimate);
        }

        public void AddComponentWithType(Type animType)
        {
            var obj = ObjectToAnimate.GetComponent(animType);
            if (obj == null)
            {
                ObjectToAnimate.AddComponent(animType);
            }
        }
    }
}