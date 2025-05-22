namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.UIToolkit
{
    using CreationTools;
    using CreationTools.ParameterTypes;
    using GraphProcessor;
    using Logic.Animation.AnimationTypes.UIToolkit;
    using Logic.Animation.Interfaces;
    using System;

    [Serializable, NodeMenuItem("Animation/UI Toolkit/UI Document Style Float Animation")]
    public class ChangeUIDocumentStyleFloatNode : ChangeUIDocumentStyleNode
    {
        #region Inspector Data
        public UIDocumentStyleFloatChange UIDocumentStyleFloatChange = new();
        #endregion

        [Input(name = "Input")]
        public float targetValue;

        public override IAnimable Animable => UIDocumentStyleFloatChange;

        public override string name => "UI Style Float Change Animation";

        public override void SetParameters(ParametersContainer parametersContainer)
        {
            SetParameter(parametersContainer, "targetValue");
        }

        protected override void SetParameter(ParametersContainer parametersContainer, string portName)
        {
            var inputPort = inputPorts.Find(p => p.fieldName == portName);
            var rotPort = inputPort.GetEdges();
            if (rotPort.Count > 0)
            {
                var param = rotPort[0].outputNode as ParameterNode;
                UIDocumentStyleFloatChange.SetTargetValue(
                    parametersContainer.GetFloatParameterData(param.parameter.name));
            }
        }
    }
}