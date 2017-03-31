using System;
using System.Collections.Generic;

namespace Chronos.Core
{
	public class DisposableScope : IDisposable
	{
		private readonly IList<IDisposable> _disposables;

		public DisposableScope()
		{
			_disposables = new List<IDisposable>();
		}

		public void Add(IDisposable disposable)
		{
			_disposables.Add(disposable);
		}

		public void Dispose()
		{
			foreach (IDisposable disposable in _disposables)
			{
				disposable.Dispose();
			}
			_disposables.Clear();
		}
	}
}
