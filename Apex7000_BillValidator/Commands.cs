﻿using System;

namespace Apex7000_BillValidator
{
    public partial class ApexValidator
    {
        /// <summary>
        /// Issue a stack command to the acceptor. Note that the acceptor must
        /// be configured to escrow mode and the bill must be in escrow. Otherwise,
        /// calling this command has no effect other than logging an error message.
        /// </summary>
        public void Stack()
        {
            if (!config.IsEscrowMode)
                log.Error("Not in escrow mode, stack command ignored!");

            // Set flag to accept
            // We do not directly instruct the unit to stack as this may interfere
            // with any pending transmissions.
            config.EscrowCommand = EscrowCommands.Stack;
        }

        /// <summary>
        /// Issue a reject command to the acceptor. Note that the acceptor must
        /// be configured to escrow mode and the bill must be in escrow. Otherwise,
        /// calling this command has no effect other than logging an error message.
        /// </summary>
        public void Reject()
        {
            if (!config.IsEscrowMode)
                log.Error("Not in escrow mode, reject command ignored!");

            // Set flag to reject
            // We do not directly instruct the unit to reject as this may interfere
            // with any pending transmissions.
            config.EscrowCommand = EscrowCommands.Reject;
        }
    }
}