using System.Threading.Tasks;

namespace MyLab.Wpf
{
    public class VmInitializationContext
    {
        public IViewManager ViewManager { get; }
        public IViewProvider ViewProvider { get; }
        public TaskScheduler UiScheduler { get; }
        public TaskScheduler BizScheduler { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="VmInitializationContext"/>
        /// </summary>
        public VmInitializationContext(
            IViewManager viewManager, 
            IViewProvider viewProvider,
            TaskScheduler uiScheduler,
            TaskScheduler bizScheduler)
        {
            ViewManager = viewManager;
            ViewProvider = viewProvider;
            UiScheduler = uiScheduler;
            BizScheduler = bizScheduler;
        }
    }
}