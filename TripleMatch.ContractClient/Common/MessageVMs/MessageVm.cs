using CommunityToolkit.Mvvm.ComponentModel;

namespace TripleMatch.ContractClient.Common.MessageVMs
{
    public partial class MessageVm
        : ObservableObject
    {
        [ObservableProperty]
        private string _message = string.Empty;
    }
}
