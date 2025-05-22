namespace AnimationSystem.Runtime.Graph.Animations
{
    using System;
    using AnimationSystem.Runtime.Graph.Animations.CreationTools;
    using DG.Tweening;
    using GraphProcessor;
    using UnityEngine;

    public class AnimationProcessor : BaseGraphProcessor
    {
        public AnimationProcessor(BaseGraph graph, ParametersContainer parametersContainer) : base(graph) 
        {
            this.parametersContainer = parametersContainer;
        }

        private ParametersContainer parametersContainer;
        private Sequence animationTween;
        public Sequence AnimationTween => animationTween;

        public void SetParameters()
        {
            var animationNodes = graph.nodes.FindAll(n => n.GetType().IsSubclassOf(typeof(AnimationNode)));
            foreach (var node in animationNodes)
            {
                var animationNode = node as AnimationNode;
                animationNode.SetParameters(parametersContainer);
            }
        }

        public Sequence RunAnimation(Action onComplete)
        {
            animationTween.Kill();
            PreProcess();
            var firstNode = graph.nodes.Find(n => n.GetType() == typeof(AnimationStartScript)) as AnimationStartScript;
            firstNode.OnProcess();
            animationTween = firstNode.GetSequence().SetAutoKill(true);
            animationTween.onComplete = () =>
            {
                onComplete?.Invoke();
                animationTween.Complete();
            };
            return animationTween;
        }
        
        public void PrepareToEvaluate()
        {
            PreProcess();
            var firstNode = graph.nodes.Find(n => n.GetType() == typeof(AnimationStartScript)) as AnimationStartScript;
            firstNode.OnProcess();
            animationTween = firstNode.GetSequence();
            animationTween.Rewind();
        }

        public void DisableEvaluate()
        {
            animationTween.Rewind();
            animationTween.Kill();
            animationTween = null;
        }
        
        public void Evaluate(float value)
        {
            if (animationTween == null)
            {
                PrepareToEvaluate();
                return;
            }
            if (animationTween.Duration() == 0)
            {
                Debug.Log("Evaluation tween duration is 0");
                return;
            }
            animationTween.Goto(value/animationTween.Duration());
        }
        
        public void GetEvaluationValue(out float value)
        {
            value = animationTween.Elapsed()/animationTween.Duration();
        }
        
        public void PreProcess()
        {
            foreach (var node in graph.nodes)
            {
                if (node.GetType() != typeof(AnimationStartScript))
                {
                    node.OnProcess();
                }
            }
        }

        public override void UpdateComputeOrder()
        {
        }

        public override void Run()
        {
        }
    }
}