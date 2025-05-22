namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Rendering
{
    using System;
    using AnimationSystem.Runtime.Graph.Animations.CreationTools;
    using AnimationSystem.Runtime.Logic.Animation;
    using AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Rendering;
    using AnimationSystem.Runtime.Logic.Animation.Interfaces;
    using CreationTools.ParameterTypes;
    using GraphProcessor;
    using UnityEngine;

    [Serializable, NodeMenuItem("Animation/Rendering/Change Color Animation")]
    public class ColorChangeAnimationNode : AnimationNode
    {
        #region Inspector Data

        public ChangeColorAnimation ChangeColorAnimation = new();

        [Input(name = "Target Color"), ShowAsDrawer]
        public Color targetColor;

        #endregion

        public override IAnimable Animable => ChangeColorAnimation;

        public override string name => "Change Color Animation";

        protected override void Process()
        {
        }

        public override void SetParameters(ParametersContainer parametersContainer)
        {
            SetParameter(parametersContainer, "targetColor");
        }

        public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
        {
            SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
            var data = new SequenceTransitionData(ChangeColorAnimation.GetTween(), sequenceAddType);
            return GetSequenceDataFromPorts(data);
        }

        public override Type GetNeededType()
        {
            return ChangeColorAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
            var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
            return edge.outputNode as ParameterNode;
        }

        public override void SetAnimableObject(GameObject gameObject)
        {
            ChangeColorAnimation.SetAnimableObject(gameObject);
        }

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
        }

        private void SetParameter(ParametersContainer parametersContainer, string portName)
        {
            var inputPort = inputPorts.Find(p => p.fieldName == portName);
            var rotPort = inputPort.GetEdges();
            if (rotPort.Count > 0)
            {
                var param = rotPort[0].outputNode as ParameterNode;
                ChangeColorAnimation.SetTargetColor(
                    parametersContainer.GetColorParameterData(param.parameter.name));
            }
        }
    }
}