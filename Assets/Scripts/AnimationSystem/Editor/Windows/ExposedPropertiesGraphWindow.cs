namespace AnimationSystem.Editor.Windows
{
	using UnityEngine;
	using UnityEditor;
	using GraphProcessor;
    using AnimationSystem.Editor.Windows.Views;
    using Assets.Scripts.AnimationSystem.Editor.Windows.Toolbar;

    public class ExposedPropertiesGraphWindow : BaseGraphWindow
	{
		BaseGraph tmpGraph;

		[MenuItem("Window/04 Exposed Properties")]
		public static BaseGraphWindow OpenWithTmpGraph()
		{
			var graphWindow = CreateWindow<ExposedPropertiesGraphWindow>();

			// When the graph is opened from the window, we don't save the graph to disk
			graphWindow.tmpGraph = ScriptableObject.CreateInstance<BaseGraph>();
			graphWindow.tmpGraph.hideFlags = HideFlags.HideAndDontSave;
			graphWindow.InitializeGraph(graphWindow.tmpGraph);

			graphWindow.Show();

			return graphWindow;
		}

		protected override void OnDestroy()
		{
			graphView?.Dispose();
			DestroyImmediate(tmpGraph);
		}

		protected override void InitializeWindow(BaseGraph graph)
		{
			titleContent = new GUIContent("Animation Graph");

			if (graphView == null)
            {
				graphView = new BaseGraphView(this);
				graphView.Add(new AnimationGraphToolbarView(graphView));
			}
			rootView.Add(graphView);
		}

		protected override void InitializeGraphView(BaseGraphView view)
		{
			view.OpenPinned<FloatParametersView>();
			view.OpenPinned<Vector3ParametersView>();
			view.OpenPinned<Vector2ParametersView>();
			view.OpenPinned<GameObjectParametersView>();
			view.OpenPinned<StringParametersView>();

			view.ToggleView<FloatParametersView>();
			view.ToggleView<Vector3ParametersView>();
			view.ToggleView<Vector2ParametersView>();
			view.ToggleView<GameObjectParametersView>();
			view.ToggleView<StringParametersView>();
		}
	}
}