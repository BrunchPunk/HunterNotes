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
using System.IO;

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
                InitializeDecorationsTab();

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
            //Populate the Decorations group table with all Deocrations Name/Skill (one row per) values from the DB
            List<Decoration> allDecorations = HNDatabase.GetAllDecorations();

            tableLayoutPanelDecorations.RowStyles.Clear();
            tableLayoutPanelDecorations.RowCount = allDecorations.Count;

            for(int row = 0; row < allDecorations.Count; row++)
            {
                tableLayoutPanelDecorations.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayoutPanelDecorations.Controls.Add(new Label() { Text = allDecorations[row].Name }, 0, row);
                tableLayoutPanelDecorations.Controls.Add(new Label() { Text = allDecorations[row].Skill }, 1, row);

            }

            tableLayoutPanelDecorations.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            tableLayoutPanelDecorations.Refresh();
            
            // Update the Owned amounts in the Database based on the MyDeco.txt file
            if(File.Exists("MyDeco.txt"))
            {
                foreach(string line in File.ReadLines("MyDeco.txt"))
                {
                    if(line.Length > 0 && line[0] != '#')
                    {
                        string[] deco = line.Split('\t');
                        if(deco.Length == 2)
                        {
                            int newOwned; 

                            if(int.TryParse(deco[1], out newOwned))
                            {
                                HNDatabase.UpdateOwned(deco[0], newOwned);
                            }
                        }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("The MyDeco.txt file is missing.");
            }

            // Populate the My Decorations group table with all Decorations Name/Owned (one per row) values from the DB
            allDecorations = HNDatabase.GetAllDecorations();

            tableLayoutPanelMyDecorations.RowStyles.Clear();
            tableLayoutPanelMyDecorations.Controls.Clear();
            tableLayoutPanelMyDecorations.RowCount = allDecorations.Count;

            for (int row = 0; row < allDecorations.Count; row++)
            {
                tableLayoutPanelMyDecorations.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayoutPanelMyDecorations.Controls.Add(new Label() { Text = allDecorations[row].Name }, 0, row);
                tableLayoutPanelMyDecorations.Controls.Add(new NumericUpDown { Value = allDecorations[row].Owned }, 1, row);
            }

            tableLayoutPanelMyDecorations.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            tableLayoutPanelMyDecorations.Refresh();

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
            for(int index = 0; index < tableLayoutPanelMyDecorations.RowCount; index++)
            {
                string name = ((Label)tableLayoutPanelMyDecorations.GetControlFromPosition(0, index)).Text;
                ((NumericUpDown)tableLayoutPanelMyDecorations.GetControlFromPosition(1, index)).Value = HNDatabase.GetOwned(name);
            }
        }

        private void commitButton_Click(object sender, EventArgs e)
        {
            List<string> myDecoFile = new List<string>();
            myDecoFile.Add("# File for keeping track of your acquired decorations");
            myDecoFile.Add("# Lines starting with a '#' are ignored");
            myDecoFile.Add("# Decorations are specified one per line with the ");
            myDecoFile.Add("# Decoration name followed by a tab then the number owned");
            myDecoFile.Add("# e.g. ");
            myDecoFile.Add("# Antidote Jewel	2");

            for (int index = 0; index < tableLayoutPanelMyDecorations.RowCount; index++)
            {
                string name = ((Label)tableLayoutPanelMyDecorations.GetControlFromPosition(0, index)).Text;
                int value = Convert.ToInt32(((NumericUpDown)tableLayoutPanelMyDecorations.GetControlFromPosition(1, index)).Value);

                myDecoFile.Add(name + '\t' + value);
                HNDatabase.UpdateOwned(name, value);
            }

            File.WriteAllLines("MyDeco.txt", myDecoFile.ToArray());
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
