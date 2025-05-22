namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Transform
{
    using System;
    using CreationTools;
    using CreationTools.ParameterTypes;
    using GraphProcessor;
    using Logic.Animation;
    using Logic.Animation.AnimationTypes.Transform.Layout;
    using Logic.Animation.Interfaces;
    using UnityEngine;

    [Serializable, NodeMenuItem("Animation/Transform/Layout Preferred Size Animation")]
    public class LayoutPreferredSizeAnimationNode : AnimationNode
    {
        #region Inspector Data
        public LayoutPreferredSizeAnimation LayoutPreferredSizeAnimation = new();

        [Input(name = "Target Preferred Size")]
        public Vector2 targetPreferredSize;
        #endregion

        public override IAnimable Animable => LayoutPreferredSizeAnimation;

        public override string name => "Layout Preferred Size Animation";

        protected override void Process()
        {
        }

        public override void SetParameters(ParametersContainer parametersContainer)
        {
            SetParameter(parametersContainer, "targetPreferredSize");
        }

        public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
        {
            SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
            var data = new SequenceTransitionData(LayoutPreferredSizeAnimation.GetTween(), sequenceAddType);
            return GetSequenceDataFromPorts(data);
        }

        public override Type GetNeededType()
        {
            return LayoutPreferredSizeAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
            var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
            return edge.outputNode as ParameterNode;
        }

        public override void SetAnimableObject(GameObject gameObject)
        {
            LayoutPreferredSizeAnimation.SetAnimableObject(gameObject);
        }

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
            throw new NotImplementedException();
        }

        private void SetParameter(ParametersContainer parametersContainer, string portName)
        {
            var inputPort = inputPorts.Find(p => p.fieldName == portName);
            var rotPort = inputPort.GetEdges();
            if (rotPort.Count > 0)
            {
                var param = rotPort[0].outputNode as ParameterNode;
                LayoutPreferredSizeAnimation.SetTargetLayoutSize(
                    parametersContainer.GetVector2Parameter(param.parameter.name));
            }
        }
    }
}