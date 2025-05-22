namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.UIToolkit
{
	using System;
	using GraphProcessor;
	using Logic.Animation.AnimationTypes.UIToolkit;
	using Logic.Animation.Interfaces;

	[Serializable]
	public abstract class ChangeUIDocumentStyleNode : AnimationNode
	{
		public override IAnimable Animable => null;

		public override string name => "UI Style Change Animation";

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override bool AreAllNodesProperlyConfigured()
		{
			if(Animable is UIDocumentStyleChange styleChange)
			{
				return styleChange.IsParameterTypeMatchingValue();
			}

			return true;
		}
	}
}