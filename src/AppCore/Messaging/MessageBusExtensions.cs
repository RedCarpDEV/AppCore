namespace RCd.AppCore.Messaging
{

    public static class MessageBusExtensions
    {

        #region Methods

        public static IMessageChannel DefaultChannel(this IMessageBus messageBus)
        {
            const string DEFAULT_CHANNEL = nameof(DEFAULT_CHANNEL);

            if (!messageBus.IsChannelRegistered(DEFAULT_CHANNEL))
            {
                messageBus.RegisterChannel(DEFAULT_CHANNEL);
            }

            return messageBus.GetChannel(DEFAULT_CHANNEL);
        }

        #endregion

    }

}