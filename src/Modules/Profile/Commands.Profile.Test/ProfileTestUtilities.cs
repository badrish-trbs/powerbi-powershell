/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Diagnostics;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Profile.Test
{
    public static class ProfileTestUtilities
    {
        public static CmdletInfo ConnectPowerBIServiceAccountCmdletInfo { get; } = new CmdletInfo($"{ConnectPowerBIServiceAccount.CmdletVerb}-{ConnectPowerBIServiceAccount.CmdletName}", typeof(ConnectPowerBIServiceAccount));
        public static CmdletInfo DisconnectPowerBIServiceAccountCmdletInfo { get; } = new CmdletInfo($"{DisconnectPowerBIServiceAccount.CmdletVerb}-{DisconnectPowerBIServiceAccount.CmdletName}", typeof(DisconnectPowerBIServiceAccount));


        public static void ConnectToPowerBI(System.Management.Automation.PowerShell ps, PowerBIEnvironmentType? environment = null)
        {
            if (environment == null)
            {
#if DEBUG
                environment = PowerBIEnvironmentType.OneBox;
#else
                environment = PowerBIEnvironmentType.Public;
#endif
            }

            ps.AddCommand(ConnectPowerBIServiceAccountCmdletInfo).AddParameter(nameof(ConnectPowerBIServiceAccount.Environment), environment);

            var result = ps.Invoke();

            TestUtilities.AssertNoCmdletErrors(ps);
            Assert.IsNotNull(result);
            ps.Commands.Clear();
        }

        public static void DisconnectToPowerBI(System.Management.Automation.PowerShell ps)
        {
            ps.AddCommand(DisconnectPowerBIServiceAccountCmdletInfo);

            var result = ps.Invoke();

            TestUtilities.AssertNoCmdletErrors(ps);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            ps.Commands.Clear();
        }

        public static void SafeDisconnectFromPowerBI(System.Management.Automation.PowerShell ps)
        {
            try
            {
                DisconnectToPowerBI(ps);
            }
            catch (Exception ex)
            {
                // ignore, not part of the test
                Trace.TraceError(ex.ToString());
            } 
        }
    }
}
