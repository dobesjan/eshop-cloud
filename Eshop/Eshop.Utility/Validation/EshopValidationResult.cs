using System.Text.Json;

namespace Eshop.Utility.Validation
{
    public enum EshopValidationStatus
    {
        OK = 0,
        FAIL = 1
    }

    public class EshopValidationResult
    {
        public EshopValidationStatus Status { get; private set; }
        public List<EshopValidationMessage> Messages { get; }

        public EshopValidationResult()
        {
            Status = EshopValidationStatus.OK;
            Messages = new List<EshopValidationMessage>();
        }

        public EshopValidationResult(EshopValidationStatus status, List<EshopValidationMessage> messages)
        {
            Status = status;
            Messages = messages;
        }

        public void AddMessage(string message)
        {
            Messages.Add(new EshopValidationMessage(message));
        }

		public void AddWarningMessage(string message)
		{
			Messages.Add(new EshopValidationMessage(message, EshopValidationMessageStatus.WARNING));
		}

		public void AddErrorMessage(string message)
        {
            Messages.Add(new EshopValidationMessage(message, EshopValidationMessageStatus.ERROR));
            SetFailStatus();
        }

        public void SetFailStatus()
        {
            Status = EshopValidationStatus.FAIL;
        }

        public void SetOkStatus()
        {
            Status = EshopValidationStatus.OK;
        }

        public void MergeValidationResult(EshopValidationResult result)
        {
            if (result.Status == EshopValidationStatus.FAIL)
            {
                Status = result.Status;
            }

            Messages.AddRange(result.Messages);
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
