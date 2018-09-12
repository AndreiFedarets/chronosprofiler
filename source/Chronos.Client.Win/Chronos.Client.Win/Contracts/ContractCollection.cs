using System.Collections.Generic;

namespace Chronos.Client.Win.Contracts
{
    public class ContractCollection
    {
        private readonly List<IContract> _contracts;

        public ContractCollection()
        {
            _contracts = new List<IContract>();
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
    }
}
