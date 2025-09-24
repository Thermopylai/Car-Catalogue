using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Car_Catalogue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Car> cars = new List<Car>();
        private List<string> inputData = new List<string>();
        private string? gearType;
        private string? engineType;
        
        public MainWindow()
        {
            InitializeComponent();
            Clear_All();        
        }

        private void Clear_All() 
        { 
            cars.Clear();
            inputData.Clear();
            gearType = null;
            engineType = null;
            ClearTextBoxes();
            SetRadioButtonsOff();
        }

        private void ClearTextBoxes()
        {
            txtMake.Text = "";
            txtModel.Text = "";
            txtColor.Text = "";
            txtMaxSpeed.Text = "";
            txtCarsIndex.Text = "";
            txtFilePath.Text = "";
            lblModel.Content = "";
            lblMake.Content = "";
            lblColor.Content = "";
            lblMaxSpeed.Content = "";
            lblCarCount.Content = 0;
            txtMake.Focus();   
        }

        private void SetRadioButtonsOff()
        {
            rbManual.IsChecked = false;
            rbAutomatic.IsChecked = false;
            rbRobotic.IsChecked = false;
            rbGasoline.IsChecked = false;
            rbDiesel.IsChecked = false;
            rbElectric.IsChecked = false;
        }
        private void btnAddCar(object sender, RoutedEventArgs e)
        {
            int maxSpeed;
            if (int.TryParse((txtMaxSpeed.Text), out maxSpeed) && gearType != null && engineType != null)
            {
                cars.Add(new Car(txtMake.Text, txtModel.Text, txtColor.Text, maxSpeed, gearType, engineType));
                ClearTextBoxes();
                SetRadioButtonsOff();
                ViewCars(cars.Count - 1);
                MessageBox.Show("Car added.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (gearType == null || engineType == null)
            {
                ClearTextBoxes();
                SetRadioButtonsOff();
                MessageBox.Show("Please, select Gear Type and Engine Type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ClearTextBoxes();
                SetRadioButtonsOff();
                MessageBox.Show("Invalid Max Speed. Please, enter an integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnViewCar(object sender, RoutedEventArgs e)
        {
            int carsIndex;
            if (!int.TryParse((txtCarsIndex.Text), out carsIndex))
            {
                MessageBox.Show("Invalid index number. Please, enter an integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            carsIndex--;
            if (carsIndex < cars.Count)
            {
                ViewCars(carsIndex);    
            }
            else
            {
                MessageBox.Show($"Index too high. The current Car Count is {cars.Count}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ViewCars(int carsIndex)
        {

            lblModel.Content = cars[carsIndex].Model;
            lblMake.Content = cars[carsIndex].Make;
            lblColor.Content = cars[carsIndex].Color;
            lblMaxSpeed.Content = $"{cars[carsIndex].MaxSpeed} km/h";
            lblCarCount.Content = cars.Count;
            txtCarsIndex.Text = $"{carsIndex + 1}";
            gearType = cars[carsIndex].GearType;
            switch (gearType)
            {
                case "Manual":
                    rbManual.IsChecked = true; break;
                case "Automatic":
                    rbAutomatic.IsChecked = true; break;
                case "Robotic":
                    rbRobotic.IsChecked = true; break;
            }
            engineType = cars[carsIndex].EngineType;
            switch (engineType)
            {
                case "Gasoline":
                    rbGasoline.IsChecked = true; break;
                case "Diesel":
                    rbDiesel.IsChecked = true; break;
                case "Electric":
                    rbElectric.IsChecked = true; break;
            }
        }

        private void GearButtonsChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton button && button.Content != null)
            {
                gearType = button.Content.ToString();
            }
        }

        private void EngineButtonsChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton button && button.Content != null)
            {
                engineType = button.Content.ToString();
            }
        }

        private void btnClearAll(object sender, RoutedEventArgs e) => Clear_All();
       
        private void btnSaveAll(object sender, RoutedEventArgs e)
        {
            string filePath = txtFilePath.Text;
            if (filePath == "")
            {
                MessageBox.Show("Please, specify path and filename.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            else if (File.Exists(filePath))
            {
                MessageBox.Show($"Warning: File {filePath} exists already.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var car in cars)
                        writer.Write(car.GiveDetails());
                    writer.Close();
                    txtFilePath.Text = "";
                    MessageBox.Show($"Details saved in {filePath}.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        
        private void btnReadFile(object sender, RoutedEventArgs e)
        {
            string filePath = txtFilePath.Text;
            if (filePath != "" && File.Exists(filePath))
            {
                Clear_All();
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null){
                        if (line != "")
                        {
                            inputData.Add(line);
                        }
                    }
                    reader.Close();
                }
            }
            else
            {
                MessageBox.Show("Please, specify a valid path and filename.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int carCount = inputData.Count / 4;
            int rowIndex = 0;
            if (carCount > 0) 
            {
                for (int i = 0; i < carCount; i++) 
                {
                    string[] row1 = inputData[rowIndex].Split([',', ':'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    rowIndex++;
                    string[] row2 = inputData[rowIndex].Split([':', ' '], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    rowIndex++;
                    string[] row3 = inputData[rowIndex].Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    rowIndex++;
                    string[] row4 = inputData[rowIndex].Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    rowIndex++;
                    try
                    {
                        cars.Add(new Car(row1[1], row1[3], row1[5], int.Parse(row2[2]), row3[1], row4[1]));
                    }
                    catch
                    {
                        MessageBox.Show("Invalid data format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Clear_All();
                        return;
                    }
                }
                MessageBox.Show($"Details read from {filePath}.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                ViewCars(0);
            }
        }
    }
}