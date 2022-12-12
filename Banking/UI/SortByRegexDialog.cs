using Eto.Forms;

namespace Banking.UI
{
    public class SortByRegexDialog: Dialog<string>
    {
        private const int StackWidth = 200;

        public SortByRegexDialog()
        {
            var textBox = new TextBox();
            DefaultButton = new Button {Text = "Create", Width = StackWidth};
            AbortButton = new Button {Text = "Cancel", Width = StackWidth};

            DefaultButton.Click += delegate
            {
                Result = textBox.Text;
                Close();
            };
            AbortButton.Click += delegate
            {
                Result = null;
                Close();
            };

            var layout = new StackLayout();
            layout.Items.Add(textBox);
            layout.Items.Add(DefaultButton);
            layout.Items.Add(AbortButton);

            Content = layout;        
        }
    }
}