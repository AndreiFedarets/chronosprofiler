using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Client.Win.Contracts
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EnableContractAttribute : Attribute
    {
        public EnableContractAttribute(Type contractType)
        {
            ContractType = contractType;
        }

        public Type ContractType { get; private set; }

        internal static IEnumerable<EnableContractAttribute> GetContractAttributes(Type type)
        {
            object[] attributeObjects = type.GetCustomAttributes(typeof(EnableContractAttribute), true);
            return attributeObjects.Select(x => (EnableContractAttribute) x);
        }
    }
}
