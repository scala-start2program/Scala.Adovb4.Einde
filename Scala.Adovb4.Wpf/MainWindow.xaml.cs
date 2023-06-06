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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Scala.Adovb4.Core.Entities;
using Scala.Adovb4.Core.Services;

namespace Scala.Adovb4.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MenuService menuService = new MenuService();
        bool isNew;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VulCombos();
            LinksActief();
            VulListbox();
        }

        private void LstMenus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControls();
            if (lstMenus.SelectedItem != null)
            {
                Core.Entities.Menu menu = (Core.Entities.Menu)lstMenus.SelectedItem;
                txtMenunaam.Text = menu.Menunaam;
                txtBereidingstijd.Text = menu.Bereidingstijd.ToString();
                txtIngredienten.Text = menu.Ingredienten;
                txtBereidingswijze.Text = menu.Bereidingswijze;
                cmbSoort.SelectedValue = menu.SoortId;
            }
        }

        private void CmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControls();
            VulListbox();
        }

        private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            cmbFilter.SelectedIndex = -1;
            VulListbox();
        }

        private void BtnNieuw_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;
            RechtsActief();
            ClearControls();
            txtMenunaam.Focus();
        }

        private void BtnWijzig_Click(object sender, RoutedEventArgs e)
        {
            if (lstMenus.SelectedItem != null)
            {
                isNew = false;
                RechtsActief();
                txtMenunaam.Focus();
            }
        }

        private void BtnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            if (lstMenus.SelectedItem != null)
            {
                if (MessageBox.Show("Ben je zeker?", "Menu verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Core.Entities.Menu menu = (Core.Entities.Menu)lstMenus.SelectedItem;
                    if (!menuService.MenuVerwijderen(menu))
                    {
                        MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ClearControls();
                    VulListbox();
                }
            }
        }

        private void BtnBewaren_Click(object sender, RoutedEventArgs e)
        {
            string menunaam = txtMenunaam.Text.Trim();
            if (menunaam.Length == 0)
            {
                MessageBox.Show("Menunaam invoeren", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMenunaam.Focus();
                return;
            }
            if (cmbSoort.SelectedIndex == -1)
            {
                MessageBox.Show("Soort opgeven", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbSoort.Focus();
                return;
            }
            string soortId = cmbSoort.SelectedValue.ToString();
            int.TryParse(txtBereidingstijd.Text, out int bereidingstijd);
            if (bereidingstijd < 0)
            {
                MessageBox.Show("Negatieve bereidingstijd verboden", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtBereidingstijd.Focus();
                return;
            }
            string ingredienten = txtIngredienten.Text.Trim();
            string bereidingswijze = txtBereidingswijze.Text.Trim();

            Core.Entities.Menu menu;
            if (isNew)
            {
                menu = new Core.Entities.Menu(menunaam, ingredienten, bereidingswijze, bereidingstijd, soortId);
                if (!menuService.MenuToevoegen(menu))
                {
                    MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                menu = (Core.Entities.Menu)lstMenus.SelectedItem;
                menu.Menunaam = menunaam;
                menu.Ingredienten = ingredienten;
                menu.Bereidingswijze = bereidingswijze;
                menu.Bereidingstijd = bereidingstijd;
                menu.SoortId = soortId;
                if (!menuService.MenuWijzigen(menu))
                {
                    MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            VulListbox();
            LinksActief();
            lstMenus.SelectedValue = menu.Id;
            LstMenus_SelectionChanged(null, null);
        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            LinksActief();
            LstMenus_SelectionChanged(null, null);
        }

        private void LinksActief()
        {
            grpMenus.IsEnabled = true;
            grpDetails.IsEnabled = false;
            btnBewaren.Visibility = Visibility.Hidden;
            btnAnnuleren.Visibility = Visibility.Hidden;
        }
        private void RechtsActief()
        {
            grpMenus.IsEnabled = false;
            grpDetails.IsEnabled = true;
            btnBewaren.Visibility = Visibility.Visible;
            btnAnnuleren.Visibility = Visibility.Visible;
        }
        private void VulListbox()
        {
            if (cmbFilter.SelectedIndex == -1)
                lstMenus.ItemsSource = menuService.GetMenus();
            else
            {
                Soort soort = (Soort)cmbFilter.SelectedItem;
                lstMenus.ItemsSource = menuService.GetMenus(soort.Id);
            }
            lstMenus.Items.Refresh();
        }
        private void ClearControls()
        {
            txtMenunaam.Text = "";
            txtBereidingstijd.Text = "";
            txtIngredienten.Text = "";
            txtBereidingswijze.Text = "";
            cmbSoort.SelectedIndex = -1;
        }
        private void VulCombos()
        {
            cmbFilter.ItemsSource = menuService.GetSoorten();
            cmbSoort.ItemsSource = menuService.GetSoorten();
        }
    }
}
