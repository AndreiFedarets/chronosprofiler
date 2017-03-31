using System;

namespace Chronos.Communication.NamedPipe
{
	public class OperationException : Exception
	{
		public OperationException(ResultCode result)
			: this(result, GetMessageForCode(result))
		{
		}

		public OperationException(ResultCode result, string messsage)
			: base(messsage ?? string.Empty)
		{
			Result = result;
		}

		public ResultCode Result { get; private set; }

		private static string GetMessageForCode(ResultCode resultCode)
		{
			switch (resultCode)
			{
				case ResultCode.SessionPoolNotAvailable:
					return "Session pool is not available";
				case ResultCode.InvalidActivationSettings:
					return "Invalid activation settings";
				case ResultCode.Ok:
					return "Ok";
				case ResultCode.ConfigurationNotFound:
					return "Configuration was not found";
				case ResultCode.SessionNotFound:
					return "Session was not found";
				case ResultCode.UnknownError:
					return "Unknown error";
			}
			return string.Empty;
		}
	}
}
