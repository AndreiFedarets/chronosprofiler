using System.Collections.Generic;
using Chronos.Core;
using System.Windows.Input;
using Rhiannon.Windows.Presentation.Commands;
using Rhiannon.Threading;

namespace Chronos.Client.Win.Views.Units.Exceptions
{
	public class ViewModel : ViewModel<ExceptionInfo>, IViewModel
	{
		private readonly IProcessShadow _processShadow;
	    private readonly IProcessShadowNavigator _processShadowNavigator;
	    private readonly ITaskFactory _taskFactory;
		private IList<string> _stack;

        public ViewModel(IProcessShadow processShadow, IProcessShadowNavigator processShadowNavigator, ITaskFactory taskFactory)
			: base(processShadow.Exceptions, processShadow)
		{
			_processShadow = processShadow;
            _taskFactory = taskFactory;
            _processShadowNavigator = processShadowNavigator;
			processShadow.Exceptions.UnitsUpdated += OnUnitsUpdated;
		}

        public ICommand GoToExceptionInCallTreeCommand { get; private set; }

		public IList<string> Stack
		{
			get { return _stack; }
			private set { SetPropertyAndNotifyChanged(() => Stack, ref _stack, value); }
		}

		public override void Dispose()
		{
			base.Dispose();
			_processShadow.Exceptions.UnitsUpdated -= OnUnitsUpdated;
		}

		protected override void OnSelectionChanged(UnitEventArgs e)
		{
			base.OnSelectionChanged(e);
			ExceptionInfo exceptionInfo = (ExceptionInfo)e.Unit;
			IList<string> stack = new List<string>();
			if (exceptionInfo != null)
			{
				foreach (uint functionId in exceptionInfo.Stack)
				{
					FunctionInfo functionInfo = _processShadow.Functions[functionId];
					ClassInfo classInfo = _processShadow.Classes[functionInfo.ClassManangedId, functionInfo.BeginLifetime];
					string className;
					if (classInfo == null)
					{
						className = "<UNKNOWN>";
					}
					else
					{
						className = classInfo.Name;
					}
					string entry = string.Format("{0}.{1}", className, functionInfo.Name);
					stack.Add(entry);
				}
			}
			Stack = stack;
		}

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            GoToExceptionInCallTreeCommand = new AsyncCommand<ExceptionInfo>(GoToExceptionInCallTree, _taskFactory);
        }

        private void GoToExceptionInCallTree(ExceptionInfo exceptionInfo)
        {
            _processShadowNavigator.NavigateFirst(EventType.ExceptionThrown, exceptionInfo.Id);
        }
	}
}
