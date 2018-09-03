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
using System.Globalization;

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
                InitializeSkillsTab();
                

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
                listBoxMaterials.Items.Add(mat.Name);
            }

            textBoxMaterials.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxMaterials.AutoCompleteCustomSource = materialsAutoCompleteList;

            textBoxMaterials.Refresh();
            listBoxMaterials.Refresh();
        }

        private void InitializeSkillsTab()
        {
            AutoCompleteStringCollection skillsAutoCompleteList = new AutoCompleteStringCollection();
            List<Skill> skillsList = HNDatabase.GetAllSkills();

            foreach (Skill skill in skillsList)
            {
                skillsAutoCompleteList.Add(skill.Name);
                listBoxSkills.Items.Add(skill.Name);
            }

            textBoxSkills.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxSkills.AutoCompleteCustomSource = skillsAutoCompleteList;

            textBoxSkills.Refresh();
            listBoxSkills.Refresh();
        }

        private void InitializeDecorationsTab()
        {
            //TODO - Populate the Decorations group table with all Deocrations Name/Skill (one row per) values from the DB

            //TODO - Update the Owned amounts in the Database based on the MyDeco.txt file

            //TODO - Populate the My Decorations group table with all Decorations Name/Owned (one per row) values from the DB

        }

        #endregion

        #region Tab Handlers
        #region Materials Tab Handlers
        private void listBoxMaterials_DoubleClick(object sender, EventArgs e)
        {
            string selectedMaterial = (string) listBoxMaterials.SelectedItem;

            string materialDescription = HNDatabase.GetMaterialDescription(selectedMaterial);

            labelMaterials.Text = materialDescription;
        }

        // Handle the Enter Key Press in the Materials TextBox
        private void textBoxMaterials_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string enteredMaterial = (string)textBoxMaterials.Text;
                enteredMaterial = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(enteredMaterial);
                string materialDescription = HNDatabase.GetMaterialDescription(enteredMaterial);
                labelMaterials.Text = materialDescription;
            }
        }

        private void buttonMaterials_Click(object sender, EventArgs e)
        {
            string enteredMaterial = (string)textBoxMaterials.Text;
            string materialDescription = HNDatabase.GetMaterialDescription(enteredMaterial);
            labelMaterials.Text = materialDescription;
        }

        #endregion

        #region Skills Tab Handlers
        private void listBoxSkills_DoubleClick(object sender, EventArgs e)
        {
            string selectedSkill = (string)listBoxSkills.SelectedItem;

            string skillDescription = HNDatabase.GetSkillDescription(selectedSkill);

            labelSkills.Text = skillDescription;
        }

        // Handle the Enter Key Press in the Materials TextBox
        private void textBoxSkills_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string enteredSkill = (string)textBoxSkills.Text;
                enteredSkill = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(enteredSkill);
                string skillDescription = HNDatabase.GetSkillDescription(enteredSkill);
                labelSkills.Text = skillDescription;
            }
        }

        private void buttonSkills_Click(object sender, EventArgs e)
        {
            string enteredSkill = (string)textBoxSkills.Text;
            string skillDescription = HNDatabase.GetSkillDescription(enteredSkill);
            labelSkills.Text = skillDescription;
        }

        #endregion

        #region Decorations Tab Handlers
        private void revertButton_Click(object sender, EventArgs e)
        {
            //TODO - Update all owned amounts in the My Decorations group table based on the database values
        }

        private void commitButton_Click(object sender, EventArgs e)
        {
            //TODO - Update the database owned amounts with whatever is in teh My Decorations group table
        }

        #endregion

        #endregion

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            HNDatabase.Close();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}
