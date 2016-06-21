using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;

namespace MilbrandtFPDB
{
    public class SecurityHelper
    {
        private const string PW = "0A58DCF5D6A2BC30B1663A97350B384E";

        public static bool VerifyPassword(string input)
        {
            return HashString(input) == PW;
        }

        private static string HashString(string input)
        {
            HashAlgorithm alg = new MD5Cng();

            byte[] inputArray = ASCIIEncoding.ASCII.GetBytes(input);
            byte[] outputArray = alg.ComputeHash(inputArray);

            return MakeHexString(outputArray);
        }

        private static string MakeHexString(byte[] input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in input)
            {
                int digit1 = b / 16;
                int digit2 = b % 16;

                sb.Append(GetHexChar(digit1));
                sb.Append(GetHexChar(digit2));
            }

            return sb.ToString();
        }

        private static char GetHexChar(int digit)
        {
            if (digit < 10)
                return digit.ToString()[0];
            else
                return (char)('A' + (digit - 10));
        }

        const string exePath = "ExternalApps/FPDB_Authorize.exe";
        const string varName = "MBAUTH";
        const string varVal = "true";
        public static bool CheckAuthorization()
        {
            string val = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);
            return val == varVal;
        }

        public static void AuthorizeCurrent()
        {
            RunAuthorizer("Authorize " + varName + " " + varVal);
        }

        public static void DeauthorizeCurrent()
        {
            RunAuthorizer("Deauthorize " + varName);
        }

        private static void RunAuthorizer(string args)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Path.Combine(Directory.GetCurrentDirectory(), exePath), args);
            p.StartInfo.Verb = "runas";
            p.Start();
            p.WaitForExit();

            if (p.ExitCode == -1)
                throw new System.Security.SecurityException();
        }
    }
}
