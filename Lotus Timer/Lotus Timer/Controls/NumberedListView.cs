﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LotusTimer.Models;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace LotusTimer.Controls
{
    
    public class NumberedListView : ListView
    {
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(NumberedListView), Color.White, BindingMode.TwoWay);

        public bool Increasing { get; set; }
        public Color TextColor { get; set; }
        protected override Cell CreateDefault(object item)
        {
            ViewCell cell = new ViewCell();
            List<object> items = new List<object>();
            foreach (var i in ItemsSource)
                items.Add(i);
            int index = items.IndexOf(item);
            Grid grid = new Grid();
            ColumnDefinition c0 = new ColumnDefinition();
            c0.Width = 40;
            grid.ColumnDefinitions.Add(c0);
            grid.ColumnSpacing = 10;
            Label number = new Label();
            if (Increasing)
                number.Text = (index + 1).ToString() + '.';
            else
                number.Text = (items.Count - index).ToString() + '.';
            number.FontAttributes = FontAttributes.Bold;
            number.TextColor = TextColor;
            number.HorizontalOptions = LayoutOptions.End;
            grid.Children.Add(number);

            Label text = new Label();
            text.Text = item.ToString();
            grid.Children.AddHorizontal(text);
            cell.View = grid;
            return cell;
        }
    }
}
