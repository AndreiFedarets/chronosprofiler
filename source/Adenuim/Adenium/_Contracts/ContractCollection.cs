using System;
using System.Collections.Generic;

namespace Adenium
{
    public class ContractCollection
    {
        private readonly List<IContract> _contracts;

        public ContractCollection(object contractOwner)
        {
            _contracts = new List<IContract>();
            RegisterOwnerContracts(contractOwner);
        }

        public void RegisterContract(IContract contract)
        {
            _contracts.Add(contract);
        }

        public void RegisterItem(object item)
        {
            foreach (IContract contract in _contracts)
            {
                contract.Register(item);
            }
        }

        public void UnregisterItem(object item)
        {
            foreach (IContract contract in _contracts)
            {
                contract.Unregister(item);
            }
        }

        public void RegisterOwnerContracts(object contractOwner)
        {
            if (contractOwner == null)
            {
                return;
            }
            Type contractOwnerType = contractOwner.GetType();
            IEnumerable<EnableContractAttribute> attributes = EnableContractAttribute.GetContractAttributes(contractOwnerType);
            foreach (EnableContractAttribute attribute in attributes)
            {
                Type contractType = attribute.ContractType;
                object contractObject = Activator.CreateInstance(contractType);
                IContract contract = (IContract)contractObject;
                RegisterContract(contract);
            }
            RegisterItem(contractOwner);
        }
    }
}
