using System.Collections.Generic;

namespace Adenium.Menu
{
    internal static class CompositeAggregator
    {
        public static bool? GetIsEnabled(IEnumerable<IMenuControl> controls)
        {
            bool? result = null;
            foreach (IMenuControl control in controls)
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

        public static bool? GetIsVisible(IEnumerable<IMenuControl> controls)
        {
            bool? result = null;
            foreach (IMenuControl control in controls)
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

        public static void MergeUnsafe(ICompositeControlCollection compositeControl, IMenuControlCollection control)
        {
            foreach (IMenuControl controlChild in control)
            {
                ICompositeControl compositeChild = (ICompositeControl)compositeControl.FindChild(controlChild.Id);
                if (compositeChild == null)
                {
                    compositeChild = CompositeControlFactory.Create(controlChild);
                    compositeControl.Add(compositeChild);
                }
                else if (controlChild is MenuControlStub)
                {
                    MergeStubUnsafe((ICompositeControlCollection)compositeChild, (MenuControlStub)controlChild);
                }
                else
                {
                    compositeChild.MergeWith(controlChild);
                }
            }
        }

        private static void MergeStubUnsafe(ICompositeControlCollection compositeControl, MenuControlStub control)
        {
            foreach (IMenuControl controlChild in control)
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
