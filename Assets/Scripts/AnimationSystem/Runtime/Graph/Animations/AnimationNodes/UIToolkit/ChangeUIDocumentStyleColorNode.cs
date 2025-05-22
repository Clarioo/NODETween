namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.UIToolkit
{
    using System;
    using CreationTools;
    using CreationTools.ParameterTypes;
    using GraphProcessor;
    using Logic.Animation.AnimationTypes.UIToolkit;
    using Logic.Animation.Interfaces;
    using UnityEngine;

    [Serializable, NodeMenuItem("Animation/UI Toolkit/UI Document Style Color Animation")]
    public class ChangeUIDocumentStyleColorNode : ChangeUIDocumentStyleNode
    {
        #region Inspector Data

        public UIDocumentStyleColorChange UIDocumentStyleColorChange = new();

        #endregion

        [Input(name = "Input")]
        public Color targetValue;

        public override IAnimable Animable => UIDocumentStyleColorChange;
        
        public override string name => "UI Style Color Change Animation";

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
                UIDocumentStyleColorChange.SetTargetValue(parametersContainer.GetColorParameterData(param.parameter.name));
            }
        }
    }
}
