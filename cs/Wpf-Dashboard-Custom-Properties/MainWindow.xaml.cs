using System.Windows;
using DevExpress.Xpf.Bars;

namespace Wpf_Dashboard_Custom_Properties {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            dashboardControl.LoadDashboard("../../Dashboard/Dashboard.xml");
        }
    }
}
