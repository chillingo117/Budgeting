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
            var layout = new StackLayout();
            layout.Orientation = Orientation.Horizontal;
            foreach (var bucket in Sorter.Buckets)
            {
                layout.Items.Add(new BucketUi(bucket, AddCurrentTransactionToBucket, RemoveBucket)
                    .Layout);
            }

            return layout;
        }
    }
}