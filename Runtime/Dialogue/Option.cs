namespace DialogueSystem
{
    /// <summary>
    /// The Option objects that compose a Decision.
    /// </summary>
    public class Option
    {
        /// <summary>
        /// The label for the option displayed.
        /// </summary>
        public string option = "";

        /// <summary>
        /// The link pointing to the connected dialogue object. <c>null</c> indicates the end of the conversation.
        /// </summary>
        public NodeLinkData link;
    }
}