namespace TH_TKPM_PXD.Service
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
