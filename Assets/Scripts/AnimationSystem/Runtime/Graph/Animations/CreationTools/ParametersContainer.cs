namespace AnimationSystem.Runtime.Graph.Animations.CreationTools
{
    using GraphProcessor;
    using ParameterTypes;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class ParametersContainer
    {
        public List<Vector2ParameterData> Vector2ParameterDatas = new();
        public List<Vector3ParameterData> Vector3ParameterDatas = new();
        public List<FloatParameterData> FloatParameterDatas = new();
        public List<StringParameterData> StringParameterDatas = new();
        public List<ColorParameterData> ColorParameterDatas = new();

        public void UpdateParametersContainer(ParametersContainer parametersContainer)
        {
            foreach (var vector2ParameterData in parametersContainer.Vector2ParameterDatas)
            {
                SetParameterValue(vector2ParameterData.ParameterName, vector2ParameterData.ParameterValue);
            }
            foreach (var vector3ParameterData in parametersContainer.Vector3ParameterDatas)
            {
                SetParameterValue(vector3ParameterData.ParameterName, vector3ParameterData.ParameterValue);
            }
            foreach (var floatParameterData in parametersContainer.FloatParameterDatas)
            {
                SetParameterValue(floatParameterData.ParameterName, floatParameterData.ParameterValue);
            }
            foreach (var stringParameterData in parametersContainer.StringParameterDatas)
            {
                SetParameterValue(stringParameterData.ParameterName, stringParameterData.ParameterValue);
            }
            foreach (var colorParameterData in parametersContainer.ColorParameterDatas)
            {
                SetParameterValue(colorParameterData.ParameterName, colorParameterData.ParameterValue);
            }
        }
        
        public virtual void SetParameterValue<T>(string parameterName, T value)
        {
            switch (value)
            {
                case Vector2 vector2:
                    UpdateParameterValue<Vector2ParameterData, Vector2, Vector2Parameter>(parameterName, Vector2ParameterDatas, vector2);
                    break;
                case Vector3 vector3:
                    UpdateParameterValue<Vector3ParameterData, Vector3, Vector3Parameter>(parameterName, Vector3ParameterDatas, vector3);
                    break;
                case float floatVal:
                    UpdateParameterValue<FloatParameterData, float, FloatParameter>(parameterName, FloatParameterDatas, floatVal);
                    break;
                case string stringVal:
                    UpdateParameterValue<StringParameterData, string, StringParameter>(parameterName, StringParameterDatas, stringVal);
                    break;
                case Color colorVal:
                    UpdateParameterValue<ColorParameterData, Color, ColorParameter>(parameterName, ColorParameterDatas, colorVal);
                    break;
            }
        }
        
        private void UpdateParameterValue<T, TU, TW>(string parameterName, List<T> datas, TU value) where T : BaseParameterData<TU, TW>, new() where TW : ExposedParameter, new()
        {
            var match = datas.Find(d => d.ParameterName == parameterName);
            if (match != null)
            {
                match.ParameterValue = value;
            }
            else
            {
                var data = new T();
                data.ParameterName = parameterName;
                data.ParameterValue = value;
                datas.Add(data);
            }
        }

        public T GetParameterWithName<T, TU, TW>(List<T> parametersContainer, string parameterName) where T : BaseParameterData<TU, TW> where TW : ExposedParameter, new()
        {
            return parametersContainer.Find(p => p.ParameterName == parameterName);
        }

        #region Type Specific Getters
        
        public Vector2ParameterData GetVector2Parameter(string parameterName)
        {
            return Vector2ParameterDatas.Find(p => p.ParameterName == parameterName);
        }
        
        public Vector3ParameterData GetVector3Parameter(string parameterName)
        {
            return Vector3ParameterDatas.Find(p => p.ParameterName == parameterName);
        }
        
        public FloatParameterData GetFloatParameterData(string parameterName)
        {
            return FloatParameterDatas.Find(p => p.ParameterName == parameterName);
        }
        
        public StringParameterData GetStringParameterData(string parameterName)
        {
            return StringParameterDatas.Find(p => p.ParameterName == parameterName);
        }
        
        public ColorParameterData GetColorParameterData(string parameterName)
        {
            return ColorParameterDatas.Find(p => p.ParameterName == parameterName);
        }
        
  #endregion
        
    }
}