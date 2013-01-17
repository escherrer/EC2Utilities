using System.Windows.Forms;

namespace EC2Utilities.Common.DebugHelper
{
    public partial class DebugCredentialsDialog : Form
    {
        public DebugCredentialsDialog()
        {
            InitializeComponent();
        }

        public string AccessKey
        {
            get { return txtAccessKey.Text; }
        }

        public string SecretKey
        {
            get { return txtSecretKey.Text; }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
