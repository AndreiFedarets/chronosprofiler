﻿namespace Chronos.Client.Win.Contracts.Dialog
{
    public interface IContractSource : Contracts.IContractSource
    {
        bool DialogReady { get; }
    }
}
