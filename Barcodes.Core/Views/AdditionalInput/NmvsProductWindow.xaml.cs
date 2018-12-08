﻿using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModels.AdditionalInput;
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

namespace Barcodes.Core.Views.AdditionalInput
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class NmvsProductWindow : Window
    {
        public NmvsProductWindow(NmvsProductViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.CloseAction = () => Close();
        }
    }
}