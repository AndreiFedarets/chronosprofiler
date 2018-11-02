using System;
using Adenium.Layouting;
using Caliburn.Micro;

namespace Adenium
{
    public abstract class ViewModel : Screen, IViewModel, IHaveLayout, IHaveScope
    {
        private readonly ViewModelContext _context;

        protected ViewModel()
        {
            _context = new ViewModelContext(this);
        }

        public virtual string ViewModelUid
        {
            get { return _context.ViewModelUid; }
        }

        public virtual Guid InstanceId
        {
            get { return _context.InstanceId; }
        }

        public new IContainerViewModel Parent
        {
            get { return base.Parent as IContainerViewModel; }
        }

        public IContainerViewModel LogicalParent
        {
            get { return _context.LogicalParent; }
        }

        public IMenuCollection Menus
        {
            get { return _context.Menus; }
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }

        ViewModelLayout IHaveLayout.Layout
        {
            get { return _context.Layout; }
        }

        void IHaveLayout.AssignLayout(ViewModelLayout layout)
        {
            _context.AssignLayout(layout);
        }

        IContainer IHaveScope.ScopeContainer
        {
            get { return _context.ScopeContainer; }
        }

        void IHaveScope.AssignScopeContainer(IContainer container)
        {
            ConfigureScopeContainer(container);
            _context.AssignScopeContainer(container);
        }

        protected virtual void ConfigureScopeContainer(IContainer container)
        {

        }
    }
}
