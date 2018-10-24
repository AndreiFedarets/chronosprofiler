using System;

namespace Adenium.Layouting
{
    internal sealed class ViewModelReference
    {
        public ViewModelReference(string typeName, ViewModelMode mode, ViewModelActivation activation)
        {
            TypeName = typeName;
            Model = mode;
            Activation = activation;
        }

        public Type Type
        {
            get { return Type.GetType(TypeName); }
        }

        public string TypeName { get; private set; }

        public ViewModelMode Model { get; private set; }

        public ViewModelActivation Activation { get; private set;  }
    }
}
