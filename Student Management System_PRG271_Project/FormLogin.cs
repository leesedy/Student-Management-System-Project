using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Student_Management_System
{
    public partial class FormLogin : Form              // Windows Forms class named FormLogin, which inherits from Form. Responsible for the login screen
    {
        SqlConnection connect = new SqlConnection(@"Data Source = LESEDIMOHOLOENG\SQLEXPRESS; Initial Catalog = lesedi ; Integrated Security = True;"); //creates a connection to a SQL Server database.
                                                                                                                                                        //Data Source = database server used. Initial Catalog = database name used.
                                                                                                                                                        //Integrated Security =  Uses Windows Authentication
        public FormLogin()
        {
            InitializeComponent();
        }

        private void cbxShowPass_CheckedChanged(object sender, EventArgs e)       //  An event handler is used to Click on button to see password that was typed by the user.
        {
            txtPassword.PasswordChar = cbxShowPass.Checked ? '\0' : '*'; 
        }

        private void label1_Click(object sender, EventArgs e)           // An event handler for a label, that exits the application when the users wants to close the program.
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)     // core functionality for user login. It checks if the username and password fields are empty and then attempts to validate the user’s credentials.
                                                                    // Try and catch is used for commands and exeception handling.
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")           // To see if user has entered in all the required fields
            {
                MessageBox.Show("Please fill in the blank spaces", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    connect.Open();                 // else statement used for if all required fields are entered to take the next step.

                    string Select = "SELECT * FROM users WHERE username = @username AND password = @password ";
                    using (SqlCommand cmd = new SqlCommand(Select, connect))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count >= 1)
                        {
                            MessageBox.Show("Login Successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //to display the main form after login
                            MainForm mainForm = new MainForm();
                            mainForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Username/Password", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }
                }
                catch (Exception ex)            // To handle execptions that was not caught by the try function.
                {
                    MessageBox.Show("Error, failed to connect to database. " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally                     // Used to close the connection after its use.
                {
                    connect.Close();
                }

            }


        }
    }
}
