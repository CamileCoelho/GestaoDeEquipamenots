using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace GestaoDeEquipamenots.ConsoleApp
{
    internal class Program
    {
        static List<string> equipamentosCadastrados = new List<string>();
        static List<string> chamadosCadastrados = new List<string>();
        static List<int> chamadosExcluidos = new List<int>();
        static bool continuar = true;
        static int idEquipamento = 0, idChamado = 0;

        static void Main(string[] args)
        {
            do
            {
                string operacao = ExecutarMenuInicial();
                
                switch (operacao)
                {
                    case "S":
                        continuar = false;
                        Console.ResetColor();
                        break;
                    case "1":
                        while (continuar)
                        {
                            string operacaoEmEquipamentos = MostarMenuControleEquipamentos();

                            if (operacaoEmEquipamentos != "1" && operacaoEmEquipamentos != "2" && operacaoEmEquipamentos != "3" && operacaoEmEquipamentos != "4" && operacaoEmEquipamentos != "5")
                            {
                                MostrarMensagemOperacaoInvalida();
                                continue;
                            }
                            else if (operacaoEmEquipamentos == "5")
                            {
                                break;
                            }
                            else
                            {
                                switch (operacaoEmEquipamentos)
                                {
                                    case "1":
                                        RegistrarEquipamento();
                                        break;
                                    case "2":
                                        if (equipamentosCadastrados.Count == 0)
                                        {
                                            Console.WriteLine("\n   Nenhum equipamento cadastrado. Por favor, cadastre um equipamento para visualizar seu registro.");
                                            Console.ReadLine();
                                            break;
                                        }
                                        VisualizarEquipamentosRegistrados();
                                        Console.ReadLine();

                                        break;
                                    case "3":
                                        if (equipamentosCadastrados.Count == 0)
                                        {
                                            Console.WriteLine("\n   Nenhum equipamento cadastrado. Por favor, cadastre um equipamento antes de editar um registro.");
                                            Console.ReadLine();
                                            break;
                                        }
                                        VisualizarEquipamentosRegistrados();

                                        Console.Write("\n   Digite o ID referente ao equipamento que deseja editar: ");
                                        int editarEquipamentoID = Convert.ToInt32(Console.ReadLine());

                                        int editarEquipamentoIndex = equipamentosCadastrados.FindIndex(e => e.StartsWith($"{editarEquipamentoID} |"));


                                        if (editarEquipamentoID < 1 || editarEquipamentoID > equipamentosCadastrados.Count)
                                        {
                                            Console.WriteLine("\n   ID inválido. Por favor, digite um ID válido.");
                                            Console.ReadLine();
                                            break;
                                        }

                                        Console.Write("\n   Digite o nome do equipamento que deseja editar, contendo no mínimo 6 caracteres: ");
                                        string nomeEquipamentoEditado = Console.ReadLine();

                                        nomeEquipamentoEditado = ValidarTamanhoNomeEquipamento(nomeEquipamentoEditado);

                                        string precoEquipamentoStringEditar = RegistrarPrecoEquipamento();

                                        Console.Write("\n   Digite o número de série desse equipamento: ");
                                        string numeroDeSerieEquipamentoEditado = Console.ReadLine();

                                        string dataFabricacaoEquipamentoEditada = RegistrarDataFabricacaoEquipamento();

                                        Console.Write("\n   Digite o fabricante desse equipamento: ");
                                        string fabricanteEquipamentoEdiatdo = Console.ReadLine();

                                        equipamentosCadastrados[editarEquipamentoIndex] = $"{editarEquipamentoID} | {nomeEquipamentoEditado} | {precoEquipamentoStringEditar} | " +
                                        $"{fabricanteEquipamentoEdiatdo} | {dataFabricacaoEquipamentoEditada} | {numeroDeSerieEquipamentoEditado}";

                                        VisualizarEquipamentosRegistrados();

                                        MostrarMensagemEquipamentoEditado();

                                        Console.ReadLine();
                                        break;
                                    case "4":
                                        if (equipamentosCadastrados.Count == 0)
                                        {
                                            Console.WriteLine("\n   Nenhum equipamento cadastrado. Por favor, cadastre um equipamento antes de excluir um.");
                                            Console.ReadLine();
                                            break;
                                        }
                                        VisualizarEquipamentosRegistrados();

                                        Console.Write("\n   Digite o ID referente ao equipamento que deseja excluir: ");
                                        int excluirEquipamentoID = Convert.ToInt32(Console.ReadLine());

                                        int excluirEquipamentoIndex = equipamentosCadastrados.FindIndex(e => e.StartsWith($"{excluirEquipamentoID} |"));

                                        if (excluirEquipamentoIndex == -1)
                                        {
                                            Console.WriteLine("\n   ID inválido. Por favor, digite um ID válido.");
                                            Console.ReadLine();
                                            break;
                                        }

                                        equipamentosCadastrados.RemoveAt(excluirEquipamentoIndex);

                                        VisualizarEquipamentosRegistrados();

                                        MostrarMensagemEquipamentoExcluido();
                                        Console.ReadLine();

                                        break;
                                }
                                continuar = true;
                            }
                        }
                            break;

                    case "2":
                        while (continuar)
                        {
                            string operacaoEmChamados = MostrarMenuControleChamados();

                            if (operacaoEmChamados != "1" && operacaoEmChamados != "2" && operacaoEmChamados != "3" && operacaoEmChamados != "4" && operacaoEmChamados != "5")
                            {
                                MostrarMensagemOperacaoInvalida();
                                continue;
                            }
                            else if (operacaoEmChamados == "5")
                            {
                                break;
                            }
                            else
                            {
                                switch (operacaoEmChamados)
                                {
                                    case "1":
                                        RegistrarChamado();
                                        break;
                                    case "2":
                                        if (equipamentosCadastrados.Count == 0)
                                        {
                                            Console.WriteLine("\n   Nenhum chamado realizado. Por favor, cadastre um chamado para visualizá-lo.");
                                            Console.ReadLine();
                                            break;
                                        }
                                        VisualizarChamadosRegistrados();
                                        Console.ReadLine();
                                        break;
                                    case "3":
                                        if (equipamentosCadastrados.Count == 0)
                                        {
                                            Console.WriteLine("\n   Nenhum equipamento cadastrado. Por favor, cadastre um equipamento antes de editar um chamado.");
                                            Console.ReadLine();
                                            break;
                                        }
                                        if (chamadosCadastrados.Count == 0)
                                        {
                                            Console.WriteLine("\n   Nenhum chamado cadastrado. Por favor, registre um chamado antes de editar um chamado.");
                                            Console.ReadLine();
                                            break;
                                        }
                                        VisualizarChamadosRegistrados();

                                        Console.Write("\n   Digite o ID do chamado que deseja editar: ");
                                        int editarChamado = Convert.ToInt32(Console.ReadLine());

                                        if (chamadosExcluidos.Contains(editarChamado - 1))
                                        {
                                            Console.WriteLine("\n   Este chamado foi excluído e não pode ser editado.");
                                            Console.ReadLine();
                                            break;
                                        }

                                        Console.Write("\n   Digite título do chamado que deseja cadastrar: ");
                                        string tituloChamadoEditado = Console.ReadLine();

                                        Console.Write("\n   Digite a descricao do chamado que deseja cadastrar: ");
                                        string descricaoChamadoEditado = Console.ReadLine();

                                        Console.Write("\n   Digite o equipamento referente a esse chamado: ");
                                        string equipamentoChamadoEditado = Console.ReadLine();

                                        string dataAberturaChamadoEditado = RegistrarDataAberturaChamado();

                                        int diasEmAbertoEdiatdo = CalcularDiasEmAbertoChamado(dataAberturaChamadoEditado);

                                        idChamado++;

                                        equipamentosCadastrados[editarChamado - 1] = $"{editarChamado} | {tituloChamadoEditado} | {equipamentoChamadoEditado} | " +
                                        $"{dataAberturaChamadoEditado} | {diasEmAbertoEdiatdo}| {descricaoChamadoEditado}";

                                        VisualizarChamadosRegistrados();

                                        MostrarMensagemChamadoEditado();

                                        Console.ReadLine();
                                        break;
                                    case "4":
                                        VisualizarChamadosRegistrados();

                                        Console.Write("\n   Digite o ID referente ao chamado que deseja excluir: ");
                                        int excluirChamado = Convert.ToInt32(Console.ReadLine());

                                        chamadosExcluidos.Add(excluirChamado - 1);

                                        equipamentosCadastrados.RemoveAt(excluirChamado - 1);

                                        VisualizarChamadosRegistrados();

                                        MostrarMensagemChamadoExcluido();

                                        Console.ReadLine();
                                        break;
                                }
                                continuar = true;
                            }
                        }
                        break;
                }
                
                Console.Clear();

            } while (continuar) ;
            
        }
        static void RegistrarChamado()
        {
            Console.Clear();
            Console.WriteLine("_________________________________________________________________________________");
            Console.WriteLine("\n                           Controle de Chamados                                ");
            Console.WriteLine("_________________________________________________________________________________");

            Console.Write("\n   Digite título do chamado que deseja cadastrar: ");
            string tituloChamado = Console.ReadLine();

            Console.Write("\n   Digite a descricao do chamado que deseja cadastrar: ");
            string descricaoChamado = Console.ReadLine();

            VisualizarEquipamentosRegistradosEscolha();

            Console.Write("\n   Digite o equipamento, mostrado na tabela acima, referente a esse chamado : ");
            string equipamentoChamado = Console.ReadLine();

            if (equipamentosCadastrados.Count == 0)
            {
                Console.WriteLine("Nenhum equipamento cadastrado.");
            }
            else
            {
                int intEquipamentoChamado = Convert.ToInt32(equipamentoChamado);

                foreach (string equipamento in equipamentosCadastrados)
                {
                    string[] atributosEquipamentos = equipamento.Split("|");

                    int idEquipamento = Convert.ToInt32(atributosEquipamentos[0]);

                    if (idEquipamento == intEquipamentoChamado)
                    {
                        equipamentoChamado = atributosEquipamentos[1].ToString();
                        break;
                    }
                }

            }
            string dataAberturaChamado = RegistrarDataAberturaChamado();

            int diasEmAberto = CalcularDiasEmAbertoChamado(dataAberturaChamado);

            idChamado++;

            string chamado = $"{idChamado} | {tituloChamado} | {equipamentoChamado} | " +
            $"{dataAberturaChamado} | {diasEmAberto} | {descricaoChamado} ";

            string[] chamadoCadastrado = new string[] { chamado };

            chamadosCadastrados.AddRange(chamadoCadastrado);
        }
        static void MostrarMensagemChamadoExcluido()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("   Seu chamado foi excluído com sucesso!");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        static void MostrarMensagemChamadoEditado()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("   Seu chamdo foi editado com sucesso!");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        static void MostrarMensagemEquipamentoExcluido()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("   Seu equipamento foi excluído com sucesso!");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        static void MostrarMensagemEquipamentoEditado()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("   Seu equipamento foi editado com sucesso!");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        static void RegistrarEquipamento()
        {
            Console.Clear();
            Console.WriteLine("_________________________________________________________________________________");
            Console.WriteLine("\n                         Controle de Equipamentos                              ");
            Console.WriteLine("_________________________________________________________________________________");

            Console.Write("\n   Digite o nome do equipamento que deseja cadastrar, contendo no\n   mínimo 6 caracteres: ");
            string nomeEquipamento = Console.ReadLine();

            nomeEquipamento = ValidarTamanhoNomeEquipamento(nomeEquipamento);

            string precoEquipamentoString = RegistrarPrecoEquipamento();

            Console.Write("\n   Digite o número de série desse equipamento: ");
            string numeroDeSerieEquipamento = Console.ReadLine();

            string dataFabricacaoEquipamento = RegistrarDataFabricacaoEquipamento();

            Console.Write("\n   Digite o fabricante desse equipamento: ");
            string fabricanteEquipamento = Console.ReadLine();

            idEquipamento++;

            string equipamento = $"{idEquipamento} | {nomeEquipamento} | {precoEquipamentoString} | " +
            $"{fabricanteEquipamento} | {dataFabricacaoEquipamento} | {numeroDeSerieEquipamento}";

            string[] equipamentoCadastrado = new string[] { equipamento };

            equipamentosCadastrados.AddRange(equipamentoCadastrado);
        }
        static int CalcularDiasEmAbertoChamado(string dataAberturaChamado)
        {
            int diasEmAberto = 0;
            DateTime dataAbertura = DateTime.ParseExact(dataAberturaChamado, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dataHoje = DateTime.Now;
            TimeSpan tempoEmAberto = dataHoje - dataAbertura;

            diasEmAberto = tempoEmAberto.Days;
            return diasEmAberto;
        }
        static string RegistrarDataAberturaChamado()
        {
            string dataAberturaChamado = "";
            continuar = true;
            while (continuar)
            {
                Console.Write("\n   Digite data de abertura do chamado realizado (dd/mm/aaaa): ");
                dataAberturaChamado = Console.ReadLine();

                Regex regex = new Regex(@"^(\d{2})/(\d{2})/(\d{4})$");
                Match match = regex.Match(dataAberturaChamado);

                if (match.Success)
                {
                    int dia = int.Parse(match.Groups[1].Value);
                    int mes = int.Parse(match.Groups[2].Value);
                    int ano = int.Parse(match.Groups[3].Value);

                    DateTime data;
                    bool dataValida = DateTime.TryParseExact(dataAberturaChamado, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data);

                    if (dataValida && data >= DateTime.Today)
                    {
                        continuar = false;
                    }
                    else if (dia >= 1 && dia <= 31 && mes >= 1 && mes <= 12 && ano >= 1900 && ano <= 2023)
                    {
                        continuar = false;
                    }
                    else
                    {
                        MostrarMensagemDataInvalida();
                    }
                }
                else
                {
                    MostrarMensagemDataFormatoInvalido();
                }
            }

            return dataAberturaChamado;
        }
        static void VisualizarChamadosRegistrados()
        {
            Console.Clear();
            Console.WriteLine("________________________________________________________________________________________________________________");
            Console.WriteLine("\n                                         Controle de Chamados                                                 ");
            Console.WriteLine("________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-10}", "ID ", "  TÍTULO ", "  EQUIPAMENTO ", "  DATA DE ABERTURA ", "  DIAS EM ABERTO ");
            Console.WriteLine("________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (string print in chamadosCadastrados)
            {
                string[] atributosChamados = print.Split('|');

                if (print != null)
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-10}", atributosChamados[0], atributosChamados[1], atributosChamados[2], atributosChamados[3], atributosChamados[4]);
                }
            }
        }
        static void VisualizarEquipamentosRegistrados()
        {
            Console.Clear();
            Console.WriteLine("________________________________________________________________________________________________________________");
            Console.WriteLine("\n                                        Controle de Equipamentos                                              ");
            Console.WriteLine("________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-10}", "ID ", "  EQUIPAMENTO  ", "  PREÇO ", "  FABRICANTE ", "  DATA DE FABRICAÇÃO "  );
            Console.WriteLine("________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (string print in equipamentosCadastrados)
            {
                string[] atributosEquipamentos = print.Split('|');

                if (print != null)
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-10}", atributosEquipamentos[0], atributosEquipamentos[1], atributosEquipamentos[2], atributosEquipamentos[3], atributosEquipamentos[4]);
                }
            }
        }
        static void VisualizarEquipamentosRegistradosEscolha()
        {
            Console.WriteLine("________________________________________________________________________________________________________________");
            Console.WriteLine();
            Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-10}", "ID ", "  EQUIPAMENTO  ", "  PREÇO ", "  FABRICANTE ", "  DATA DE FABRICAÇÃO ");
            Console.WriteLine("________________________________________________________________________________________________________________");
            Console.WriteLine();

            foreach (string print in equipamentosCadastrados)
            {
                string[] atributosEquipamentos = print.Split('|');

                if (print != null)
                {
                    Console.WriteLine("{0,-5}|{1,-25}|{2,-25}|{3,-25}|{4,-10}", atributosEquipamentos[0], atributosEquipamentos[1], atributosEquipamentos[2], atributosEquipamentos[3], atributosEquipamentos[4]);
                }
            }
        }
        static string RegistrarDataFabricacaoEquipamento()
        {
            string dataFabricacaoEquipamento = "";
            continuar = true;
            while (continuar)
            {
                Console.Write("\n   Digite o a data de fabricação desse equipamento (dd/mm/aaaa): ");
                dataFabricacaoEquipamento = Console.ReadLine();

                Regex regex = new Regex(@"^(\d{2})/(\d{2})/(\d{4})$");
                Match match = regex.Match(dataFabricacaoEquipamento);

                if (match.Success)
                {
                    int dia = int.Parse(match.Groups[1].Value);
                    int mes = int.Parse(match.Groups[2].Value);
                    int ano = int.Parse(match.Groups[3].Value);

                    DateTime data;
                    bool dataValida = DateTime.TryParseExact(dataFabricacaoEquipamento, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data);

                    if (dataValida && data >= DateTime.Today)
                    {
                        continuar = false;
                    }
                    else if (dia >= 1 && dia <= 31 && mes >= 1 && mes <= 12 && ano >= 1900 && ano <= 2023)
                    {
                        continuar = false;
                    }
                    else
                    {
                        MostrarMensagemDataInvalida();
                    }
                }
                else
                {
                    MostrarMensagemDataFormatoInvalido();
                }
            }

            return dataFabricacaoEquipamento;
        }
        static string RegistrarPrecoEquipamento()
        {
            string precoEquipamentoString = ""; 
            double precoEquipamento;
            bool precoValido = false;
            while (!precoValido)
            {
                Console.Write("\n   Digite o preço de aquisição desse equipamento (R$): ");
                precoEquipamentoString = Console.ReadLine();
               
                if (!double.TryParse(precoEquipamentoString, out precoEquipamento))
                {
                    MostrarMensagemApenasNumeros();
                }
                else if (precoEquipamento <= 0)
                {
                    MostrarMensagemValorInvalido();
                }
                else
                {
                    precoValido = true;
                }
               
            }

            return precoEquipamentoString;
        }
        private static void MostrarMensagemApenasNumeros()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n   Valor inválido. Digite apenas numeros. Tente novamente. ");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        static string ValidarTamanhoNomeEquipamento(string nomeEquipamento)
        {
            while (nomeEquipamento.Length < 6)
            {
                MostrarMensagemOperacaoInvalida();
                Console.Write("\n   O nome deve ter no mínimo 6 caracteres. \n   Digite o nome do equipamento que deseja cadastrar: ");
                nomeEquipamento = Console.ReadLine();
                Console.WriteLine();
            }

            return nomeEquipamento;
        }
        static void MostrarMensagemValorInvalido()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n   O valor fornecido é inválido. O valor deve ser maior que zero. Tente novamente.");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        static void MostrarMensagemDataInvalida()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("   Data inválida. A data de fabricação deve ser entre 1900 e a data atual. Tente novamente.");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        static void MostrarMensagemDataFormatoInvalido()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("   Data inválida. O formato esperado é dd/mm/aaaa. Tente novamente.");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        static void MostrarMensagemOperacaoInvalida()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("\n   Operação inválida, tente novamente. ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.ReadLine();
        }
        static string MostarMenuControleEquipamentos()
        {
            Console.Clear();
            Console.WriteLine("_________________________________________________________________________________");
            Console.WriteLine("\n                         Controle de Equipamentos                              ");
            Console.WriteLine("_________________________________________________________________________________");
            Console.WriteLine("\n   Digite 1 para inserir um novo equipamento. ");
            Console.WriteLine("\n   Digite 2 para visualizar seus equipamentos registrados. ");
            Console.WriteLine("\n   Digite 3 para editar algum de seus equipamentos registrados. ");
            Console.WriteLine("\n   Digite 4 para excluir algum de seus equipamentos registrados. ");
            Console.WriteLine("\n   Digite 5 para voltar ao menu inicial. ");
            Console.WriteLine("_________________________________________________________________________________");
            Console.Write("\n   ");
            string operacaoEmEquipamentos = Console.ReadLine();
            return operacaoEmEquipamentos;
        }
        static string MostrarMenuControleChamados()
        {
            Console.Clear();
            Console.WriteLine("_________________________________________________________________________________");
            Console.WriteLine("\n                         Controle de Chamados                                  ");
            Console.WriteLine("_________________________________________________________________________________");
            Console.WriteLine("\n   Digite 1 para registrar um chamado. ");
            Console.WriteLine("\n   Digite 2 para visualizar seus chamados registrados. ");
            Console.WriteLine("\n   Digite 3 para editar algum de seus chamados. ");
            Console.WriteLine("\n   Digite 4 para excluir algum de seus chamados. ");
            Console.WriteLine("\n   Digite 5 para voltar ao menu inicial.  ");
            Console.WriteLine("_________________________________________________________________________________");
            Console.Write("\n   ");
            string operacaoEmChamados = Console.ReadLine();
            return operacaoEmChamados;
        }
        static string ExecutarMenuInicial()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Clear();
            Console.WriteLine("_________________________________________________________________________________");
            Console.WriteLine("\n                         Gestão de Equipamentos                                ");
            Console.WriteLine("_________________________________________________________________________________");
            Console.WriteLine("\n   Digite 1 para realizar o Controle de Equipamentos. ");
            Console.WriteLine("\n   Digite 2 para realizar o Controle de Chamados. ");
            Console.WriteLine("\n   Digite S para sair. ");
            Console.WriteLine("_________________________________________________________________________________");
            Console.Write("\n   ");
            string operacao = Console.ReadLine().ToUpper();

            while (operacao != "1" && operacao != "2" && operacao != "S")
            {                
                if (operacao != "1" && operacao != "2" && operacao != "S")
                {
                    MostrarMensagemOperacaoInvalida();
                    break; 
                }
            }
            return operacao;
        }
    }
}