using System.IO;
using System.Web.Script.Serialization;

namespace EC2Utilities.Common.DebugHelper
{
    public static class CredentialHelper
    {
        private const string CredFilePath = @"C:\EC2Utility.Debug";

        public static DebugCredentials GetDebugCredentials()
        {
            if (!File.Exists(CredFilePath))
            {
                return CreateCredFile();
            }

            return PopulateFromCredFile();
        }

        private static DebugCredentials CreateCredFile()
        {
            DebugCredentials creds = GatherCredentials();

            FileStream credFile = File.Open(CredFilePath, FileMode.CreateNew);

            var serializer = new JavaScriptSerializer();

            string serializedCreds = serializer.Serialize(creds);

            var writer = new StreamWriter(credFile);

            writer.Write(serializedCreds);

            writer.Close();
            credFile.Close();

            return creds;
        }

        private static DebugCredentials GatherCredentials()
        {
            var dcForm = new DebugCredentialsDialog();

            dcForm.ShowDialog();

            return new DebugCredentials
                       {

                           AccessKeyId = dcForm.AccessKey,
                           SecretKey = dcForm.SecretKey,
                           Login = dcForm.Login,
                           Password = dcForm.Password,
                       };
        }

        private static DebugCredentials PopulateFromCredFile()
        {
            FileStream credFile = File.Open(CredFilePath, FileMode.Open);

            var reader = new StreamReader(credFile);

            var serializer = new JavaScriptSerializer();

            var creds = serializer.Deserialize<DebugCredentials>(reader.ReadToEnd());

            reader.Close();
            credFile.Close();

            return new DebugCredentials
                       {
                           AccessKeyId = creds.AccessKeyId,
                           SecretKey = creds.SecretKey,
                           Login = creds.Login,
                           Password = creds.Password
                       };
        }
    }
}