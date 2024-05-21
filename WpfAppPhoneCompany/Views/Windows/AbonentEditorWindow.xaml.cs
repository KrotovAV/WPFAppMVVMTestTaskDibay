using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppPhoneCompany.Views.Windows
{
    /// <summary>
    /// Interaction logic for AbonentEditorWindow.xaml
    /// </summary>
    public partial class AbonentEditorWindow : Window
    {
        //#region Name
        //public static readonly DependencyProperty NameProperty =
        //    DependencyProperty.Register(
        //        nameof(Name),
        //        typeof(string),
        //        typeof(AbonentEditorWindow),
        //        new PropertyMetadata(null));

        //public string Name { get => (string)GetValue(NameProperty); set => SetValue(NameProperty, value); }
        //#endregion

        //#region SurName
        //public static readonly DependencyProperty SurNameProperty =
        //    DependencyProperty.Register(
        //        nameof(SurName),
        //        typeof(string),
        //        typeof(AbonentEditorWindow),
        //        new PropertyMetadata(null));

        //public string SurName { get => (string)GetValue(SurNameProperty); set => SetValue(SurNameProperty, value); }
        //#endregion


        //public AbonentEditorWindow() => InitializeComponent();
        public AbonentEditorWindow() 
        {
            InitializeComponent();
        }
    }
}
