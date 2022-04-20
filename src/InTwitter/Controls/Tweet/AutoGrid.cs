using InTwitter.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public class AutoGrid : Grid
    {
        public AutoGrid()
            : base()
        {
            MaxColumns = 3;
            AutoheightRequest = 230;
        }

        public List<MediaSourceViewModel> ItemsSource
        {
            get => (List<MediaSourceViewModel>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(List<MediaSourceViewModel>), typeof(AutoGrid), coerceValue: OnItemsSourceCoerce);

        public DataTemplate Template { get; set; }

        public int MaxColumns { get; set; }

        public double AutoheightRequest { get; set; }

        private static object OnItemsSourceCoerce(BindableObject bindable, object value)
        {
            var grid = bindable as AutoGrid;
            var list = value as List<MediaSourceViewModel>;

            if (grid != null && list != null && list.Count != 0)
            {
                int column = list.Count > grid.MaxColumns ? grid.MaxColumns : list.Count;

                int row = (int)Math.Ceiling(list.Count / (column * 1.0));

                for (int i = 0; i < column; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = new GridLength(1, GridUnitType.Star),
                    });
                }

                for (int i = 0; i < row; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = new GridLength(1, GridUnitType.Star),
                    });
                }

                int counter = 0;

                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < column; j++)
                    {
                        var view = (View)grid.Template.CreateContent();
                        view.BindingContext = list[counter];
                        if (list.Count > 1)
                        {
                            view.SetBinding(View.HeightRequestProperty, new Binding(nameof(View.Width), source: view));
                        }

                        grid.Children.Add(view, j, i);

                        counter++;
                        if (counter >= list.Count)
                        {
                            break;
                        }
                    }
                }

                if (list.Count == 1)
                {
                    grid.HeightRequest = grid.AutoheightRequest;
                }
            }

            return value;
        }
    }
}
