﻿using Apex7000_BillValidator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace Apex7000_BillValidator_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApexValidator validator;
        private RS232Config config;

        private static Dictionary<int, int> currencyMap = new Dictionary<int, int>();
        private Dictionary<int, long> cashbox = new Dictionary<int, long>();

        // Configure our map
        static MainWindow()
        {
            currencyMap.Add(1, 1);
            currencyMap.Add(2, 2);
            currencyMap.Add(3, 5);
            currencyMap.Add(4, 10);
            currencyMap.Add(5, 20);
            currencyMap.Add(6, 50);
            currencyMap.Add(7, 100);
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Testing on CAN firmware using escrow mode
            config = new RS232Config("COM4", CultureInfo.CurrentCulture, true);
            validator = new ApexValidator(config);

            validator.OnPowerUp += validator_PowerUp;
            validator.IsIdling += validator_IsIdling;
            validator.IsAccepting += validator_IsAccepting;
            validator.IsReturning += validator_IsReturning;
            validator.IsStacking +=validator_IsStacking;

            validator.IsEscrowed += validator_OnEscrow;
            validator.OnCredit += validator_OnCredit;

            validator.OnBillStacked += validator_BillStacked;
            validator.OnBillReturned += validator_OnBillReturned;
            
            validator.OnError += validator_OnError;
            validator.OnCashboxAttached += validator_CashboxAttached;

            validator.Connect();
        }

        void validator_OnBillReturned(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void validator_IsStacking(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void validator_IsReturning(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void validator_IsIdling(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void validator_IsAccepting(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void validator_OnError(object sender, ErrorTypes type)
        {
            Console.WriteLine("Error has occured: {0}", type.ToString());
        }

        void validator_CashboxAttached(object sender, EventArgs e)
        {
            Console.WriteLine("Box Attached");
        }

        void validator_BillStacked(object sender, EventArgs e)
        {            
            Console.WriteLine("Bill Stacked");
        }

        void validator_OnEscrow(object sender, int denomination)
        {
            validator.Stack();
            if(currencyMap.ContainsKey(denomination))
                Console.WriteLine("Escrowed ${0}", currencyMap[denomination]);
        }

        private void validator_OnCredit(object sender, int denomination)
        {
            if (currencyMap.ContainsKey(denomination))
            {
                var val = currencyMap[denomination];
                Console.WriteLine("Credited ${0}", AddCredit(val));
            }
        }

        void validator_PowerUp(object sender, EventArgs e)
        {
            Console.WriteLine("Acceptor Powered Up");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.Write(config.GetDebugBuffer());
        }
    }
}
