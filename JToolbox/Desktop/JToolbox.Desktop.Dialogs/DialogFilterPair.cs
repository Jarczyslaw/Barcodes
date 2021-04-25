namespace JToolbox.Desktop.Dialogs
{
    public class DialogFilterPair
    {
        public string DisplayName { get; set; }
        public string ExtensionsList { get; set; }

        public DialogFilterPair()
        {
        }

        public DialogFilterPair(string extension)
        {
            DisplayName = extension;
            ExtensionsList = extension;
        }
    }
}
