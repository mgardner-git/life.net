using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Life;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LifeMatrix activeLifeMatrix;
        private ObservableCollection<LifeMatrix> _lifeMatrices;
        Thread iterateThread;

        public MainWindow()
        {
            InitializeComponent();
            
            
        }

        public ObservableCollection<LifeMatrix> LifeMatrices { get { return _lifeMatrices; } }
        private void StopIterate(Object sender = null, RoutedEventArgs rea = null)
        {            
            if (iterateThread != null)
            {
                this.iterateForeverButton.IsEnabled = true;
                this.iterateButton.IsEnabled = true;
                iterateThread.Abort();
            }
        }


        private void disableButtons()
        {
            this.iterateForeverButton.IsEnabled = false;
            this.iterateButton.IsEnabled = false;

        }
        private void iterateForever()
        {
            Action disableAction = disableButtons;
            Dispatcher.Invoke(disableAction);
            try
            {
                while (true)
                {

                    iterate();
                    Thread.Sleep(500);
                }
            }
            catch (ThreadAbortException tae)
            {
                //this is a normal when they press the stop button, no need to do anything
            }
            catch (TaskCanceledException tce)
            {
                //this is normal when the user closes the window, no need to do anything
            }
        }

        private void StartIterateForeverThread(object sender = null, RoutedEventArgs e = null)
        {
            ThreadStart job = new ThreadStart(iterateForever);
            iterateThread = new Thread(job);
            iterateThread.Start();

        }

        public void Randomize(Object sender = null, RoutedEventArgs e = null)
        {
            lock (activeLifeMatrix)
            {
                activeLifeMatrix.Randomize();
            }
            Action updateAction = updateDisplay;
            this.Dispatcher.Invoke(updateDisplay);

        }
        
        private void updateDisplay()
        {
            lock (activeLifeMatrix)
            {
                activeLifeMatrix.UpdateDisplay(matrix);
            }
        }

        private void iterate(object sender=null, RoutedEventArgs e=null) {
            lock (activeLifeMatrix)
            {
                activeLifeMatrix.Iterate();
            }
            Action updateAction = updateDisplay;
            this.Dispatcher.Invoke(updateDisplay);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _lifeMatrices = new ObservableCollection<LifeMatrix>();
            LifeMatrix matrix1 = new StandardLife(20);
            matrix1.Randomize();
            _lifeMatrices.Add(matrix1);
            LifeMatrix matrix2 = new RedBlueLife(20);
            _lifeMatrices.Add(matrix2);
            matrix2.Randomize();
            LifeMatrix matrix3 = new GeneticLife(20);
            matrix3.Randomize();
            _lifeMatrices.Add(matrix3);

            activeLifeMatrix = matrix2;
            typesBox.DataContext = _lifeMatrices;

            matrix.RowDefinitions.Clear();

            for (int rowIndex = 0; rowIndex < activeLifeMatrix.Size; rowIndex++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength((int)(matrix.ActualHeight / activeLifeMatrix.Size));
                matrix.RowDefinitions.Add(rowDef);
            }
            matrix.ColumnDefinitions.Clear();
            for (int columnIndex = 0; columnIndex < activeLifeMatrix.Size; columnIndex++)
            {
                ColumnDefinition colDef = null;
                colDef = new ColumnDefinition();
                colDef.Width = new GridLength(matrix.ActualWidth / activeLifeMatrix.Size);
                matrix.ColumnDefinitions.Add(colDef);
            }
            
            activeLifeMatrix.Iterate();
            updateDisplay();


        }


        private void Window_Closed(object sender, EventArgs e)
        {

        }
        

        private void OnSelectType(object sender, SelectionChangedEventArgs e)
        {
            lock (activeLifeMatrix)
            {
                activeLifeMatrix = typesBox.SelectedItem as LifeMatrix;
                updateDisplay();
            }

            
        }
    }


}
