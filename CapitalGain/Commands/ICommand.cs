namespace CapitalGain.Commands
{
    public interface ICommand<TInput, TOutput>
    {
        public TOutput Execute(TInput input);
    }
}
