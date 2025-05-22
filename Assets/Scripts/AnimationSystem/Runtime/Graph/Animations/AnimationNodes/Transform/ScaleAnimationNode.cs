namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Transform
{
	using System;
	using CreationTools;
	using CreationTools.ParameterTypes;
	using GraphProcessor;
	using Logic.Animation;
	using Logic.Animation.AnimationTypes.Transform.Scale;
	using Logic.Animation.Interfaces;
	using UnityEngine;

	[Serializable, NodeMenuItem("Animation/Transform/Scale Animation")]
	public class ScaleAnimationNode : AnimationNode
	{
		#region Inspector Data
		public ChangeScaleAnimation ChangeScaleAnimation = new();
        
		[Input(name = "Target Rotation"), ShowAsDrawer]
		public Vector3 targetScale;
		#endregion
        
		public override IAnimable Animable => ChangeScaleAnimation;

        public override string name => "Scale Animation";

		protected override void Process()
		{
		}

		public override void SetParameters(ParametersContainer parametersContainer)
		{
			SetParameter(parametersContainer, "targetScale");
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
        {
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var data = new SequenceTransitionData(ChangeScaleAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data); 
		}

        public override Type GetNeededType()
        {
			return ChangeScaleAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			ChangeScaleAnimation.SetAnimableObject(gameObject);
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
		        ChangeScaleAnimation.SetTargetScale(parametersContainer.GetVector3Parameter(param.parameter.name));
	        }
        }
    }
}