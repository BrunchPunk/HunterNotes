using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace HunterNotes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try
            {
                HNDatabase.Init();

                InitializeMaterialsTab();
                

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + "\nApplication will close", "Fatal Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        #region Tab Initializers
        private void InitializeMaterialsTab()
        {
            AutoCompleteStringCollection materialsAutoCompleteList = new AutoCompleteStringCollection();
            List<Material> materialsList = HNDatabase.GetAllMaterials();

            foreach(Material mat in materialsList)
            {
                materialsAutoCompleteList.Add(mat.Name);
                listBox1.Items.Add(mat.Name);
            }

            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = materialsAutoCompleteList;

            textBox1.Refresh();
            listBox1.Refresh();
        }

        #endregion

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            HNDatabase.Close();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            string selectedMaterial = (string) listBox1.SelectedItem;

            string materialDescription = HNDatabase.GetMaterialDescription(selectedMaterial);

            label1.Text = materialDescription;
        }

        // Handle the Enter Key Press in the Materials TextBox
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string enteredMaterial = (string)textBox1.Text;
                string materialDescription = HNDatabase.GetMaterialDescription(enteredMaterial);
                label1.Text = materialDescription;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string enteredMaterial = (string)textBox1.Text;
            string materialDescription = HNDatabase.GetMaterialDescription(enteredMaterial);
            label1.Text = materialDescription;
        }
    }
}
