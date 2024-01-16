namespace Wallet.Common.Mediatr.Command
{
    public interface IApplicationCommandSender
    {
        Task SendAsync(IApplicationCommand command);

        Task<TResult> SendAsync<TResult>(IApplicationCommand<TResult> command);
    }
}
