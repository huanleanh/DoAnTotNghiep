using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System.Web.Script.Serialization;

namespace WindowsFormsApp2
{
    public partial class Receiver : Form
    {
        svEntities1 svm = new svEntities1();
        public Receiver()
        {
            InitializeComponent();
        }

        private void btnReceiver_Click(object sender, EventArgs e)
        {
            IConnectionFactory factory = new ConnectionFactory("tcp://localhost:61616");
            IConnection con = factory.CreateConnection("admin", "admin");
            con.Start();

            ISession session = con.CreateSession(AcknowledgementMode.AutoAcknowledge);
            ActiveMQQueue destination = new ActiveMQQueue("MyQueue");

            IMessageConsumer consumer = session.CreateConsumer(destination);
            IMessage message = consumer.Receive();

            if (message is ITextMessage)
            {
                ITextMessage msg = message as ITextMessage;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                SinhVien sv = jss.Deserialize<SinhVien>(msg.Text);
                txtName.Text = sv.Name;
            }
            else
            {
                Console.WriteLine("Unexpected message type: " + message.GetType().Name);
            }
            

            session.Close();
            con.Close();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            sv s = new sv();
            s.NameSV = txtName.Text;
            svm.sv.Add(s);
            svm.SaveChanges();
            //txtName.Clear();
 
        }
    }
}
