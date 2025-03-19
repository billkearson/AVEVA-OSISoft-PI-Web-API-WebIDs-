using System.Text.Json;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Collections;


namespace PI_WEB_IDs
{
    public partial class Form1 : Form
    {
        // Create an instance of the PIWebAPI helper class
        PIWebAPI piwebapi = new PIWebAPI();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Provide a explicxit path to the specific .env file
            //string env = System.IO.Path.GetFullPath(@"..\..\..\") + @"\dev.env";
            string env = System.IO.Path.GetFullPath(@"..\..\..\") + @"\lab.env";
                        
            // Provide the .env file information foe the PIWebAPI properties
            piwebapi.PIAPIServer = Env.get_Env_Value("PIAPISERVER", env);
            piwebapi.PIAPIServerPort = Env.get_Env_Value("PIAPISERVERPORT", env);
            piwebapi.Username = Env.get_Env_Value("PIAPIUSERNAME", env);
            piwebapi.Password = Env.get_Env_Value("PIAPIPASSWORD", env);
            piwebapi.PIServer = Env.get_Env_Value("PISERVER", env);
            piwebapi.Debug = Convert.ToBoolean(Env.get_Env_Value("DEBUG", env));
            piwebapi.EnableSslValidation = Convert.ToBoolean(Env.get_Env_Value("ENABLESSLVALIDATION", env));
            piwebapi.UseDefaultCredentials = Convert.ToBoolean(Env.get_Env_Value("USEDEFAULTCREDENTIALS", env));

            // Populate the texyboxes with the initial infromation
            textBox1.Text = piwebapi.PIServer;
            textBox3.Text = piwebapi.PIAPIServer;
            textBox4.Text = piwebapi.PIAPIServerPort;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Build the PathOnly WebId client-side and display the information
            string webid = piwebapi.CreatePIPointPathWebID(textBox1.Text, textBox2.Text);            
            listBox1.Items.Add(webid);
            textBox5.Text = webid;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // Test the PathOnly WebID, get the PI Point attributes, and display the information
            string response = await piwebapi.ReadAttributedsFromWebID(textBox5.Text);
            Dictionary<string, JsonElement> nameValuePairs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(response);

            foreach (var kvp in nameValuePairs)
            {
                listBox1.Items.Add($"{kvp.Key}:{kvp.Value}");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            // Use the PathOnly WebID to get the current value for the PI Point and display the information
            string response = await piwebapi.ReadCurrentValueFromWebID(textBox5.Text);
            Dictionary<string, JsonElement> nameValuePairs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(response);

            foreach (var kvp in nameValuePairs)
            {
                listBox1.Items.Add($"{kvp.Key}:{kvp.Value}");

            }
        }
    }

































}
