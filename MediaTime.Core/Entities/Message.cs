namespace MediaTime.Core.Entities
{
    /// <summary>
    ///    Defines the Message type.
    /// </summary>
    public class Message : Cirrious.MvvmCross.Plugins.Messenger.MvxMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public Message(object sender)
            : base(sender)
        {
        }
    }
}
