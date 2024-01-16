namespace Wallet.Common.Codes
{
    public class ErrorMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
        /// Ctor for serialization, Do not delete!.
        /// </summary>
        internal ErrorMessage()
        {
            this.Code = string.Empty;
            this.Message = string.Empty;
        }

        //Only use test ErrorMessage data scenario 
        protected internal ErrorMessage(string errorCode, string message)
        {
            this.Code = errorCode;
            this.Message = message;
        }
        
        internal ErrorMessage(ErrorCode errorCode, string message)
        {
            this.ErrorCode = errorCode;
            this.Code = errorCode.ToCode();
            this.Message = message;
        }

        public ErrorCode ErrorCode { get; }

        public string Code { get; }

        public string Message { get; }

        public override string ToString()
        {
            return this.Code + ": " + this.Message;
        }
    }
}
