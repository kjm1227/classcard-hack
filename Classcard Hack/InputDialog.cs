using System;
using System.Windows.Forms;

namespace Classcard_Hack
{
    public partial class InputDialog : Form
    {
        public string InputResult { get; set; }

        public InputDialog()
        {
            InitializeComponent();
        }

        private void InputDialog_Load(object sender, EventArgs e)
        {

        }

        private void ScoreInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Return) 
            { 
                e.Handled = true;
                this.Close(); 
            }
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InputDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(ScoreInput.Text))
            {
                InputResult = ScoreInput.Text;
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
