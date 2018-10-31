﻿using System;
using Adenium.Layouting;
using Caliburn.Micro;

namespace Adenium
{
    public abstract class ViewModel : Screen, IViewModel
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

        public IMenuCollection Menus
        {
            get { return _context.Menus; }
        }

        public virtual void Dispose()
        {
            _context.Dispose();
        }
    }
}