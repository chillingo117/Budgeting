using Eto.Forms;

namespace Banking.UI
{
    public class AddBucketDialog: Dialog<string>
    {
        private const int StackWidth = 200;
        public AddBucketDialog()
        {
            var textbox = new TextBox{Width = StackWidth};
            var layout = new StackLayout {Width = StackWidth};
            
            DefaultButton = new Button {Text = "Create", Width = StackWidth};
            AbortButton = new Button {Text = "Cancel", Width = StackWidth};
            
            DefaultButton.Click += delegate { Result = textbox.Text; Close();};
            AbortButton.Click += delegate { Result = null; Close(); };
            
            layout.Items.Add(textbox);
            layout.Items.Add(DefaultButton);
            layout.Items.Add(AbortButton);

            Content = layout;
        }
    }
}