namespace AnimationSystem.Editor.Windows.Views
{
    using GraphProcessor;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor.Experimental.GraphView;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class PartedParametersView<T> : ExposedParameterView
    {
        public virtual string parameterTittle { get; }

        protected override void UpdateParameterList()
        {
            content.Clear();

            foreach (var param in graphView.graph.exposedParameters)
            {
                if (param.GetType() == typeof(T))
                {
                    var row = new BlackboardRow(new ExposedParameterFieldView(graphView, param), new ExposedParameterPropertyView(graphView, param));
                    row.expanded = param.settings.expanded;
                    row.RegisterCallback<GeometryChangedEvent>(e => {
                        param.settings.expanded = row.expanded;
                    });

                    content.Add(row);
                }
            }
        }

        protected override void Initialize(BaseGraphView graphView)
        {
            base.Initialize(graphView);
            base.title = parameterTittle;
        }

        protected override IEnumerable<Type> GetExposedParameterTypes()
        {
            yield return typeof(T);
        }
    }
}