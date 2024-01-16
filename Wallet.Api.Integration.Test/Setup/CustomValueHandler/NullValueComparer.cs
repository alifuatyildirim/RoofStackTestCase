using TechTalk.SpecFlow.Assist;

namespace Wallet.Api.Integration.Test.Setup.CustomValueHandler
{
    public class NullValueComparer : IValueComparer
    {
        private readonly string nullText;

        public NullValueComparer(string nullText)
        {
            this.nullText = nullText;
        }

        public bool CanCompare(object actualValue)
        {
            return actualValue == null;
        }

        public bool Compare(string expectedValue, object actualValue)
        {
            return expectedValue == this.nullText && actualValue == null;
        }
    }
}
