namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.TMP
{
    using CreationTools;
    using GraphProcessor;
    using Logic.Animation;
    using Logic.Animation.AnimationTypes.TMP;
    using Logic.Animation.Interfaces;
    using System;
    using UnityEngine;

    [Serializable, NodeMenuItem("Animation/TMP/Font Spacing Animation")]
    public class FontSpacingAnimationNode : AnimationNode
    {
        
    #region Inspector Data
        public ChangeFontSpacingAnimation ChangeFontSpacingAnimation = new();

        [Input(name = "Target Font Spacing"), ShowAsDrawer]
        public float targetSpacing;
		    
#endregion

        public override IAnimable Animable => ChangeFontSpacingAnimation;

        public override string name => "Font Spacing Animation";

        protected override void Process()
        {
        }

        public override void SetParameters(ParametersContainer parametersContainer)
        {
            SetParameter(parametersContainer, "targetSpacing");
        }

        public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
        {
            SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
            var data = new SequenceTransitionData(ChangeFontSpacingAnimation.GetTween(), sequenceAddType);
            return GetSequenceDataFromPorts(data);
        }

        public override Type GetNeededType()
        {
            return ChangeFontSpacingAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
            var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
            return edge.outputNode as ParameterNode;
        }

        public override void SetAnimableObject(GameObject gameObject)
        {
            ChangeFontSpacingAnimation.SetAnimableObject(gameObject);
        }

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
        }

        protected override void SetParameter(ParametersContainer parametersContainer, string portName)
        {
            var inputPort = inputPorts.Find(p => p.fieldName == portName);
            var rotPort = inputPort.GetEdges();
            if (rotPort.Count > 0)
            {
                var param = rotPort[0].outputNode as ParameterNode;
                ChangeFontSpacingAnimation.SetTargetValue(parametersContainer.GetFloatParameterData(param.parameter.name));
            }
        }
    }
}