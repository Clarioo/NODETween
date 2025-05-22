namespace AnimationSystem.Mapper
{
    #if VIEW_MANAGER_ANIMATION_SUPPORT
    using Core.ViewManager.Interfaces;
#endif
    using Runtime.Logic.Animation;
    using System.Threading.Tasks;

    public class NodeAnimationRunner : AnimationRunner
        #if VIEW_MANAGER_ANIMATION_SUPPORT
        , IAnimationRunner
  #endif
        
    {
        public void Setup()
        {
        }

        public async Task Play()
        {
            await base.Play(null, false);
        }
        public async void PlayInstantly()
        {
            await base.PlayInstantly();
        }

        public void Stop()
        {
            Complete();
        }
        
        public void SetParameter<T>(string parameterName, T value)
        {
            ParametersContainer.SetParameterValue(parameterName, value);
        }
    }
}
