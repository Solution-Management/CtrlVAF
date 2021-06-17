namespace CtrlVAF.Models
{
    public interface IDispatcher<TReturn> : IDispatcher_Common
    {
        /// <summary>
        /// Main dispatcher entry method. Searches for suitable classes and instantiates them to execute some logic.
        /// </summary>
        /// <returns>Object of type <see cref="TReturn"/>.
        /// In case no suitable types are found or no return is expected this will be <see cref="default"/>. </returns>
        TReturn Dispatch(params ICtrlVAFCommand[] commands);
    }

    public interface IDispatcher : IDispatcher_Common
    {
        /// <summary>
        /// Main dispatcher entry method. Searches for suitable classes and instantiates them to execute some logic.
        /// </summary>
        void Dispatch(params ICtrlVAFCommand[] commands);
    }
}