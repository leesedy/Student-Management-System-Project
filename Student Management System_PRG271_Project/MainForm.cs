using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Student_Management_System.BusinessLayer;

namespace Student_Management_System
{
    

    public partial class MainForm : Form
    {   
        private string studentfilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Students.txt");
        public MainForm()
        {
            InitializeComponent();                                  // Define columns for the DataGridView

            dgvStudentData.ColumnCount = 5;
            dgvStudentData.Columns[0].Name = "Student ID";
            dgvStudentData.Columns[1].Name = "Name";
            dgvStudentData.Columns[2].Name = "Surname";
            dgvStudentData.Columns[3].Name = "Age";
            dgvStudentData.Columns[4].Name = "Course";

            // Set default styles for DataGridView
            dgvStudentData.DefaultCellStyle.ForeColor = Color.Black; // Set text color
            dgvStudentData.DefaultCellStyle.BackColor = Color.White; // Set background color
            dgvStudentData.DefaultCellStyle.SelectionBackColor = Color.LightBlue; // Set selection background color
            dgvStudentData.DefaultCellStyle.SelectionForeColor = Color.Black; // Set selection text color
            LoadStudents();                         
            

        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            if (dgvStudentData.SelectedRows.Count > 0)                      // Check if a student row is selected
            {
                var selectedRow = dgvStudentData.SelectedRows[0];           // Select student ID from the selected row
                string studentID = selectedRow.Cells[0].Value.ToString();

                
                var confirmDeletion = MessageBox.Show("Are you sure you want to delete this student?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);          // Confirm the deletion

                if (confirmDeletion == DialogResult.Yes)
                {
                    
                    var studentRecords = File.ReadAllLines(studentfilepath).ToList();                       // Read all lines from the file and convert them to a list
                    
                    studentRecords = studentRecords.Where(line => line.Split(',')[0] != studentID).ToList();            // Find and remove the line with the matching Student ID

                    File.WriteAllLines(studentfilepath, studentRecords);                    // Write the updated list back to the file

                    dgvStudentData.Rows.Remove(selectedRow);                    // Remove the selected row from the DataGridView

                    MessageBox.Show("Student record deleted successfully!", "Delete Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a student record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnViewStud_Click(object sender, EventArgs e)
        {

            if (File.Exists(studentfilepath))                       // Check if the student data file exists
            {

                dgvStudentData.Rows.Clear();                // Clear existing rows before loading new data



                foreach (string line in File.ReadAllLines(studentfilepath))
                {

                    string[] studentData = line.Split(',');                    // Split each line by tabs


                    if (studentData.Length == 5)                    // Ensure there are exactly five elements before adding a row
                    {
                        dgvStudentData.Rows.Add(studentData[0], studentData[1], studentData[2], studentData[3], studentData[4]);
                    }
                    else
                    {
                        MessageBox.Show("The data format in the file is incorrect. Each line should have exactly five fields.", "Data Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("There is no student data found in the specified file path.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvStudentData.CurrentCell != null)                // Check if a cell is selected
                {
                    int index = dgvStudentData.CurrentCell.RowIndex;   // Get the index of the selected row in the DataGridView

                    var studentRecords = File.ReadAllLines(studentfilepath).ToList(); // Read all student records from the file into a list


                    if (index >= 0 && index < studentRecords.Count)                    // Ensure the index is within the range of the records list
                    {

                        var selectedRecord = studentRecords[index].Split(',');                        // Split the selected record's fields by comma (assuming data is comma-separated)


                        selectedRecord[1] = txtName.Text;                        // Update each field based on the TextBox inputs
                        selectedRecord[2] = txtSurname.Text;
                        selectedRecord[3] = txtAge.Text;
                        selectedRecord[4] = txtCourse.Text;


                        studentRecords[index] = string.Join(",", selectedRecord);                        // Replace the original record in the list with the updated one

                        File.WriteAllLines(studentfilepath, studentRecords);                        // Write the updated list of records back to the text file


                        MessageBox.Show("Student updated successfully!", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);                        // Notify the user of success

                        LoadStudents();                        // Refresh the DataGridView to show the updated data

                    }
                    else
                    {
                        MessageBox.Show("Invalid student record selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a student record to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the student record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }


        private void btnAddStud_Click(object sender, EventArgs e)           //This is an event handler for the "Add Student" button click event. It executes when the button is clicked.
        {
            string studentID = txtStudID.Text;    
            string name = txtName.Text;
            string sname=txtSurname.Text;
            int age;
            string course = txtCourse.Text;

            if (string.IsNullOrEmpty(studentID) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(sname) || !int.TryParse(txtAge.Text, out age) || string.IsNullOrEmpty(course))    //checks if any fields are empty or if the age is not a valid integer. If validation fails, it shows an error message and exits the method.
            {
                MessageBox.Show("Please enter valid data.");
                return;
            }

            string studentRecord = $"{studentID},{name},{sname},{age},{course}";                          //Creates a comma-separated string for the student record.
            File.AppendAllText("students.txt", studentRecord + Environment.NewLine);                    //Appends the student record to the students.txt file, adding a newline character at the end.
            MessageBox.Show("Student added successfully.");                                             // Displays a message box to notify0 the user that the student has been added.
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();             
        }

        private void LoadStudents()
        {

            dgvStudentData.Rows.Clear();                            // Clear existing rows

            try
            {

                MessageBox.Show("Looking for file at: " + studentfilepath);                // Display the file path for debugging

                if (!File.Exists(studentfilepath))
                {
                    MessageBox.Show("File not found at: " + studentfilepath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (string line in File.ReadAllLines(studentfilepath))         // Reads the data to the studentfilepath and shows how the code must be displayed within the text file.
                {
                    string[] studentDetails = line.Split(',');

                    if (studentDetails.Length == 5)
                    {
                        dgvStudentData.Rows.Add(studentDetails[0], studentDetails[1], studentDetails[2], studentDetails[3], studentDetails[4]);
                    }
                    else
                    {
                        MessageBox.Show("Incorrect format in students.txt.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenReport_Click(object sender, EventArgs e)
        {
            try
            {

                var studentRecords = File.ReadAllLines(studentfilepath);                   // Read all lines from the student file

                int totalStudents = studentRecords.Length;
                int totalAge = 0;

                foreach (var line in studentRecords)
                {
                    var studentData = line.Split(',');

                    if (int.TryParse(studentData[3], out int age))                      // Assume that Age is the third field in each line (index 3)
                    {
                        totalAge += age;
                    }
                }


                double averageAge = totalStudents > 0 ? (double)totalAge / totalStudents : 0;                // Calculate the average age


                string reportMessage = $"Total Students: {totalStudents}\nAverage Age: {averageAge:F2}";                // Display the report in a message box
                MessageBox.Show(reportMessage, "Student Report", MessageBoxButtons.OK, MessageBoxIcon.Information);


                string reportFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "summary.txt");                      // Save the report to summary.txt
                File.WriteAllText(reportFilePath, reportMessage);

                MessageBox.Show("Report generated successfully and saved to summary.txt.", "Report Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while generating the report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


        
