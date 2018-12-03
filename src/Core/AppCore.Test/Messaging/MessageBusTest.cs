using Moq;
using RCd.AppCore.Messaging;
using System;
using System.Collections.Generic;
using Xunit;

namespace RCd.AppCore.Test.Messaging
{

    public class MessageBusTest
    {

        [Fact]
        public void Constructor_Throws_ArgumentNullException_If_MessageBusFactory_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MessageBus(null));
        }

        [Fact]
        public void RegisterChannel_Throws_ArgumentNullException_If_ChannelId_Is_NullOrEmpty()
        {
            var mockMessageChannel = new Mock<IMessageChannel>();

            IMessageChannel MessageBusFactory(string channelId) => mockMessageChannel.Object;

            var messageBus = new MessageBus(MessageBusFactory);

            Assert.Throws<ArgumentNullException>(() => messageBus.RegisterChannel(null));
            Assert.Throws<ArgumentNullException>(() => messageBus.RegisterChannel(string.Empty));
        }
        
        [Fact]
        public void RegisterChannel_Throws_InvalidOperationException_If_ChannelId_Is_Registered_Already()
        {
            const string registeredChannelId = nameof(registeredChannelId);

            var mockMessageChannel = new Mock<IMessageChannel>();

            IMessageChannel MessageBusFactory(string channelId) => mockMessageChannel.Object;

            var messageBus = new MessageBus(MessageBusFactory);

            messageBus.RegisterChannel(registeredChannelId);

            Assert.Throws<InvalidOperationException>(() => messageBus.RegisterChannel(registeredChannelId));
        }

        [Fact]
        public void IsChannelRegistered_Returns_True_If_Channel_IsRegistered_And_False_If_Not()
        {
            const string registeredChannelId = nameof(registeredChannelId);

            var mockMessageChannel = new Mock<IMessageChannel>();

            IMessageChannel MessageBusFactory(string channelId) => mockMessageChannel.Object;

            var messageBus = new MessageBus(MessageBusFactory);

            Assert.False(messageBus.IsChannelRegistered(registeredChannelId));

            messageBus.RegisterChannel(registeredChannelId);

            Assert.True(messageBus.IsChannelRegistered(registeredChannelId));

            Assert.False(messageBus.IsChannelRegistered(null));
            Assert.False(messageBus.IsChannelRegistered(string.Empty));
        }

        [Fact]
        public void GetChannel_Throws_ArgumentNullException_If_Channel_Is_Null_Or_Empty()
        {
            var mockMessageChannel = new Mock<IMessageChannel>();

            IMessageChannel MessageBusFactory(string channelId) => mockMessageChannel.Object;

            var messageBus = new MessageBus(MessageBusFactory);

            Assert.Throws<ArgumentNullException>(() => messageBus.GetChannel(null));
            Assert.Throws<ArgumentNullException>(() => messageBus.GetChannel(string.Empty));
        }

        [Fact]
        public void GetChannel_Throws_ArgumentNullException_If_ChannelId_Is_Not_Registered()
        {
            const string notRegisteredChannelId = nameof(notRegisteredChannelId);

            var mockMessageChannel = new Mock<IMessageChannel>();

            IMessageChannel MessageBusFactory(string channelId) => mockMessageChannel.Object;

            var messageBus = new MessageBus(MessageBusFactory);

            Assert.Throws<KeyNotFoundException>(() => messageBus.GetChannel(notRegisteredChannelId));
        }

        [Fact]
        public void GetChannel_Returns_Registerd_Channel()
        {
            const string registeredChannelId = nameof(registeredChannelId);

            var mockMessageChannel = new Mock<IMessageChannel>();

            IMessageChannel MessageBusFactory(string channelId) => mockMessageChannel.Object;

            var messageBus = new MessageBus(MessageBusFactory);

            messageBus.RegisterChannel(registeredChannelId);

            Assert.Equal(mockMessageChannel.Object, messageBus.GetChannel(registeredChannelId));
        }

        [Fact]
        public void Dispose_Disposes_AllRegistered_Channels()
        {
            var dispose1Called = false;
            var dispose2Called = false;
            const string registeredChannelId1 = nameof(registeredChannelId1);
            const string registeredChannelId2 = nameof(registeredChannelId2);

            var mockMessageChannel1 = new Mock<IMessageChannel>();
            mockMessageChannel1.Setup(x => x.Dispose()).Callback(() => dispose1Called = true);

            var mockMessageChannel2 = new Mock<IMessageChannel>();
            mockMessageChannel2.Setup(x => x.Dispose()).Callback(() => dispose2Called = true);

            IMessageChannel CreateChannel(string channelId)
            {
                if (channelId == registeredChannelId1)
                {
                    return mockMessageChannel1.Object;
                }
                if (channelId == registeredChannelId2)
                {
                    return mockMessageChannel2.Object;
                }
                return null;
            }

            var messageBus = new MessageBus(CreateChannel);

            messageBus.RegisterChannel(registeredChannelId1);
            messageBus.RegisterChannel(registeredChannelId2);

            messageBus.Dispose();

            Assert.True(dispose1Called);
            Assert.True(dispose2Called);
        }

    }

}