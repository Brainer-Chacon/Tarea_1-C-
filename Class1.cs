namespace Tarea_1

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace ChatApp
{
    public partial class MainForm : Form   // representa la ventana principal de la aplicación
    {
        private TcpListener server;   // Se utilizan para implementar el servidor.
        private TcpClient client;     // Se utilizan para implementar  el cliente.
        private int port;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)  // evento que se dispara cuando la ventana principal se carga. Aquí se inicializa el servidor de escucha de sockets.
        {
            // Configurar el puerto de escucha y iniciar el servidor
            port = ...; // Asignar el puerto recibido desde la línea de comandos
            StartServer(port);
        }

        private async void StartServer(int port)  // Inicia el servidor para escuchar conexiones entrantes.
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            while (true)
            {
                var client = await server.AcceptTcpClientAsync();
                HandleClient(client);
            }
        }

        private async void HandleClient(TcpClient client)   // Maneja los mensajes entrantes y los muestra en la interfaz
        {
            var stream = client.GetStream();
            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            // Mostrar el mensaje en la UI
            Invoke(new Action(() => listBoxMessages.Items.Add(message)));
        }

        private void SendMessage(string message, int destinationPort)  // Envía mensajes al puerto destino especificado por el usuario.
        {
            try
            {
                var client = new TcpClient("localhost", destinationPort);
                var stream = client.GetStream();
                var data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)  //controlador de eventos que se ejecuta cuando el usuario hace clic en el botón de enviar, capturando el mensaje y el puerto de destino.
        {
            string message = textBoxMessage.Text;
            int destinationPort = int.Parse(textBoxDestinationPort.Text);
            SendMessage(message, destinationPort);
        }
    }
}

