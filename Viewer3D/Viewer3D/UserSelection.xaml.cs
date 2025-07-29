using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Viewer3D
{
	/// <summary>
	/// Interaction logic for UserSelection.xaml
	/// </summary>
	public partial class UserSelection : Window
	{
		public int UserSelectionIndex { get; private set; }

		public UserSelection(string windowTitle, List<string> availableChoices)
		{
			InitializeComponent();

			// set title
			this.Title = windowTitle;

			// fill listbox
			for( int i = 0; i < availableChoices.Count; ++i )
			{
				ListBoxItem newItem = new ListBoxItem();
				newItem.Content = availableChoices[i];

				Listbox_Selection.Items.Add(newItem);
			}
			Listbox_Selection.SelectedIndex = 0;
		}

		private void Button_Cancel_Click(object sender, RoutedEventArgs e)
		{
			UserSelectionIndex = -1;
			this.Close();
		}

		private void Button_Select_Click(object sender, RoutedEventArgs e)
		{
			UserSelectionIndex = Listbox_Selection.SelectedIndex;
			this.Close();
		}
	}
}
