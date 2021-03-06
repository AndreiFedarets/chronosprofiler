﻿using System;
using System.Linq;

namespace Adenium
{
    public sealed class ViewModelAttribute : Attribute
    {
        public ViewModelAttribute(string viewModelUid)
            : this(viewModelUid, string.Empty)
        {
        }

        public ViewModelAttribute(string viewModelUid, string viewType)
        {
            ViewModelUid = viewModelUid;
            ViewType = viewType;
        }

        public string ViewModelUid { get; private set; }

        public string ViewType { get; private set; }

        public static ViewModelAttribute GetAttribute(IViewModel viewModel)
        {
            Type viewModelType = viewModel.GetType();
            object[] attributes = viewModelType.GetCustomAttributes(typeof(ViewModelAttribute), true);
            ViewModelAttribute attribute = (ViewModelAttribute)attributes.FirstOrDefault();
            return attribute;
        }
    }
}
