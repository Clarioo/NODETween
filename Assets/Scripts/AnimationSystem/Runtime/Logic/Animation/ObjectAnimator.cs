namespace AnimationSystem.Runtime.Logic.Animation
{
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;
    #if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    public class ObjectAnimator : 
        #if ODIN_INSPECTOR
        SerializedMonoBehaviour
#else
        MonoBehaviour
#endif
    {
        #if ODIN_INSPECTOR
        [InlineEditor]
#endif
        public List<IAnimable> Animables = new ();
    }
}