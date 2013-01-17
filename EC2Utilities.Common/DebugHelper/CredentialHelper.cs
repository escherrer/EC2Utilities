using System.Diagnostics;
using System.IO;
using System.Web.Script.Serialization;

namespace EC2Utilities.Common.DebugHelper
{
    public static class CredentialHelper
    {
        private const string CredFileName = @"EC2Utility.Debug";
        private const string CredFileDirectory = @"C:\EC2Utility\";

        public static DebugCredentials GetDebugCredentials()
        {
            if (!File.Exists(CredFileDirectory + CredFileName))
            {
                return CreateCredFile();
            }

            return PopulateFromCredFile();
        }

        private static DebugCredentials CreateCredFile()
        {
            var creds = new DebugCredentials
            {
                AccessKeyId = "Put Access Key Here",
                SecretKey = "Put Secret Key Here"
            };

            if (!Directory.Exists(CredFileDirectory))
                Directory.CreateDirectory(CredFileDirectory);

            FileStream credFile = File.Open(CredFileDirectory + CredFileName, FileMode.CreateNew);

            var serializer = new JavaScriptSerializer();

            string serializedCreds = serializer.Serialize(creds);

            var writer = new StreamWriter(credFile);

            writer.Write(serializedCreds);

            writer.Close();
            credFile.Close();

            string debugMsg = string.Format(
                    "A file has been created at {0} for purposes of storing EC2 credentials locally for debugging. Please edit this file to provide EC2 credentials to use while debugging. When not debugging these credentials are read from App.config.",
                    CredFileDirectory + CredFileName);
            Debug.Assert(false, debugMsg);

            return creds;
        }

        private static DebugCredentials GatherCredentials()
        {
            var dcForm = new DebugCredentialsDialog();

            dcForm.ShowDialog();

            return new DebugCredentials
                       {

                           AccessKeyId = dcForm.AccessKey,
                           SecretKey = dcForm.SecretKey
                       };
        }

        private static DebugCredentials PopulateFromCredFile()
        {
            FileStream credFile = File.Open(CredFileDirectory + CredFileName, FileMode.Open);

            var reader = new StreamReader(credFile);

            var serializer = new JavaScriptSerializer();

            var creds = serializer.Deserialize<DebugCredentials>(reader.ReadToEnd());

            reader.Close();
            credFile.Close();

            return new DebugCredentials
                       {
                           AccessKeyId = creds.AccessKeyId,
                           SecretKey = creds.SecretKey
                       };
        }
    }
}