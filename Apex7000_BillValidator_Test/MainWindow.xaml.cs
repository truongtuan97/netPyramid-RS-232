﻿using Apex7000_BillValidator;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Apex7000_BillValidator_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApexValidator validator;
        private RS232Config config;

        private FixedObservableLinkedList<DebugBufferEntry> debugQueue;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            debugQueue = new FixedObservableLinkedList<DebugBufferEntry>(20);
            AvailablePorts.ItemsSource = ApexValidator.GetAvailablePorts();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            PortName = AvailablePorts.Text;
            if (string.IsNullOrEmpty(PortName))
            {                
                MessageBox.Show("Please select a port");
                return;
            }

            // Testing on CAN firmware using escrow mode
            config = new RS232Config(PortName, true);
            validator = new ApexValidator(config);

            // Configure logging
            config.OnSerialData += config_OnSerialData;
            ConsoleLogger.ItemsSource = debugQueue;

            // Configure events and state (All optional)
            validator.OnEvent += validator_OnEvent;
            validator.OnStateChanged += validator_OnStateChanged;
            validator.OnError += validator_OnError;
            validator.OnCashboxAttached += validator_CashboxAttached;

            // Required if you are in escrow mode
            validator.OnEscrowed += validator_OnEscrow;
            validator.OnCredit += validator_OnCredit;
            
            // This starts the acceptor
            validator.Connect();
        }

        void config_OnSerialData(object sender, DebugBufferEntry entry)
        {
            DoOnUIThread(() =>
            {
                debugQueue.Add(entry);

                ConsoleLogger.ScrollIntoView(entry);

            });
        }
               
        void validator_OnStateChanged(object sender, States state)
        {
            State = state;
        }

        void validator_OnEvent(object sender, Events e)
        {
            switch(e)
            {
                case Events.BillRejected:
                    setEvent(btnRejected);
                    break;
                case Events.Cheated:
                    setEvent(btnCheated);
                    break;
                case Events.PowerUp:
                    setEvent(btnPup);
                    break;
                case Events.Returned:
                    setEvent(btnReturned);
                    break;
                case Events.Stacked:
                    setEvent(btnStacked);
                    break;
            }
        }
        void validator_CashboxAttached(object sender, EventArgs e)
        {
            Console.WriteLine("Box Attached");
            setState(btnCB);
        }

        void validator_OnError(object sender, Errors type)
        {
            Console.WriteLine("Error has occured: {0}", type.ToString());


            switch (type)
            {
                  case Errors.CashboxMissing:
                    setError(btnCB);
                    break;
                case Errors.ChecksumError:
                    // TODO
                    break;
                case Errors.InvalidCommand:
                    // TODO
                    break;
                case Errors.PortError:
                    // TODO
                    break;
                case Errors.Timeout:
                    // TODO
                    break;
                case Errors.WriteError:
                    // TODO
                    break;
            }

        }

        private void validator_OnCredit(object sender, int denomination)
        {
            if (currencyMap.ContainsKey(denomination))
            {
                var val = currencyMap[denomination];
                Console.WriteLine("Credited ${0}", AddCredit(val));
            }
        }

        void validator_OnEscrow(object sender, int denomination)
        {
            validator.Stack();
            State = States.Escrowed;

            if (currencyMap.ContainsKey(denomination))
                Console.WriteLine("Escrowed ${0}", currencyMap[denomination]);
        }       
    }
    
}
