using Moq;
using RCd.AppCore.Messaging;
using System.Threading.Tasks;
using Xunit;

namespace RCd.AppCore.Test.Messaging
{

    public class MessageChannelTest
    {

        [Fact]
        public void Unsubscribe_Returns_False_If_Subscriber_Is_Null()
        {
            var messageChannel = new MessageChannel();

            Assert.False(messageChannel.Unsubscribe(null));
        }

        [Fact]
        public void Unsubscribe_Returns_False_If_Subscribers_MessageType_IsNull()
        {
            var mockSubscriber = new Mock<ISubscriber>();

            var messageChannel = new MessageChannel();

            Assert.False(messageChannel.Unsubscribe(mockSubscriber.Object));
        }

        [Fact]
        public void Unsubscribe_Returns_False_If_Subscriber_Is_Not_Subscribed()
        {
            var mockSubscriber = new Mock<ISubscriber>();
            mockSubscriber.SetupGet(x => x.MessageType).Returns(typeof(string));

            var messageChannel = new MessageChannel();

            Assert.False(messageChannel.Unsubscribe(mockSubscriber.Object));
        }

        [Fact]
        public void Unsubscribe_Returns_True_If_Subscriber_Is_Subscribed()
        {
            var messageChannel = new MessageChannel();

            var subscriber = messageChannel.Subscribe<string>();

            Assert.True(messageChannel.Unsubscribe(subscriber));
        }

        [Fact]
        public void IsSubscribed_Returns_False_If_Subscriber_Is_Null()
        {
            var messageChannel = new MessageChannel();

            Assert.False(messageChannel.IsSubscribed(null));
        }

        [Fact]
        public void IsSubscribed_Returns_False_If_Subscribers_MessageType_IsNull()
        {
            var mockSubscriber = new Mock<ISubscriber>();

            var messageChannel = new MessageChannel();

            Assert.False(messageChannel.IsSubscribed(mockSubscriber.Object));
        }

        [Fact]
        public void IsSubscribed_Returns_False_If_Subscriber_IsNot_Subscribed()
        {
            var mockSubscriber = new Mock<ISubscriber>();
            mockSubscriber.SetupGet(x => x.MessageType).Returns(typeof(string));

            var messageChannel = new MessageChannel();

            Assert.False(messageChannel.IsSubscribed(mockSubscriber.Object));
        }

        [Fact]
        public void IsSubscribed_Returns_True_If_Subscriber_Is_Subscribed()
        {
            var messageChannel = new MessageChannel();

            var subscriber = messageChannel.Subscribe<string>();

            Assert.True(messageChannel.IsSubscribed(subscriber));
        }

        [Fact]
        public async Task PublishAsync_Runs_All_Interested_Subscribers()
        {
            const string message = nameof(message);
            var subscriber1Handled = false;
            var subscriber2Handled = false;
            var subscriber3Handled = false;
            var subscriber4Handled = false;

            var messageChannel = new MessageChannel();

            messageChannel.Subscribe<string>(m =>
            {
                subscriber1Handled = true;
                return Task.CompletedTask;
            });

            var subscriber2 = messageChannel.Subscribe<string>(m =>
            {
                subscriber2Handled = true;
                return Task.CompletedTask;
            });
            subscriber2.Filter(m => m == message);

            var subscriber3 = messageChannel.Subscribe<string>(m =>
            {
                subscriber3Handled = true;
                return Task.CompletedTask;
            });
            subscriber3.Filter(m => m != message);

            messageChannel.Subscribe<int>(m =>
            {
                subscriber4Handled = true;
                return Task.CompletedTask;
            });

            await messageChannel.PublishAsync(message).ConfigureAwait(false);

            Assert.True(subscriber1Handled);
            Assert.True(subscriber2Handled);
            Assert.False(subscriber3Handled);
            Assert.False(subscriber4Handled);
        }

        [Fact]
        public void Dispose_Unsubscribes_All_Registered_Subscribers()
        {
            var messageChannel = new MessageChannel();

            var subscriber1 = messageChannel.Subscribe<string>();
            var subscriber2 = messageChannel.Subscribe<int>();
            var subscriber3 = messageChannel.Subscribe<object>();
            var subscriber4 = messageChannel.Subscribe<object>();

            messageChannel.Dispose();

            Assert.False(messageChannel.IsSubscribed(subscriber1));
            Assert.False(messageChannel.IsSubscribed(subscriber2));
            Assert.False(messageChannel.IsSubscribed(subscriber3));
            Assert.False(messageChannel.IsSubscribed(subscriber4));
        }

    }

}