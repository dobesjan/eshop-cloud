using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Utility.Validation
{
	public enum EshopValidationMessageStatus
	{
		INFO = 0,
		WARNING = 1,
		ERROR = 2
	}

	public class EshopValidationMessage
	{
		public EshopValidationMessageStatus Status;
		public string Message { get; }

		public EshopValidationMessage(string message)
		{
			Status = EshopValidationMessageStatus.INFO;
			Message = message;
		}

		public EshopValidationMessage(string message, EshopValidationMessageStatus status)
		{
			Status = status;
			Message = message;
		}
	}
}
