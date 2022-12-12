using System.Linq;
using Banking.Source;
using Eto.Drawing;
using Eto.Forms;

namespace Banking.UI
{
    public enum Page
    {
        Sorting,
        History
    }

    public class MainForm : Form
    {
        public MainForm(string[] args)
        {
            var isTest = args.Contains(Constants.TestArg);
            ToolBar = new ToolBar();
            Menu = new MenuBar();
            
            Sorter = new Sorter();
            SorterUi = new SorterUi(Sorter, RefreshUi);
            HistoryUi = new HistoryUi(Sorter, RefreshUi);
            CurrentPage = Page.Sorting;
            
            Title = "It's Budgeting Time";
            MinimumSize = new Size(1200, 600);

            RefreshUi();
            
            AttachOpenFileDialog();
            AttachSaveSummaryDialog(isTest);
            AttachHistoryMenuItem();
            AttachSortingMenuItem();
        }
        
        private Sorter Sorter { get; }
        private SorterUi SorterUi { get; }
        private HistoryUi HistoryUi { get; }
        private Page CurrentPage { get; set; }
        private string OpenedFilename;
        
        private void AttachOpenFileDialog()
        {
            var chooseFile = new Command {MenuText = "Open File"};
            chooseFile.Executed += (sender, e) => {
                var dialog = new OpenFileDialog
                {
                    MultiSelect = false,
                    Filters = { new FileFilter("csvs", ".csv") },   
                };
                dialog.ShowDialog(this);
                
                OpenedFilename = dialog.Filenames.FirstOrDefault();
                if (OpenedFilename != null)
                    Sorter.LoadData(OpenedFilename);
                RefreshUi();
            };
            
            Menu.Items.Add(chooseFile);
        }

        private void AttachSaveSummaryDialog(bool isTest)
        {
            var saveSummary = new SaveSummaryCommand(Sorter, isTest);
            Menu.Items.Add(saveSummary);
        }

        private void AttachHistoryMenuItem()
        {
            var navToHistoryPage = new Command {MenuText = "History"};
            navToHistoryPage.Executed += (sender, e) =>
            {
                CurrentPage = Page.History;
                RefreshUi();
            };
            Menu.Items.Add(navToHistoryPage);
        }
        
        private void AttachSortingMenuItem()
        {
            var navToSortingPage = new Command {MenuText = "Sorting"};
            navToSortingPage.Executed += (sender, e) =>
            {
                CurrentPage = Page.Sorting;
                RefreshUi();
            };
            Menu.Items.Add(navToSortingPage);
        }

        private void RefreshUi()
        {
            if (CurrentPage == Page.Sorting)
            {
                ToolBar = SorterUi.ToolBar;
                Content = SorterUi.Layout();
            }
            else
            {
                ToolBar = HistoryUi.ToolBar;
                Content = HistoryUi.Layout();
            }
        }
    }
}