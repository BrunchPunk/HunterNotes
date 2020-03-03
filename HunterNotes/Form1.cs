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
using System.Threading;

namespace HunterNotes
{
    //This is a test
    public partial class Form1 : Form
    {
        private List<ComboBox> solverSkillsComboBoxes = new List<ComboBox>();
        private List<NumericUpDown> solverSkillsUpDowns = new List<NumericUpDown>();
        private List<CheckBox> solverSkillsCheckBoxes = new List<CheckBox>();

        public Form1()
        {
            InitializeComponent();
            try
            {
                HNDatabase.Init();

                InitializeMaterialsTab();
                InitializeSkillsTab();
                InitializeDecorationsTab();
                InitializeArmorTab();
                InitializeForgeTab();
                InitializeSetBonusesTab();
                InitializeSolverTab();

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + "\nApplication will close", "Fatal Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        #region Tab Initializers
        private void InitializeSolverTab()
        {
            AutoCompleteStringCollection skillOptionsAutoCompleteList = new AutoCompleteStringCollection();
            List<string> skillOptions = new List<string>();

            skillOptions.AddRange(HNDatabase.GetAllSkillsNames());

            skillOptionsAutoCompleteList.AddRange(skillOptions.ToArray());

            comboBoxSkill1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBoxSkill1.AutoCompleteCustomSource = skillOptionsAutoCompleteList;
            comboBoxSkill1.Items.AddRange(skillOptions.ToArray());
            solverSkillsComboBoxes.Insert(0, comboBoxSkill1);
            solverSkillsUpDowns.Insert(0, numericUpDownSkill1);
            solverSkillsCheckBoxes.Insert(0, checkBoxSkill1);

            comboBoxSkill2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBoxSkill2.AutoCompleteCustomSource = skillOptionsAutoCompleteList;
            comboBoxSkill2.Items.AddRange(skillOptions.ToArray());
            solverSkillsComboBoxes.Insert(1, comboBoxSkill2);
            solverSkillsUpDowns.Insert(1, numericUpDownSkill2);
            solverSkillsCheckBoxes.Insert(1, checkBoxSkill2);

            comboBoxSkill3.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBoxSkill3.AutoCompleteCustomSource = skillOptionsAutoCompleteList;
            comboBoxSkill3.Items.AddRange(skillOptions.ToArray());
            solverSkillsComboBoxes.Insert(2, comboBoxSkill3);
            solverSkillsUpDowns.Insert(2, numericUpDownSkill3);
            solverSkillsCheckBoxes.Insert(2, checkBoxSkill3);

            comboBoxSkill4.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBoxSkill4.AutoCompleteCustomSource = skillOptionsAutoCompleteList;
            comboBoxSkill4.Items.AddRange(skillOptions.ToArray());
            solverSkillsComboBoxes.Insert(3, comboBoxSkill4);
            solverSkillsUpDowns.Insert(3, numericUpDownSkill4);
            solverSkillsCheckBoxes.Insert(3, checkBoxSkill4);

            comboBoxSkill5.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBoxSkill5.AutoCompleteCustomSource = skillOptionsAutoCompleteList;
            comboBoxSkill5.Items.AddRange(skillOptions.ToArray());
            solverSkillsComboBoxes.Insert(4, comboBoxSkill5);
            solverSkillsUpDowns.Insert(4, numericUpDownSkill5);
            solverSkillsCheckBoxes.Insert(4, checkBoxSkill5);

            comboBoxSkill6.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBoxSkill6.AutoCompleteCustomSource = skillOptionsAutoCompleteList;
            comboBoxSkill6.Items.AddRange(skillOptions.ToArray());
            solverSkillsComboBoxes.Insert(5, comboBoxSkill6);
            solverSkillsUpDowns.Insert(5, numericUpDownSkill6);
            solverSkillsCheckBoxes.Insert(5, checkBoxSkill6);

        }

        private void InitializeArmorTab()
        {
            BindingSource armorBS = new BindingSource {DataSource = HNDatabase.GetArmorDataSet().Tables[0].DefaultView };
            dataGridView1.DataSource = armorBS;
            //dataGridView1.DataSource = HNDatabase.GetArmorDataSet().Tables[0].DefaultView;

            dataGridView1.Columns[0].HeaderText = "Armor Name";
            dataGridView1.Columns[1].HeaderText = "Equip Slot";
            dataGridView1.Columns[2].HeaderText = "Skill 1 Name";
            dataGridView1.Columns[3].HeaderText = "Skill 1 Points";
            dataGridView1.Columns[4].HeaderText = "Skill 2 Name";
            dataGridView1.Columns[5].HeaderText = "Skill 2 Points";
            dataGridView1.Columns[6].HeaderText = "Skill 3 Name";
            dataGridView1.Columns[7].HeaderText = "Skill 3 Points";
            dataGridView1.Columns[8].HeaderText = "Skill 4 Name";
            dataGridView1.Columns[9].HeaderText = "Skill 4 Points";
            dataGridView1.Columns[10].HeaderText = "Decoration Size 1";
            dataGridView1.Columns[11].HeaderText = "Decoration Size 2";
            dataGridView1.Columns[12].HeaderText = "Decoration Size 3";
            dataGridView1.Columns[13].HeaderText = "Set Bonus";


            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        }

        private void InitializeForgeTab()
        {
            BindingSource forgeBS = new BindingSource { DataSource = HNDatabase.GetForgeDataSet().Tables[0].DefaultView };
            dataGridView2.DataSource = forgeBS;
            //dataGridView1.DataSource = HNDatabase.GetArmorDataSet().Tables[0].DefaultView;

            dataGridView2.Columns[0].HeaderText = "Armor Name";
            dataGridView2.Columns[1].HeaderText = "Ingredient 1";
            dataGridView2.Columns[2].HeaderText = "Ingredient 1 Count";
            dataGridView2.Columns[3].HeaderText = "Ingredient 2";
            dataGridView2.Columns[4].HeaderText = "Ingredient 2 Count";
            dataGridView2.Columns[5].HeaderText = "Ingredient 3";
            dataGridView2.Columns[6].HeaderText = "Ingredient 3 Count";
            dataGridView2.Columns[7].HeaderText = "Ingredient 4";
            dataGridView2.Columns[8].HeaderText = "Ingredient 4 Count";

            dataGridView2.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

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

            labelSkills.Text = "";

            textBoxSkills.Refresh();
            listBoxSkills.Refresh();
        }

        private void InitializeSetBonusesTab()
        {
            BindingSource setBonusesBS = new BindingSource { DataSource = HNDatabase.GetSetBonusesDataSet().Tables[0].DefaultView };
            dataGridView3.DataSource = setBonusesBS;
            //dataGridView1.DataSource = HNDatabase.GetArmorDataSet().Tables[0].DefaultView;

            dataGridView3.Columns[0].HeaderText = "Set Bonus Name";
            dataGridView3.Columns[1].HeaderText = "Skill 1 Name";
            dataGridView3.Columns[2].HeaderText = "Skill 1 Pieces Needed";
            dataGridView3.Columns[3].HeaderText = "Skill 2 Name";
            dataGridView3.Columns[4].HeaderText = "Skill 2 Pieces Needed";

            dataGridView3.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
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
                tableLayoutPanelDecorations.Controls.Add(new Label() { Text = allDecorations[row].Skill, AutoSize = true }, 1, row);
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

        #region Solver Tab Handlers
        private void handleSolverSkillUpdate(int skillNumber, string skillName)
        {
            Console.WriteLine("Skill " + skillNumber + ": " + skillName);

            List<string> skillNames = HNDatabase.GetAllSkillsNames();
            List<string> setBonusNames = HNDatabase.GetAllSetBonusesNames();

            //Check that a valid Skill name was entered
            if(skillNames.Contains(skillName))
            {
                // Update skill Maximum
                int skillMax = HNDatabase.GetSkillMaxPoints(skillName);
                solverSkillsUpDowns[skillNumber].Maximum = skillMax;

                //Update Up/Down value as necessary
                if (solverSkillsCheckBoxes[skillNumber].Checked)
                {
                    solverSkillsUpDowns[skillNumber].Value = skillMax;
                }
                else
                {
                    if (solverSkillsUpDowns[skillNumber].Value > skillMax)
                        solverSkillsUpDowns[skillNumber].Value = skillMax;
                }

            }
            else //Invalid Skill Entered - Reset combo box
            {
                solverSkillsComboBoxes[skillNumber].Text = "";
                solverSkillsUpDowns[skillNumber].Maximum = 100;

                if (solverSkillsCheckBoxes[skillNumber].Checked)
                    solverSkillsUpDowns[skillNumber].Value = 0;
            }

        }

        private void handleSolverCheckboxUpdate(int skillNumber, bool boxChecked)
        {
            if (boxChecked)
            {
                solverSkillsUpDowns[skillNumber].Enabled = false;
            }
            else
            {
                solverSkillsUpDowns[skillNumber].Enabled = true;
            }

            handleSolverSkillUpdate(skillNumber, solverSkillsComboBoxes[skillNumber].Text);

        }

        private void buttonSolve_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 6; i++)
            {
                solverSkillsComboBoxes[i].Enabled = false;
                solverSkillsUpDowns[i].Enabled = false;
                solverSkillsCheckBoxes[i].Enabled = false;
            }

            buttonSolve.Enabled = false;
            buttonReset.Enabled = false;

            textBoxSolverResults.Text = "Solving...";
            textBoxSolverResults.Refresh();

            Thread.Sleep(5000); //TODO - Place Holder! this will be where we call the solver

            for (int i = 0; i < 6; i++)
            {
                solverSkillsComboBoxes[i].Enabled = true;
                solverSkillsUpDowns[i].Enabled = true;
                solverSkillsCheckBoxes[i].Enabled = true;
            }

            buttonSolve.Enabled = true;
            buttonReset.Enabled = true;

            textBoxSolverResults.Text = "Solved!";
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 6; i++)
            {
                solverSkillsComboBoxes[i].Text = "";
                solverSkillsUpDowns[i].Value = 0;
                solverSkillsUpDowns[i].Maximum = 100;
                solverSkillsCheckBoxes[i].Checked = false;
            }

            textBoxSolverResults.Text = "";

        }

        //Skill 1 Handlers
        private void comboBoxSkill1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            handleSolverSkillUpdate(0, (string)comboBoxSkill1.SelectedItem);
        }

        private void comboBoxSkill1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                handleSolverSkillUpdate(0, comboBoxSkill1.Text);
            }
        }

        private void comboBoxSkill1_Leave(object sender, EventArgs e)
        {
            handleSolverSkillUpdate(0, comboBoxSkill1.Text);
        }

        private void checkBoxSkill1_CheckedChanged(object sender, EventArgs e)
        {
            handleSolverCheckboxUpdate(0, checkBoxSkill1.Checked);
        }

        //Skill 2 Handlers
        private void comboBoxSkill2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            handleSolverSkillUpdate(1, (string)comboBoxSkill2.SelectedItem);
        }

        private void comboBoxSkill2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                handleSolverSkillUpdate(1, comboBoxSkill2.Text);
            }
        }

        private void checkBoxSkill2_CheckedChanged(object sender, EventArgs e)
        {
            handleSolverCheckboxUpdate(1, checkBoxSkill2.Checked);
        }

        //Skill 3 Handlers
        private void comboBoxSkill3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            handleSolverSkillUpdate(2, (string)comboBoxSkill3.SelectedItem);
        }

        private void comboBoxSkill3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                handleSolverSkillUpdate(2, comboBoxSkill3.Text);
            }
        }

        private void checkBoxSkill3_CheckedChanged(object sender, EventArgs e)
        {
            handleSolverCheckboxUpdate(2, checkBoxSkill3.Checked);
        }

        //Skill 4 Handlers
        private void comboBoxSkill4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            handleSolverSkillUpdate(3, (string)comboBoxSkill4.SelectedItem);
        }

        private void comboBoxSkill4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                handleSolverSkillUpdate(3, comboBoxSkill4.Text);
            }
        }

        private void checkBoxSkill4_CheckedChanged(object sender, EventArgs e)
        {
            handleSolverCheckboxUpdate(3, checkBoxSkill4.Checked);
        }

        //Skill 5 Handlers
        private void comboBoxSkill5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            handleSolverSkillUpdate(4, (string)comboBoxSkill5.SelectedItem);
        }

        private void comboBoxSkill5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                handleSolverSkillUpdate(4, comboBoxSkill5.Text);
            }
        }

        private void checkBoxSkill5_CheckedChanged(object sender, EventArgs e)
        {
            handleSolverCheckboxUpdate(4, checkBoxSkill5.Checked);
        }

        //Skill 6 Handlers
        private void comboBoxSkill6_SelectionChangeCommitted(object sender, EventArgs e)
        {
            handleSolverSkillUpdate(5, (string)comboBoxSkill6.SelectedItem);
        }

        private void comboBoxSkill6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                handleSolverSkillUpdate(5, comboBoxSkill6.Text);
            }
        }

        private void checkBoxSkill6_CheckedChanged(object sender, EventArgs e)
        {
            handleSolverCheckboxUpdate(5, checkBoxSkill6.Checked);
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
