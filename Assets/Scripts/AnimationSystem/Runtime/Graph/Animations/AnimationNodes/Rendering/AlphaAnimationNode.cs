namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Rendering
{
    using System;
    using CreationTools;
    using CreationTools.ParameterTypes;
    using GraphProcessor;
    using Logic.Animation;
    using Logic.Animation.AnimationTypes.Rendering;
    using Logic.Animation.Interfaces;
    using UnityEngine;

    [Serializable, NodeMenuItem("Animation/Rendering/Alpha Animation")]
    public class AlphaAnimationNode : AnimationNode
    {
        #region Inspector Data

        public ChangeAlphaAnimation ChangeAlphaAnimation = new();

        [Input(name = "Target Alpha"), ShowAsDrawer]
        public float targetAlpha;

        #endregion

        public override IAnimable Animable => ChangeAlphaAnimation;

        public override string name => "Alpha Animation";

        protected override void Process()
        {
        }

        public override void SetParameters(ParametersContainer parametersContainer)
        {
            SetParameter(parametersContainer, "targetAlpha");
        }

        public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
        {
            SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
            var data = new SequenceTransitionData(ChangeAlphaAnimation.GetTween(), sequenceAddType);
            return GetSequenceDataFromPorts(data);
        }

        public override Type GetNeededType()
        {
            return ChangeAlphaAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
            var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
            return edge.outputNode as ParameterNode;
        }

        public override void SetAnimableObject(GameObject gameObject)
        {
            ChangeAlphaAnimation.SetAnimableObject(gameObject);
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
                ChangeAlphaAnimation.SetTargetAlpha(parametersContainer.GetFloatParameterData(param.parameter.name));
            }
        }
    }
}