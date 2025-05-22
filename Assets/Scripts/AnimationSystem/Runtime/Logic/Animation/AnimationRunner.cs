namespace AnimationSystem.Runtime.Logic.Animation
{
    using DG.Tweening;
    using Graph.Animations;
    using Graph.Animations.CreationTools;
    using System;
    using System.Threading.Tasks;
    using UnityEngine;
#if UNITY_EDITOR
    using DG.DOTweenEditor;
#endif
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    /// Class responsible for creating and playing animations using listed subanimations
    /// </summary>
    [ExecuteAlways]
    public class AnimationRunner : 
        #if ODIN_INSPECTOR
        SerializedMonoBehaviour
        #else
        MonoBehaviour
        #endif    
    {
        #region Odin Inspector
        private const string GRAPH = "Graph";
        private const string ANIMATION = "Animation";

        #endregion

        #region Inspector
        
        #if ODIN_INSPECTOR
        [BoxGroup(GRAPH)]
        #endif
        [SerializeField]
        protected AnimationGraphCreator animationGraphCreator = new AnimationGraphCreator();
  #endregion

        #region Temp Variables
        #if ODIN_INSPECTOR
        [SerializeField, BoxGroup(ANIMATION), Range(0,1f),
         OnValueChanged("EvaluatePreview"), ShowIf("@IsPreviewing")]
        #else
        [SerializeField, HideInInspector]
    #endif
        private float animationProgress;
        private Sequence sequence;
        private AnimationProcessor graphProcessor;
        private bool isPreviewing;
  #endregion

        #region Properties
        public ParametersContainer ParametersContainer => animationGraphCreator.ParametersContainer;
        
        public bool IsPreviewing => isPreviewing;
        public bool IsPreviewPlaying => sequence != null && sequence.IsPlaying();
  #endregion

        #region Events
        public event Action OnAnimationTick;
        public event Action<float> OnAnimationTickWithProgress;
  #endregion

        private async void Awake()
        {
            // Ensure that preview is disabled on start to avoid any issues
            #if UNITY_EDITOR
            await DisablePreview();
            #endif
        }

        public void ReloadParameters()
        {
            graphProcessor ??= new AnimationProcessor(animationGraphCreator.SampleGraph, ParametersContainer);
            graphProcessor.SetParameters();
            animationGraphCreator.FillParameters();
        }

        public virtual async Task Play(Action onFinish = null, bool playCached = true)
        {
            if (graphProcessor == null | !playCached)
            {
                ReloadParameters();
            }
            sequence = graphProcessor.RunAnimation(onFinish);
            sequence.Play();
            sequence.onUpdate -= OnAnimationUpdate;
            sequence.onUpdate += OnAnimationUpdate;
            await sequence.AsyncWaitForCompletion();
        }

        public virtual async Task PlayInstantly()
        {
            ReloadParameters();
            sequence = graphProcessor.RunAnimation(null);
            sequence.Play();
            sequence.Complete();
        }
        
        #if UNITY_EDITOR
        
        /// <summary>
        /// Creates tween from graph and allows to preview it in editor. If tween is already playing it will stop it and start again
        /// </summary>
        #if ODIN_INSPECTOR
        [BoxGroup(ANIMATION), Button("Start Preview")]
        #endif
        public virtual async Task EnablePreview()
        {
            DOTweenEditorPreview.Stop(true);
            animationProgress = 0;
            ReloadParameters();
            sequence = graphProcessor.RunAnimation(null);
            DOTweenEditorPreview.PrepareTweenForPreview(sequence);
            sequence.onUpdate += UpdatePreviewProgress;
            DOTweenEditorPreview.Start();
            isPreviewing = true;
        }
        
        /// <summary>
        /// Pauses or resumes tween preview if it is playing
        /// </summary>
        #if ODIN_INSPECTOR
        [BoxGroup(ANIMATION), Button("Play/Pause Preview")]
        #endif
        public virtual async Task PlayPausePreview()
        {
            if (!isPreviewing) return;
            if (sequence.IsPlaying())
            {
                sequence.Pause();
            }
            else
            {
                sequence.Play();
            }
        }
        
        /// <summary>
        /// Disables tween preview mode and resets animables to initial state
        /// </summary>
        #if ODIN_INSPECTOR
        [BoxGroup(ANIMATION), Button("Stop Preview")]
        #endif
        public virtual async Task DisablePreview()
        {
            DOTweenEditorPreview.Stop(true);
            sequence.Kill();
            isPreviewing = false;
            animationProgress = 0;
        }

        public void EvaluatePreview(float value)
        {
            if(!isPreviewing) return;
            sequence.Goto(sequence.Duration() * value);
            animationProgress = value;
        }
        
        private void UpdatePreviewProgress()
        {
            if(!IsPreviewPlaying) return;
            graphProcessor.GetEvaluationValue(out animationProgress);
        }
        
        
        #endif

        /// <summary>
        /// Prepares animation to be evaluated manually or disables evaluation
        /// </summary>
        /// <param name="enable"></param>
        public void EnableEvaluate(bool enable)
        {
            if (enable)
            {
                ReloadParameters();
                graphProcessor.PrepareToEvaluate();
            }
            else
                graphProcessor?.DisableEvaluate();
        }
        
        /// <summary>
        /// Evaluates animation at given time. If withCallbackProgress is true it will change animationProgress value
        /// </summary>
        /// <param name="time"></param>
        /// <param name="withCallbackProgress"></param>
        public void Evaluate(float time)
        {
            if (graphProcessor == null) return;
            sequence.Pause();
            graphProcessor.Evaluate(time);
            graphProcessor.GetEvaluationValue(out animationProgress);
        }
        
        /// <summary>
        /// Completes current sequence
        /// </summary>
        public void Complete()
        {
            sequence.Goto(sequence.Duration());
        }
        
        /// <summary>
        /// Sets animation graph to be played
        /// </summary>
        /// <param name="animationGraph"></param>
        public virtual void SetAnimation(AnimationGraph animationGraph)
        {
            animationGraphCreator.SampleGraph = animationGraph;
            graphProcessor = null;
        }
        
        /// <summary>
        /// Stops animation and kill sequence
        /// </summary>
        /// <param name="playToEnd">If true animation plays to end and then kill sequence</param>
        private void StopSequence(bool playToEnd = false)
        {
            if (sequence == null || !sequence.IsPlaying()) return;
            sequence.Kill(playToEnd);
        }
        
        /// <summary>
        /// Returns current animation progress and Invoke OnAnimationTick event
        /// </summary>
        private void OnAnimationUpdate()
        {
            animationProgress = sequence.Elapsed() / sequence.Duration();
            OnAnimationTickWithProgress?.Invoke(animationProgress);
            OnAnimationTick?.Invoke();
        }
    }
}