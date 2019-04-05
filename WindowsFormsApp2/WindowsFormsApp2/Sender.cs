using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;

namespace WindowsFormsApp2
{
    public partial class Sender : Form
    {
        public Sender()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SinhVien sv = new SinhVien();
            sv.Name = txtTen.Text;
            txtTen.Clear();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsontxt = jss.Serialize(sv);
            IConnectionFactory factory = new ConnectionFactory("tcp://localhost:61616");
            IConnection con = factory.CreateConnection("admin", "admin");
            con.Start();
            ISession session = con.CreateSession(AcknowledgementMode.AutoAcknowledge);
            ActiveMQQueue destination = new ActiveMQQueue("MyQueue");
            IMessageProducer producer = session.CreateProducer(destination);

            IMessage msg = new ActiveMQTextMessage(jsontxt);
            producer.Send(msg);
            Console.WriteLine("Finished");

            session.Close();
            con.Close();


        }

        private void txtTen_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
