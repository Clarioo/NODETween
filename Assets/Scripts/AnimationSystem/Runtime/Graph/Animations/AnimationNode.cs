namespace AnimationSystem.Runtime.Graph.Animations
{
	using System;
	using System.Collections.Generic;
	using CreationTools;
	using CreationTools.ParameterTypes;
	using GraphProcessor;
	using Logic.Animation;
	using Logic.Animation.Interfaces;
	using UnityEngine;

	[Serializable]
	public abstract class AnimationNode : BaseNode
	{
		[HideInInspector]
		public int NodeId;

		#region InOut
		[Input(name = "Input")]
		public int animationInput;
		[Input(name = "AnimableGO")]
		public GameObject animableGo;

		[Output(name = "Append")]
		public int outputAppend;
		[Output(name = "Join")]
		public int outputJoin;
		#endregion
		
		public abstract IAnimable Animable { get; }
			

		public override string name => "AnimationNode";

		public override void OnNodeCreated()
		{
			base.OnNodeCreated();
			if(Animable != null)
				Animable.AssignedNodeGUID = GUID;
		}

		/// <summary>
		/// Method responsible for setting parameters for the node.
		/// It is called by the AnimationProcessor class before the animation is run.
		/// </summary>
		/// <param name="parametersContainer"></param>
		public virtual void SetParameters(ParametersContainer parametersContainer)
        {

        }

		/// <summary>
		/// Method to get the sequence data from the node.
		/// Sets Animable object, build the sequence and returns nested data from connected nodes. 
		/// </summary>
		/// <param name="sequenceAddType"></param>
		/// <returns></returns>
		public virtual SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var data = new SequenceTransitionData(Animable.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		/// <summary>
		/// Uses recursion to get the sequence data from connected nodes.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public virtual SequenceTransitionData GetSequenceDataFromPorts(SequenceTransitionData data)
        {
			var appendEdges = outputPorts.Find(p => p.fieldName == "outputAppend").GetEdges();
			if (appendEdges.Count > 0)
			{
				foreach (var edge in appendEdges)
				{
					var node = edge.inputNode as AnimationNode;
					data.AppendedSequences.Add(node.GetSequenceData(SequenceAddType.Append));
				}
			}
			var joinedEdges = outputPorts.Find(p => p.fieldName == "outputJoin").GetEdges();
			if (joinedEdges.Count > 0)
			{
				foreach (var edge in joinedEdges)
				{
					var node = edge.inputNode as AnimationNode;
					data.JoinedSequences.Add(node.GetSequenceData(SequenceAddType.Join));
				}
			}
			return data;
		}

		/// <summary>
		/// Override this to specify which parameter should be assigned to the node.
		/// </summary>
		/// <returns></returns>
		public abstract ParameterNode GetAssignedParameter();
		

		/// <summary>
		/// Override to add Animable object to animation class. eg. Transform for RotationNode, CanvasGroup for AlphaNode
		/// </summary>
		/// <param name="gameObject"></param>
		public virtual void SetAnimableObject(GameObject gameObject)
		{
			Animable.SetAnimableObject(gameObject);
		}
		
		/// <summary>
		/// Method to pass optional GameObjects to the node. For example in rotation node you can pass lookAt object.
		/// </summary>
		/// <param name="optionalGOs"></param>
		public virtual void SetOptionalGOs(GameObject[] optionalGOs){}

		
		/// <summary>
		/// Returns the type of the component that the node needs to work properly. For example AlphaNode needs CanvasGroup.
		/// </summary>
		/// <returns></returns>
		public virtual Type GetNeededType()
		{
			return Animable.GetAnimableType();
		}

		/// <summary>
		/// Custom method to specify configuration of nodes in the graph.
		/// </summary>
		/// <returns></returns>
		public virtual bool AreAllNodesProperlyConfigured()
		{
			return true;
		}
		
		/// <summary>
		/// Base method for set parameters from container based on port name.
		/// </summary>
		/// <param name="parametersContainer"></param>
		/// <param name="portName"></param>
		protected virtual void SetParameter(ParametersContainer parametersContainer, string portName){}

		#region TypeSpecificParameterGetters
		// Need to decide if we want to keep these methods or move those to the ParameterContainer class.
		
		
#endregion

	}
}