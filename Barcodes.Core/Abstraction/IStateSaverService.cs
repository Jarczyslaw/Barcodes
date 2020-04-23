namespace Barcodes.Core.Abstraction
{
    public interface IStateSaverService
    {
        void Save<TViewModel, TState>(TViewModel viewModel);

        void Load<TViewModel, TState>(TViewModel viewModel);
    }
}