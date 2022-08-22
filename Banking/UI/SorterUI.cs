using System;
using Banking.Source;
using Eto.Forms;

namespace Banking.UI
{
    public class SorterUi
    {
        public SorterUi(Sorter sorter, Action refreshUi)
        {
            Sorter = sorter;
            _refreshUi = refreshUi;
        }

        private Sorter Sorter { get; }
        private readonly Action _refreshUi;

        private void AddCurrentTransactionToBucket(string name)
        {
            Sorter.AddCurrentTransactionToBucket(name);
            _refreshUi();
        }

        private void RemoveBucket(string name)
        {
            Sorter.RemoveBucket(name);
            _refreshUi();
        }

        public StackLayout Layout()
        {
            var layout = new StackLayout{Width = 1200, Spacing = 10};

            var count = 0;
            var subLayoutRow = new StackLayout{Width = 1200, Orientation = Orientation.Horizontal, Spacing = 5};

            foreach (var bucket in Sorter.Buckets)
            {
                subLayoutRow.Items.Add(new BucketUi(bucket, AddCurrentTransactionToBucket, RemoveBucket).Layout);
                if (count++ > 10)
                {
                    count = 0;
                    layout.Items.Add(subLayoutRow);
                    subLayoutRow = new StackLayout{Width = 1200, Orientation = Orientation.Horizontal, Spacing = 5};
                }
            }
            layout.Items.Add(subLayoutRow);
            return layout;
        }
    }
}