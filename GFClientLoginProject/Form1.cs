using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace GFClientLoginProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Carregar as credenciais salvas ao abrir o formul�rio
            CarregarCredenciais();
        }

        // M�todo para carregar as �ltimas credenciais salvas
        private void CarregarCredenciais()
        {
            // Carrega o login e senha (em MD5) salvos
            txtLogin.Text = Properties.Settings.Default.Login;

            // Verificar se a op��o de salvar a senha estava marcada
            SVPassCB.Checked = Properties.Settings.Default.SVPass;

            //SVPassCB.Checked ? txtSenha.Text = Properties.Settings.Default.Senha : txtSenha.Text = "";

            txtSenha.Text = (SVPassCB.Checked ? Properties.Settings.Default.Senha : "");

            // Se quiser carregar a senha original (desabilitado por padr�o para seguran�a)
            // txtSenha.Text = Properties.Settings.Default.Senha;
        }

        // M�todo para salvar as credenciais no arquivo de configura��es
        private void SalvarCredenciais(string login, string senha, bool svpassck)
        {
            Properties.Settings.Default.Login = login;
            Properties.Settings.Default.Senha = (svpassck ? senha : ""); // Ou senha em MD5, dependendo da sua necessidade
            Properties.Settings.Default.SVPass = svpassck;
            Properties.Settings.Default.Save(); // Salva os valores
        }

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            // Obter os valores dos campos de login e senha
            string login = txtLogin.Text;
            string senha = txtSenha.Text;

            // Salvar as credenciais digitadas
            SalvarCredenciais(login, senha, SVPassCB.Checked);



            // Converter a senha para MD5
            string senhaHashMD5 = GerarHashMD5(senha);

            // Conte�do do script .bat (insira aqui o conte�do do seu .bat)
            string batScript = "@echo off\n" +
                               "A:\n" +
                               "cd\\GAMESALPHA\\GrandFantasia Server Files\\074\\Ultimate\\ \n " +
                               "start GrandFantasia.exe EasyFun -a %1 -p %2 \n"; //+
                               //"echo Parametros recebidos: %1 %2 \n" +
                               //"pause";

            // Caminho tempor�rio para criar o arquivo .bat em tempo de execu��o
            string tempBatPath = Path.Combine(Path.GetTempPath(), "tempScript.bat");

            // Escreve o conte�do do .bat em um arquivo tempor�rio
            File.WriteAllText(tempBatPath, batScript);

            // Monta os argumentos opcionais de login e senha
            string argumentos = string.Empty;

            if (!string.IsNullOrEmpty(login))
            {
                argumentos += $" {login}";
            }

            if (!string.IsNullOrEmpty(senhaHashMD5))
            {
                argumentos += $" {senhaHashMD5}";
            }

            // Cria as informa��es do processo para executar o .bat tempor�rio
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = tempBatPath;
            processInfo.Arguments = argumentos;
            processInfo.UseShellExecute = false;

            try
            {
                // Inicia o processo
                Process process = Process.Start(processInfo);
                process.WaitForExit(); // Aguarda o processo terminar

                //MessageBox.Show("Comando executado com sucesso.");

                // Encerra o programa, j� que tudo correu bem
                Application.Exit();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao executar o comando: {ex.Message}");
            }
            finally
            {
                // Remove o arquivo .bat tempor�rio ap�s a execu��o
                if (File.Exists(tempBatPath))
                {
                    File.Delete(tempBatPath);
                }
            }
        }

        // M�todo para converter o texto da senha em hash MD5
        private string GerarHashMD5(string input)
        {
            // Inicializa o MD5
            using (MD5 md5 = MD5.Create())
            {
                // Converte o input para bytes
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);

                // Gera o hash em bytes
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Converte os bytes para string hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // "X2" gera string em formato hexadecimal
                }

                return sb.ToString(); // Retorna o hash MD5 como string
            }

        }

        private void ManualLogin_Click(object sender, EventArgs e)
        {

            // Conte�do do script .bat (insira aqui o conte�do do seu .bat)
            string batScript = "@echo off\n" +
                               "A:\n" +
                               "cd\\GAMESALPHA\\GrandFantasia Server Files\\074\\Ultimate\\ \n " +
                               "start GrandFantasia.exe EasyFun";


            // Caminho tempor�rio para criar o arquivo .bat em tempo de execu��o
            string tempBatPath = Path.Combine(Path.GetTempPath(), "tempScript.bat");

            // Escreve o conte�do do .bat em um arquivo tempor�rio
            File.WriteAllText(tempBatPath, batScript);


            // Cria as informa��es do processo para executar o .bat tempor�rio
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = tempBatPath;
            processInfo.UseShellExecute = false;

            try
            {
                // Inicia o processo
                Process process = Process.Start(processInfo);
                process.WaitForExit(); // Aguarda o processo terminar

                //MessageBox.Show("Comando executado com sucesso.");
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Erro ao executar o comando: {ex.Message}");
            }
            finally
            {
                // Remove o arquivo .bat tempor�rio ap�s a execu��o
                if (File.Exists(tempBatPath))
                {
                    File.Delete(tempBatPath);
                }
            }
        }
    }
}
