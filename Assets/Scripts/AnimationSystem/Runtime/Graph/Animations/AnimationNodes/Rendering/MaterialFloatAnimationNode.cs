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

	[Serializable, NodeMenuItem("Animation/Rendering/Material Float Animation")]
	public class MaterialFloatAnimationNode : AnimationNode
	{
		#region Inspector Data
		public ChangeMaterialFloatAnimation ChangeMaterialFloatAnimation = new();
		
		[Input(name = "Target Material Float"), ShowAsDrawer]
		public float targetMaterialFloat;
		#endregion

		public override IAnimable Animable => ChangeMaterialFloatAnimation;

		public override string name => "Material Float Animation";

		protected override void Process()
		{
		}

		public override void SetParameters(ParametersContainer parametersContainer)
		{
			SetParameter(parametersContainer, "targetMaterialFloat");
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var data = new SequenceTransitionData(ChangeMaterialFloatAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return ChangeMaterialFloatAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			ChangeMaterialFloatAnimation.SetAnimableObject(gameObject);
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
				ChangeMaterialFloatAnimation.SetTargetValue(parametersContainer.GetFloatParameterData(param.parameter.name));
			}
		}	
	}
}