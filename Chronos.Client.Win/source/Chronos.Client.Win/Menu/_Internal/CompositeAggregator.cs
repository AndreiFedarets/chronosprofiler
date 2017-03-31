using System.Collections.Generic;

namespace Chronos.Client.Win.Menu
{
    internal static class CompositeAggregator
    {
        public static bool? GetIsEnabled(IEnumerable<IControl> controls)
        {
            bool? result = null;
            foreach (IControl control in controls)
            {
                bool? isEnabled = control.IsEnabled;
                if (isEnabled.HasValue)
                {
                    result = isEnabled;
                    if (isEnabled.Value)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public static bool? GetIsVisible(IEnumerable<IControl> controls)
        {
            bool? result = null;
            foreach (IControl control in controls)
            {
                bool? isVisible = control.IsVisible;
                if (isVisible.HasValue)
                {
                    result = isVisible;
                    if (isVisible.Value)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public static void MergeUnsafe(ICompositeControlCollection compositeControl, IControlCollection control)
        {
            foreach (IControl controlChild in control)
            {
                ICompositeControl compositeChild = (ICompositeControl)compositeControl.FindChild(controlChild.Id);
                if (compositeChild == null)
                {
                    compositeChild = CompositeControlFactory.Create(controlChild);
                    compositeControl.Add(compositeChild);
                }
                else if (controlChild is IControlStub)
                {
                    MergeStubUnsafe((ICompositeControlCollection)compositeChild, (IControlStub)controlChild);
                }
                else
                {
                    compositeChild.MergeWith(controlChild);
                }
            }
        }

        private static void MergeStubUnsafe(ICompositeControlCollection compositeControl, IControlStub control)
        {
            foreach (IControl controlChild in control)
            {
                ICompositeControl compositeChild = (ICompositeControl)compositeControl.FindChild(controlChild.Id);
                if (compositeChild == null)
                {
                    compositeChild = CompositeControlFactory.Create(controlChild);
                    compositeControl.Add(compositeChild);
                }
                else
                {
                    compositeChild.MergeWith(controlChild);
                }
            }
        }
    }
}
