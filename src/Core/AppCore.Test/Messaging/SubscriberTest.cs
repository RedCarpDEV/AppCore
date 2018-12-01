using Moq;
using RCd.AppCore.Messaging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RCd.AppCore.Test.Messaging
{

    public class SubscriberTest
    {

        [Fact]
        public void Constructor_Throws_ArgumentNullException_If_MessageBusFactory_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Subscriber<string>(null));
        }

        [Fact]
        public void Subscriber_Has_Correct_MessageType()
        {
            var mockMessageChannel = new Mock<IMessageChannel>();

            var subscriber = new Subscriber<string>(mockMessageChannel.Object);

            Assert.Equal(typeof(string), subscriber.MessageType);
        }

        [Fact]
        public void Filter_Does_Not_Throw_Exception_If_MessageFilter_IsNull()
        {
            var mockMessageChannel = new Mock<IMessageChannel>();

            ISubscriber<string> subscriber = new Subscriber<string>(mockMessageChannel.Object);

            subscriber.Filter(null);
        }

        [Fact]
        public void Filter_Returns_True_If_MessageFilter_IsNull()
        {
            var mockMessageChannel = new Mock<IMessageChannel>();

            var subscriber = new Subscriber<string>(mockMessageChannel.Object);

            ((ISubscriber<string>)subscriber).Filter(null);

            Assert.True(subscriber.Filter(string.Empty));
        }

        [Fact]
        public void Filter_Calls_Registered_MessageFilter_Correctly()
        {
            const string message = nameof(message);
            var called = false;

            var mockMessageChannel = new Mock<IMessageChannel>();

            var subscriber = new Subscriber<string>(mockMessageChannel.Object);

            ((ISubscriber<string>)subscriber).Filter(m => called = m == message);

            subscriber.Filter(message);

            Assert.True(called);
        }

        [Fact]
        public void Run_Does_Not_Throw_Exception_If_MessageHandler_IsNull()
        {
            var mockMessageChannel = new Mock<IMessageChannel>();

            ISubscriber<string> subscriber = new Subscriber<string>(mockMessageChannel.Object);

            subscriber.Run(null);
        }

        [Fact]
        public async Task Run_Returns_Task_If_MessageHandler_IsNull()
        {
            var mockMessageChannel = new Mock<IMessageChannel>();

            var subscriber = new Subscriber<string>(mockMessageChannel.Object);

            ((ISubscriber<string>)subscriber).Run(null);

            await subscriber.RunAsync(string.Empty);
        }

        [Fact]
        public async Task Run_Calls_Registered_MessageHandler_Correctly()
        {
            const string expectedMessage = nameof(expectedMessage);
            var message = string.Empty;

            var mockMessageChannel = new Mock<IMessageChannel>();

            var subscriber = new Subscriber<string>(mockMessageChannel.Object);

            ((ISubscriber<string>)subscriber).Run(m =>
            {
                message = m;
                return Task.CompletedTask;
            });

            await subscriber.RunAsync(expectedMessage);

            Assert.Equal(expectedMessage, message);
        }

        [Fact]
        public void Dispose_Unregisters_From_MessageChannel()
        {
            var called = false;

            var mockMessageChannel = new Mock<IMessageChannel>();

            var subscriber = new Subscriber<string>(mockMessageChannel.Object);

            mockMessageChannel
                .Setup(m => m.Unsubscribe(subscriber))
                .Callback(() => called = true);

            subscriber.Dispose();

            Assert.True(called);
        }

    }

}