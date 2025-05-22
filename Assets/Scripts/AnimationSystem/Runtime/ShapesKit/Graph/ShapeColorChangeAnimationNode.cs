namespace AnimationSystem.Runtime.ShapesKit.Graph
{
    using GraphProcessor;
    using Logic;
    using Runtime.Graph.Animations;
    using Runtime.Graph.Animations.CreationTools;
    using Runtime.Logic.Animation;
    using Runtime.Logic.Animation.Interfaces;
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;

    [Serializable, NodeMenuItem("Animation/Rendering/Change Shape Color Animation")]
    public class ShapeColorChangeAnimationNode : AnimationNode
    {
#region Inspector Data

        [FormerlySerializedAs("ChangeColorAnimation")]
        public ChangeBaseShapeColor ChangeShapeColorAnimation = new();

        [Input(name = "Target Color"), ShowAsDrawer]
        public Color targetColor;

        #endregion

        public override IAnimable Animable => ChangeShapeColorAnimation;

        public override string name => "Change Shape Color Animation";

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
            var data = new SequenceTransitionData(ChangeShapeColorAnimation.GetTween(), sequenceAddType);
            return GetSequenceDataFromPorts(data);
        }

        public override Type GetNeededType()
        {
            return ChangeShapeColorAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
            var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
            return edge.outputNode as ParameterNode;
        }

        public override void SetAnimableObject(GameObject gameObject)
        {
            ChangeShapeColorAnimation.SetAnimableObject(gameObject);
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
                ChangeShapeColorAnimation.SetTargetColor(
                    parametersContainer.GetColorParameterData(param.parameter.name));
            }
        }
    }
}
