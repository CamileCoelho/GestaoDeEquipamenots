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
                                ApresentarMensagemEmVermelho("\n   Operação inválida, tente novamente. ");
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
                                            ApresentarMensagemEmVermelho("\n   Nenhum equipamento cadastrado. Por favor, cadastre um equipamento para visualizar seu registro.");
                                            break;
                                        }
                                        VisualizarEquipamentosRegistrados();
                                        Console.ReadLine();

                                        break;
                                    case "3":
                                        if (equipamentosCadastrados.Count == 0)
                                        {
                                            ApresentarMensagemEmVermelho("\n   Nenhum equipamento cadastrado. Por favor, cadastre um equipamento antes de editar um registro.");
                                            break;
                                        }
                                        VisualizarEquipamentosRegistrados();

                                        Console.Write("\n   Digite o ID referente ao equipamento que deseja editar: ");
                                        int editarEquipamentoID = Convert.ToInt32(Console.ReadLine());

                                        int editarEquipamentoIndex = equipamentosCadastrados.FindIndex(e => e.StartsWith($"{editarEquipamentoID} |"));


                                        if (editarEquipamentoID < 1 || editarEquipamentoID > equipamentosCadastrados.Count)
                                        {
                                            ApresentarMensagemEmVermelho("\n   ID inválido. Por favor, digite um ID válido.");
                                            return;
                                        }

                                        Console.Write("\n   Digite o nome do equipamento editado, contendo no mínimo 6 caracteres: ");
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

                                        ApresentarMensagemEmVerde("\n   Seu equipamento foi editado com sucesso!");
                                        break;
                                    case "4":
                                        if (equipamentosCadastrados.Count == 0)
                                        {
                                            ApresentarMensagemEmVermelho("\n   Você nao possuí nenhum equipamento cadastrado. Por favor, cadastre um equipamento antes de excluir um.");
                                            break;
                                        }
                                        VisualizarEquipamentosRegistrados();

                                        Console.Write("\n   Digite o ID referente ao equipamento que deseja excluir: ");
                                        int excluirEquipamentoID = Convert.ToInt32(Console.ReadLine());

                                        int excluirEquipamentoIndex = equipamentosCadastrados.FindIndex(e => e.StartsWith($"{excluirEquipamentoID} |"));

                                        if (excluirEquipamentoIndex == -1)
                                        {
                                            ApresentarMensagemEmVermelho("\n   ID inválido. Por favor, digite um ID válido.");
                                            break;
                                        }

                                        equipamentosCadastrados.RemoveAt(excluirEquipamentoIndex);

                                        for (int i = excluirEquipamentoIndex; i < equipamentosCadastrados.Count; i++)
                                        {
                                            string equipamento = equipamentosCadastrados[i];
                                            string[] dadosEquipamento = equipamento.Split('|');
                                            int novoID = i + 1;
                                            equipamentosCadastrados[i] = $"{novoID} | {dadosEquipamento[1].Trim()} | {dadosEquipamento[2].Trim()} | {dadosEquipamento[3].Trim()} | {dadosEquipamento[4].Trim()} | {dadosEquipamento[5].Trim()}";
                                        }

                                        VisualizarEquipamentosRegistrados();

                                        ApresentarMensagemEmVerde("\n   Seu equipamento foi excluído com sucesso, e sua lista está atualizada!");
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
                                ApresentarMensagemEmVermelho("\n   Operação inválida, tente novamente. ");
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
                                        if (chamadosCadastrados.Count == 0)
                                        {
                                            ApresentarMensagemEmVermelho("\n   Nenhum chamado realizado. Por favor, cadastre um chamado para visualizá-lo.");
                                            break;
                                        }
                                        VisualizarChamadosRegistrados();
                                        Console.ReadLine();
                                        break;
                                    case "3":
                                        if (equipamentosCadastrados.Count == 0)
                                        {
                                            ApresentarMensagemEmVermelho("\n   Nenhum equipamento cadastrado. Por favor, cadastre um equipamento antes de editar um chamado.");
                                            break;
                                        }
                                        else if (chamadosCadastrados.Count == 0)
                                        {
                                            ApresentarMensagemEmVermelho("\n   Nenhum chamado cadastrado. Por favor, registre um chamado antes de editar um chamado.");
                                            break;
                                        }
                                        VisualizarChamadosRegistrados();

                                        Console.Write("\n   Digite o ID do chamado que deseja editar: ");
                                        int editarChamado = Convert.ToInt32(Console.ReadLine());

                                        if (chamadosExcluidos.Contains(editarChamado - 1))
                                        {
                                            ApresentarMensagemEmVermelho("\n   Este chamado foi excluído e não pode ser editado.");
                                            break;
                                        }

                                        Console.Write("\n   Digite o título do chamado: ");
                                        string tituloChamadoEditado = Console.ReadLine();

                                        Console.Write("\n   Digite a descricao do chamado: ");
                                        string descricaoChamadoEditado = Console.ReadLine();

                                        VisualizarEquipamentosRegistradosEscolha();

                                        Console.Write("\n   Digite o equipamento referente a esse chamado: ");
                                        string equipamentoChamadoEditado = Console.ReadLine();

                                        string dataAberturaChamadoEditado = RegistrarDataAberturaChamado();

                                        int diasEmAbertoEdiatdo = CalcularDiasEmAbertoChamado(dataAberturaChamadoEditado);

                                        idChamado++;

                                        string chamadoEditado = equipamentosCadastrados[editarChamado - 1];

                                        chamadoEditado = $"{editarChamado} | {tituloChamadoEditado} | {equipamentoChamadoEditado} | " +
                                        $"{dataAberturaChamadoEditado} | {diasEmAbertoEdiatdo}| {descricaoChamadoEditado}";

                                        string[] chamadoCadastrado = new string[] { chamadoEditado };
                                        chamadosCadastrados.AddRange(chamadoCadastrado);

                                        VisualizarChamadosRegistrados();

                                        ApresentarMensagemEmVerde("\n   Seu chamado foi editado com sucesso!");
                                        break;
                                    case "4":
                                        if (chamadosCadastrados.Count == 0)
                                        {
                                            ApresentarMensagemEmVermelho("\n   Nenhum chamado cadastrado. Por favor, registre um chamado antes de excluír um chamado.");
                                            break;
                                        }
                                        VisualizarChamadosRegistrados();

                                        Console.Write("\n   Digite o ID referente ao chamado que deseja excluir: ");
                                        int excluirChamadoID = Convert.ToInt32(Console.ReadLine());
                                        int excluirChamadoIndex = chamadosCadastrados.FindIndex(e => e.StartsWith($"{excluirChamadoID} |"));

                                        if (excluirChamadoIndex == -1)
                                        {
                                            ApresentarMensagemEmVermelho("\n   ID inválido. Por favor, digite um ID válido.");
                                            break;
                                        }
                                        chamadosExcluidos.Add(excluirChamadoID - 1);

                                        equipamentosCadastrados.RemoveAt(excluirChamadoID - 1);

                                        VisualizarChamadosRegistrados();

                                        ApresentarMensagemEmVerde("\n   Seu chamado foi excluído com sucesso!");
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

            if (equipamentosCadastrados.Count == 0)
            {
                ApresentarMensagemEmVermelho("\n   Nenhum equipamento cadastrado. Por favor, cadastre um equipamento antes\n   de realidar um chamado.");
                return;
            }

            Console.Write("\n   Digite título do chamado que deseja cadastrar: ");
            string tituloChamado = Console.ReadLine();

            Console.Write("\n   Digite a descricao do chamado que deseja cadastrar: ");
            string descricaoChamado = Console.ReadLine();

            while (true)
            {
                VisualizarEquipamentosRegistradosEscolha();

                Console.Write("\n   Digite o ID de um dos equipamentos, mostrado na tabela acima, referente a esse chamado : ");
                string equipamentoChamado = Console.ReadLine();
                bool equipamentoEncontrado = false;

                int intEquipamentoChamado = Convert.ToInt32(equipamentoChamado);

                foreach (string equipamento in equipamentosCadastrados)
                {
                    string[] atributosEquipamentos = equipamento.Split("|");

                    int idEquipamento = Convert.ToInt32(atributosEquipamentos[0]);

                    if (idEquipamento == intEquipamentoChamado)
                    {
                        equipamentoChamado = atributosEquipamentos[1].ToString();
                        equipamentoEncontrado = true;
                        break;
                    }
                }
                if (equipamentoEncontrado)
                {
                    string dataAberturaChamado = RegistrarDataAberturaChamado();
                    int diasEmAberto = CalcularDiasEmAbertoChamado(dataAberturaChamado);

                    idChamado++;

                    string chamado = $"{idChamado} | {tituloChamado} | {equipamentoChamado} | " +
                    $"{dataAberturaChamado} | {diasEmAberto} | {descricaoChamado} ";

                    string[] chamadoCadastrado = new string[] { chamado };
                    chamadosCadastrados.AddRange(chamadoCadastrado);

                    break;
                }
                else
                {
                    ApresentarMensagemEmVermelho("\n   ID de equipamento inválido. Por favor, digite um ID válido. ");
                }
            }
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

            int ultimoIdEquipamento = 0;
            if (equipamentosCadastrados.Count > 0)
            {
                string ultimoEquipamento = equipamentosCadastrados.Last();
                ultimoIdEquipamento = Int32.Parse(ultimoEquipamento.Split('|')[0].Trim());
            }

            idEquipamento = ultimoIdEquipamento + 1;

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
                        ApresentarMensagemEmVermelho("\n   Data inválida. A data de fabricação deve ser entre 1900 e a data atual. Tente novamente.\"   Valor inválido. Digite apenas numeros. Tente novamente. ");
                    }
                }
                else
                {
                    ApresentarMensagemEmVermelho("\n   Data inválida. O formato esperado é dd/mm/aaaa. Tente novamente.");
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
                        ApresentarMensagemEmVermelho("\n   Data inválida. A data de fabricação deve ser uma data existente. Tente novamente. ");
                    }
                }
                else
                {
                    ApresentarMensagemEmVermelho("\n   Data inválida. O formato esperado é dd/mm/aaaa. Tente novamente.");
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
                    ApresentarMensagemEmVermelho("\n   Valor inválido. Digite apenas numeros. Tente novamente. ");
                }
                else if (precoEquipamento <= 0)
                {
                    ApresentarMensagemEmVermelho("\n   O valor fornecido é inválido. O valor deve ser maior que zero. Tente novamente.");
                }
                else
                {
                    precoValido = true;
                }
               
            }

            return precoEquipamentoString;
        }
        static string ValidarTamanhoNomeEquipamento(string nomeEquipamento)
        {
            while (nomeEquipamento.Length < 6)
            {
                ApresentarMensagemEmVermelho("\n   Operação inválida, tente novamente. ");
                Console.Write("\n   O nome deve ter no mínimo 6 caracteres. \n   Digite o nome do equipamento que deseja cadastrar: ");
                nomeEquipamento = Console.ReadLine();
            }

            return nomeEquipamento;
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
                    ApresentarMensagemEmVermelho("\n   Operação inválida, tente novamente. ");
                    break; 
                }
            }
            return operacao;
        }
        static void ApresentarMensagemEmVermelho(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(mensagem);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.ReadLine();
        }
        static void ApresentarMensagemEmVerde(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(mensagem);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.ReadLine();
        }
    }
}