using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading.Tasks;

namespace Paralelismo___Tarea1
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=PEDROMAIRONI,1433;Database=ecommerce;Uid=pedro;Pwd=Juandejesus2930;Integrated Security=True;TrustServerCertificate=True;";

        public Form1()
        {
            InitializeComponent();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await _FetchProducts();
        }

        async Task _FetchProducts()
        {
            /* Get products */
            var firstTask = QueryAsynchronous("SELECT * FROM Producto");
            /* Get orders */
            var secondTask = QueryAsynchronous("SELECT * FROM Orden");

            await Task.WhenAll(firstTask, secondTask);
        }

        async Task QueryAsynchronous(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    await connection.OpenAsync();

                    // Use a SqlCommand to execute the query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        string threadInfo = $"Thread ID: {Thread.CurrentThread.ManagedThreadId}";
                        Invoke((Action)(() => textBox1.AppendText(threadInfo + Environment.NewLine)));
                        
                        Invoke((Action)(() => dataGridView1.DataSource = dataTable));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
