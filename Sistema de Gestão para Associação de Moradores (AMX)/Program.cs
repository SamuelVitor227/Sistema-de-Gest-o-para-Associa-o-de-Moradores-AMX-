using System;
using System.Collections.Generic;
using System.IO; // Adicionado para Path e File
using System.Linq;

namespace AssociacaoAMX
{
    class Program
    {
        private static Associacao amx;

        static void Main(string[] args)
        {
            amx = new Associacao();
            CarregarHabilidades();

            int opcao = 0;

            while (opcao != 18)
            {
                ExibirMenu();

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida. Digite um número entre 1 e 18.");
                    Console.ReadKey();
                    continue;
                }

                ProcessarOpcao(opcao);

                if (opcao != 18)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        static void ExibirMenu()
        {
            Console.Clear(); // Limpa a tela para cada exibição de menu
            Console.WriteLine("================= Associação de Moradores da Xulamblândia (AMX) =================");
            Console.WriteLine("1  - Cadastrar Associado (Prestador ou Produtor)");
            Console.WriteLine("2  - Cadastrar Habilidade (com Pontos de Dificuldade)");
            Console.WriteLine("3  - Criar Demanda");
            Console.WriteLine("4  - Atribuir Habilidade para Associado");
            Console.WriteLine("5  - Atribuir Habilidade para Demanda");
            Console.WriteLine("6  - Definir Atendente para Demanda");
            Console.WriteLine("7  - Finalizar Demanda");
            Console.WriteLine("8  - Listar Associados");
            Console.WriteLine("9  - Listar Habilidades");
            Console.WriteLine("10 - Listar Demandas");
            Console.WriteLine("11 - Localizar Associado por CPF");
            Console.WriteLine("12 - Localizar Habilidade por ID");
            Console.WriteLine("13 - Localizar Demanda por ID");
            Console.WriteLine("14 - Registrar Produto para Produtor");
            Console.WriteLine("15 - Listar Produtos");
            Console.WriteLine("16 - Gerar Relatórios");
            Console.WriteLine("17 - GERAR CARGA DE DADOS DE TESTE");
            Console.WriteLine("18 - Sair");
            Console.Write("\nEscolha uma opção: ");
        }

        static void ProcessarOpcao(int opcao)
        {
            try
            {
                switch (opcao)
                {
                    case 1:
                        Console.Write("Digite o nome do associado: ");
                        string nome = Console.ReadLine();
                        Console.Write("Digite o CPF do associado: ");
                        string cpf = Console.ReadLine();
                        CadastrarAssociado(nome, cpf);
                        break;
                    case 2:
                        Console.Write("Digite a descrição da habilidade: ");
                        string descHab = Console.ReadLine();
                        CadastrarHabilidade(descHab);
                        break;
                    case 3:
                        Console.Write("Digite o CPF do criador da demanda: ");
                        string cpfCriador = Console.ReadLine();
                        Console.Write("Digite a descrição da demanda: ");
                        string desc = Console.ReadLine();
                        int tempoPrevisto = LerTempoPrevistoValido(); // Retorna em frações de 30min
                        Console.Write("Digite o prazo (em dias): ");
                        if (!int.TryParse(Console.ReadLine(), out int prazo) || prazo <= 0)
                        {
                            throw new FormatException("Prazo inválido. Deve ser um número inteiro positivo.");
                        }
                        CadastrarDemanda(cpfCriador, desc, tempoPrevisto, prazo);
                        break;
                    case 4:
                        Console.Write("Digite o CPF do associado (Prestador): ");
                        string cpfAssoc = Console.ReadLine();
                        Console.Write("Digite o ID da habilidade: ");
                        if (!int.TryParse(Console.ReadLine(), out int idHabAssoc))
                        {
                            throw new FormatException("ID da habilidade inválido.");
                        }
                        AtribuirHabilidadeAssociado(cpfAssoc, idHabAssoc);
                        break;
                    case 5:
                        Console.Write("Digite o ID da demanda: ");
                        if (!int.TryParse(Console.ReadLine(), out int idDemHab))
                        {
                            throw new FormatException("ID da demanda inválido.");
                        }
                        Console.Write("Digite o ID da habilidade: ");
                        if (!int.TryParse(Console.ReadLine(), out int idHabDem))
                        {
                            throw new FormatException("ID da habilidade inválido.");
                        }
                        AtribuirHabilidadeDemanda(idDemHab, idHabDem);
                        break;
                    case 6:
                        Console.Write("Digite o ID da demanda: ");
                        if (!int.TryParse(Console.ReadLine(), out int idDemAten))
                        {
                            throw new FormatException("ID da demanda inválido.");
                        }
                        DefinirAtendente(idDemAten);
                        break;
                    case 7:
                        Console.Write("Digite o ID da demanda a ser finalizada: ");
                        if (!int.TryParse(Console.ReadLine(), out int idDemFin))
                        {
                            throw new FormatException("ID da demanda inválido.");
                        }
                        FinalizarDemanda(idDemFin);
                        break;
                    case 8: ListarAssociados(); break;
                    case 9: ListarHabilidades(); break;
                    case 10: ListarDemandas(); break;
                    case 11:
                        Console.Write("Digite o CPF do associado: ");
                        Associado associado = amx.LocalizarAssociado(Console.ReadLine());
                        if (associado == null)
                            throw new KeyNotFoundException("Associado não encontrado.");
                        Console.WriteLine(associado.ToString());
                        break;
                    case 12:
                        Console.Write("Digite o ID da habilidade: ");
                        if (int.TryParse(Console.ReadLine(), out int idHabLoc))
                        {
                            Habilidade habilidade = amx.LocalizarHabilidade(idHabLoc);
                            if (habilidade == null)
                                throw new KeyNotFoundException("Habilidade não encontrada.");
                            Console.WriteLine(habilidade.ToString());
                        }
                        else { throw new FormatException("ID inválido."); }
                        break;
                    case 13:
                        Console.Write("Digite o ID da demanda: ");
                        if (int.TryParse(Console.ReadLine(), out int idDemLoc))
                        {
                            Demanda demanda = amx.LocalizarDemanda(idDemLoc);
                            if (demanda == null)
                                throw new KeyNotFoundException("Demanda não encontrada.");
                            Console.WriteLine(demanda.ToString());
                        }
                        else { throw new FormatException("ID inválido."); }
                        break;
                    case 14: RegistrarProduto(); break;
                    case 15: ListarProdutos(); break;
                    case 16:
                        ProcessarMenuRelatorios();
                        break;
                    case 17:
                        GerarCargaDeDadosCompleta();
                        break;
                    case 18: Console.WriteLine("Saindo do sistema..."); break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            catch (Associado.CreditosInsuficientesException ex) { Console.WriteLine($"Erro de Crédito: {ex.Message}"); }
            catch (InvalidOperationException ex) { Console.WriteLine($"Erro de Operação: {ex.Message}"); }
            catch (KeyNotFoundException ex) { Console.WriteLine($"Erro de Busca: {ex.Message}"); }
            catch (FormatException ex) { Console.WriteLine($"Erro de Formato: {ex.Message}"); }
            catch (ArgumentException ex) { Console.WriteLine($"Erro de Argumento: {ex.Message}"); } // Adicionado para erros de validação
            catch (Exception ex) { Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}"); }
        }

        private static void GerarCargaDeDadosCompleta()
        {
            Console.WriteLine("\nIniciando geração de carga de dados completa...");

            Console.WriteLine("Passo 1: Gerando associados e habilidades...");
            GerarAssociados.Executar(amx);
            Console.WriteLine("Associados e habilidades de teste gerados.");

            Console.WriteLine("Passo 2: Gerando as demandas e tentando atribuí-las imediatamente...");
            GeradorDemandas.Executar(amx); // GeradorDemandas já tenta atribuir e finalizar
            Console.WriteLine("Demandas de teste geradas e atribuídas/finalizadas conforme possível.");

            // A lógica de atribuição e finalização que estava aqui no loop foreach foi movida para GeradorDemandas.Executar
            // e para o método RebalanceCreditsForAssociado dentro de Associacao.
            // Para simular a economia, podemos tentar rebalancear créditos para alguns associados após a carga inicial.
            Console.WriteLine("\nPasso 3: Verificando e rebalanceando créditos para associados com saldo negativo...");
            foreach (var associado in amx.GetAssociados().Values.ToList()) // ToList() para evitar modificação durante iteração
            {
                if (associado.CalcularCreditos() < -10)
                {
                    Console.WriteLine($"- Tentando rebalancear créditos para {associado.GetNome()} (saldo: {associado.CalcularCreditos()})");
                    amx.ProcurarAssociadoParaDemanda(associado);
                }
            }
            Console.WriteLine("Verificação e rebalanceamento de créditos concluído.");

            Console.WriteLine("\nCarga de dados de teste completa gerada com sucesso!");
        }

        static void ExibirMenuRelatorios()
        {
            Console.Clear();
            Console.WriteLine("=========================== MENU DE RELATÓRIOS ===========================");
            Console.WriteLine("1 - Associados que não podem registrar demanda neste momento");
            Console.WriteLine("2 - Associados hábeis para uma demanda, ordenados por saldo");
            Console.WriteLine("3 - Os 10 associados com maior saldo de créditos");
            Console.WriteLine("4 - Demandas não alocadas para nenhum prestador");
            Console.WriteLine("5 - Demandas que podem ser atendidas por um associado específico (Prestador)"); // Alterado para refletir o tipo
            Console.WriteLine("6 - As 10 demandas que dão a maior quantidade de créditos");
            Console.WriteLine("7 - Média da diferença de tempo entre atendimento e prazo");
            Console.WriteLine("8 - Demandas solucionáveis por um conjunto de habilidades");
            Console.WriteLine("0 - Voltar ao Menu Principal");
            Console.Write("\nEscolha uma opção de relatório: ");
        }

        static void ProcessarMenuRelatorios()
        {
            int opcaoRelatorio = -1;
            while (opcaoRelatorio != 0)
            {
                ExibirMenuRelatorios();
                if (!int.TryParse(Console.ReadLine(), out opcaoRelatorio))
                {
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    Console.ReadKey();
                    continue;
                }

                try
                {
                    switch (opcaoRelatorio)
                    {
                        case 1:
                            List<Associado> associadosInaptos = amx.ListarAssociadosSemCreditoParaDemanda();
                            Console.WriteLine("\n--- Associados que não podem registrar demanda ---");
                            if (!associadosInaptos.Any())
                            {
                                Console.WriteLine("Todos os associados podem registrar demandas.");
                            }
                            else
                            {
                                foreach (Associado a in associadosInaptos)
                                {
                                    Console.WriteLine($"- {a.GetNome()} (CPF: {a.GetCPF()}, Saldo: {a.CalcularCreditos()})");
                                }
                            }
                            break;
                        case 2:
                            Console.Write("Digite o ID da demanda para encontrar associados hábeis: ");
                            if (!int.TryParse(Console.ReadLine(), out int demandaId))
                            {
                                throw new FormatException("ID da demanda inválido.");
                            }
                            Demanda demandaLocalizada = amx.LocalizarDemanda(demandaId);
                            if (demandaLocalizada == null)
                            {
                                throw new KeyNotFoundException("Demanda não encontrada.");
                            }
                            Console.WriteLine("\n--- Associados Hábeis para a Demanda ---");
                            Console.WriteLine(amx.AssociadosHabeis(demandaLocalizada)); // Usa o método da associação
                            break;
                        case 3:
                            List<Associado> top10Associados = amx.ListarTop10AssociadosMaiorSaldo();
                            Console.WriteLine("\n--- Top 10 Associados com Maior Saldo de Créditos ---");
                            if (!top10Associados.Any())
                            {
                                Console.WriteLine("Não há associados para listar.");
                            }
                            else
                            {
                                foreach (Associado a in top10Associados)
                                {
                                    Console.WriteLine($"- {a.GetNome()} (Saldo: {a.CalcularCreditos()})");
                                }
                            }
                            break;
                        case 4:
                            List<Demanda> demandasNaoAlocadas = amx.ListarDemandasNaoAlocadas();
                            Console.WriteLine("\n--- Demandas ainda não alocadas ---");
                            if (!demandasNaoAlocadas.Any())
                            {
                                Console.WriteLine("Todas as demandas estão alocadas.");
                            }
                            else
                            {
                                foreach (Demanda d in demandasNaoAlocadas)
                                {
                                    Console.WriteLine($"- ID: {d.GetHashCode()} | Descrição: {d.GetDescricao()}");
                                }
                            }
                            break;
                        case 5:
                            Console.Write("Digite o CPF do associado (Prestador) para ver demandas atendíveis: ");
                            string cpfAssociadoParaDemanda = Console.ReadLine();
                            Associado associadoConsulta = amx.LocalizarAssociado(cpfAssociadoParaDemanda);

                            if (associadoConsulta == null)
                                throw new KeyNotFoundException("Associado não encontrado.");

                            if (!(associadoConsulta is IHabil prestadorConsulta))
                                throw new InvalidOperationException("O associado informado não é um Prestador.");

                            List<Demanda> todasAsDemandas = amx.GetDemandas().Values.ToList();
                            var demandasAtendiveis = todasAsDemandas
                                .Where(d => d.GetAtendente() == null && prestadorConsulta.TemHabilidadesNecessarias(d))
                                .ToList();

                            Console.WriteLine($"\n--- Demandas que o Prestador {((Associado)prestadorConsulta).GetNome()} pode atender ---");
                            if (!demandasAtendiveis.Any())
                            {
                                Console.WriteLine("Este prestador não pode atender a nenhuma demanda em aberto.");
                            }
                            else
                            {
                                foreach (Demanda d in demandasAtendiveis)
                                {
                                    Console.WriteLine($"- ID: {d.GetHashCode()} | Descrição: {d.GetDescricao()} | Tempo Previsto: {d.GetTempoPrevisto()} (30min fracs.)");
                                }
                            }
                            break;
                        case 6:
                            // Top 10 demandas que dão mais créditos (já finalizadas)
                            var top10Demandas = amx.GetDemandas().Values
                                .Where(d => d.FoiFinalizada())
                                .OrderByDescending(d => d.CalcularCreditos())
                                .Take(10)
                                .ToList();

                            Console.WriteLine("\n--- Top 10 Demandas com Maior Quantidade de Créditos (Finalizadas) ---");
                            if (!top10Demandas.Any())
                            {
                                Console.WriteLine("Não há demandas finalizadas para listar.");
                            }
                            else
                            {
                                foreach (Demanda d in top10Demandas)
                                {
                                    Console.WriteLine($"- ID: {d.GetHashCode()} | Créditos: {d.CalcularCreditos()} | Descrição: {d.GetDescricao()}");
                                }
                            }
                            break;
                        case 7:
                            double mediaDias = amx.MediaAtendimentoPrazos();
                            Console.WriteLine($"\nValor negativo significa que as demandas estão sendo atendidas X dias antes do prazo finalizar.");
                            Console.WriteLine($"A média de diferença dos atendimentos para os prazos é de {mediaDias:F2} dias."); // Formatado para 2 casas decimais
                            break;
                        case 8:
                            Console.Write("Digite os IDs das habilidades (separados por vírgula, ex: 1,5,7): ");
                            string idsInput = Console.ReadLine();
                            List<int> idsHabilidadesParaBusca = new List<int>();
                            try
                            {
                                idsHabilidadesParaBusca = idsInput.Split(',')
                                                                .Select(s => int.Parse(s.Trim()))
                                                                .ToList();
                            }
                            catch (FormatException)
                            {
                                throw new FormatException("Entrada de IDs de habilidades inválida. Use números separados por vírgula.");
                            }

                            // Converter IDs de habilidades para objetos Habilidade para o método
                            List<Habilidade> habilidadesParaBusca = new List<Habilidade>();
                            foreach (int id in idsHabilidadesParaBusca)
                            {
                                Habilidade h = amx.LocalizarHabilidade(id);
                                if (h != null)
                                {
                                    habilidadesParaBusca.Add(h);
                                }
                                else
                                {
                                    Console.WriteLine($"Atenção: Habilidade com ID {id} não encontrada e será ignorada.");
                                }
                            }

                            if (!habilidadesParaBusca.Any())
                            {
                                Console.WriteLine("Nenhuma habilidade válida foi fornecida para a busca.");
                                break;
                            }

                            // Filtrar demandas que exigem TODAS as habilidades fornecidas
                            var demandasSolucionaveis = amx.GetDemandas().Values
                                .Where(d => d.GetHabilidadesNecessarias().Values.All(dh => habilidadesParaBusca.Any(h => h.Equals(dh))))
                                .ToList();

                            Console.WriteLine("\n--- Demandas que podem ser solucionadas com as habilidades fornecidas ---");
                            if (!demandasSolucionaveis.Any())
                            {
                                Console.WriteLine("Nenhuma demanda encontrada que exija EXATAMENTE este conjunto de habilidades.");
                                // Poderíamos adicionar lógica para "pelo menos" este conjunto de habilidades, se fosse um requisito
                            }
                            else
                            {
                                foreach (Demanda d in demandasSolucionaveis)
                                {
                                    Console.WriteLine($"- ID: {d.GetHashCode()} | Descrição: {d.GetDescricao()}");
                                }
                            }
                            break;
                        case 0:
                            Console.WriteLine("\nRetornando ao menu principal...");
                            break;
                        default:
                            Console.WriteLine("Opção de relatório inválida.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nOcorreu um erro ao gerar o relatório: {ex.Message}");
                }

                if (opcaoRelatorio != 0)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        static void CadastrarAssociado(string nome, string cpf)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do associado não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("O CPF fornecido é inválido.");

            if (amx.LocalizarAssociado(cpf) != null)
                throw new InvalidOperationException("Já existe um associado com este CPF.");

            char tipoAssociadoChar = ' ';
            while (tipoAssociadoChar != 'p' && tipoAssociadoChar != 'r')
            {
                Console.Write("Digite o tipo do associado (P para Prestador, R para Produtor): ");
                tipoAssociadoChar = Console.ReadLine().Trim().ToLower().FirstOrDefault();
                if (tipoAssociadoChar != 'p' && tipoAssociadoChar != 'r')
                    Console.WriteLine("Tipo inválido. Por favor, digite 'P' ou 'R'.");
            }

            Associado novoAssociado;
            if (tipoAssociadoChar == 'r') // Produtor
            {
                novoAssociado = new Produtor(nome, cpf);
            }
            else // Prestador
            {
                novoAssociado = new Prestador(nome, cpf);
            }

            amx.CadastrarAssociado(novoAssociado);
            Console.WriteLine($"Associado '{novoAssociado.GetNome()}' ({novoAssociado.GetType().Name}) cadastrado com sucesso!");
        }

        static void CadastrarHabilidade(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("A descrição da habilidade não pode ser vazia.");

            if (amx.GetHabilidades().Any(h => h.Value.GetHabilidade().Equals(descricao, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Esta habilidade já está cadastrada.");

            int pontosDificuldade = 0; // Renomeado para clareza
            while (pontosDificuldade < 1 || pontosDificuldade > 3)
            {
                Console.Write("Digite os pontos de dificuldade da habilidade (1, 2 ou 3): ");
                if (!int.TryParse(Console.ReadLine(), out pontosDificuldade) || pontosDificuldade < 1 || pontosDificuldade > 3)
                {
                    Console.WriteLine("Valor inválido. Insira 1, 2 ou 3.");
                    pontosDificuldade = 0;
                }
            }

            Habilidade novaHabilidade = new Habilidade(descricao, pontosDificuldade);
            amx.CadastrarHabilidade(novaHabilidade);
            Console.WriteLine($"Habilidade '{descricao}' com {pontosDificuldade} pontos de dificuldade cadastrada! ID: {novaHabilidade.GetHashCode()}");
        }

        private static void RegistrarProduto()
        {
            Console.Write("Digite o CPF do produtor: ");
            string cpf = Console.ReadLine();
            Associado assoc = amx.LocalizarAssociado(cpf);

            if (assoc == null) throw new KeyNotFoundException("Associado não encontrado.");

            // Usa a interface para verificar se é um produtor
            if (!(assoc is IProdutor produtor))
                throw new InvalidOperationException("Operação não permitida. O associado informado não é um Produtor.");

            char continuar = 's';
            while (continuar == 's')
            {
                Console.Write("Digite a descrição do produto: ");
                string descricao = Console.ReadLine();

                int pontos = 0;
                List<int> pontosValidos = new List<int> { 1, 2, 3, 5 };
                while (!pontosValidos.Contains(pontos))
                {
                    Console.Write("Digite os pontos do produto (1, 2, 3 ou 5): ");
                    if (!int.TryParse(Console.ReadLine(), out pontos) || !pontosValidos.Contains(pontos))
                    {
                        Console.WriteLine("Valor inválido. Insira 1, 2, 3 ou 5.");
                        pontos = 0;
                    }
                }

                Produto novoProduto = new Produto(descricao, pontos);
                produtor.RegistrarProduto(novoProduto); // Chama o método da interface
                Console.WriteLine($"Produto '{descricao}' de {pontos} pontos registrado para {assoc.GetNome()}!");

                Console.Write("Deseja registrar outro produto para este produtor? (s/n): ");
                continuar = Console.ReadLine().Trim().ToLower().FirstOrDefault();
            }
        }

        static void CadastrarDemanda(string cpf, string descricao, int tempoPrevistoEmFracoes, int prazo)
        {
            Associado criador = amx.LocalizarAssociado(cpf);
            if (criador == null) throw new KeyNotFoundException("Associado criador não encontrado!");

            Demanda demanda = new Demanda(criador, descricao, tempoPrevistoEmFracoes, prazo);
            
            // O registro da demanda no associado é feito no método RegistrarDemanda do Associado,
            // que já faz a verificação de créditos.
            criador.RegistrarDemanda(demanda); // Isso também pode lançar CreditosInsuficientesException

            amx.CadastrarDemanda(demanda); // Cadastra a demanda na associação principal

            Console.WriteLine($"Demanda cadastrada com sucesso! ID: {demanda.GetHashCode()}");

            Console.WriteLine("Agora, atribua as habilidades necessárias para esta demanda.");
            char continuar = 's';
            while (continuar == 's')
            {
                Console.Write("Digite o ID da habilidade a ser adicionada: ");
                if (int.TryParse(Console.ReadLine(), out int idHab))
                {
                    AtribuirHabilidadeDemanda(demanda.GetHashCode(), idHab);
                }
                else
                {
                    Console.WriteLine("ID de habilidade inválido. Tentativa ignorada.");
                }
                Console.Write("Deseja adicionar outra habilidade? (s/n): ");
                continuar = Console.ReadLine().Trim().ToLower().FirstOrDefault();
            }
        }

        static void AtribuirHabilidadeAssociado(string cpf, int idHabilidade)
        {
            Associado associado = amx.LocalizarAssociado(cpf);
            if (associado == null) throw new KeyNotFoundException("Associado não encontrado.");

            // Verifica se o associado é um Prestador usando a interface
            if (!(associado is IHabil prestador))
                throw new InvalidOperationException("Apenas associados do tipo 'Prestador' podem ter habilidades de serviço.");

            Habilidade habilidade = amx.LocalizarHabilidade(idHabilidade);
            if (habilidade == null) throw new KeyNotFoundException("Habilidade não encontrada.");

            prestador.AtribuirHabilidade(habilidade); // Chama o método da interface
            Console.WriteLine($"Habilidade '{habilidade.GetHabilidade()}' atribuída com sucesso para '{associado.GetNome()}'!");
        }

        static void AtribuirHabilidadeDemanda(int idDemanda, int idHabilidade)
        {
            Habilidade habilidade = amx.LocalizarHabilidade(idHabilidade);
            if (habilidade == null) throw new KeyNotFoundException("Habilidade não encontrada.");

            Demanda demanda = amx.LocalizarDemanda(idDemanda);
            if (demanda == null) throw new KeyNotFoundException("Demanda não encontrada.");

            demanda.AtribuirHabilidadesNecessarias(habilidade);
            Console.WriteLine($"Habilidade '{habilidade.GetHabilidade()}' atribuída com sucesso para a demanda {idDemanda}!");
        }

        static void DefinirAtendente(int idDemanda)
        {
            Demanda demandaAtendimento = amx.LocalizarDemanda(idDemanda);
            if (demandaAtendimento == null) throw new KeyNotFoundException("Demanda não encontrada.");

            if (demandaAtendimento.GetAtendente() != null)
                throw new InvalidOperationException("Esta demanda já possui um atendente.");

            // amx.DefinirMelhorPrestadorParaDemanda retorna Prestador (que implementa IPrestadorServico)
            Prestador melhorPrestador = amx.DefinirMelhorPrestadorParaDemanda(demandaAtendimento);
            if (melhorPrestador == null)
                throw new InvalidOperationException("Nenhum prestador qualificado e disponível foi encontrado para esta demanda.");

            // O método AtribuirDemanda do Prestador já chama Demanda.FinalizarDemanda(this) internamente.
            melhorPrestador.AtribuirDemanda(demandaAtendimento);
            Console.WriteLine($"Demanda {idDemanda} atribuída com sucesso para o atendente {melhorPrestador.GetNome()}!");
        }

        static void FinalizarDemanda(int idDemanda)
        {
            Demanda demanda = amx.LocalizarDemanda(idDemanda);
            if (demanda == null) throw new KeyNotFoundException("Demanda não encontrada.");

            if (demanda.FoiFinalizada())
            {
                Console.WriteLine($"A demanda {idDemanda} já está finalizada.");
                return;
            }

            //se não tiver atendente, vamos solicitar um.
            if (demanda.GetAtendente() == null)
            {
                Console.WriteLine("Esta demanda ainda não tem um atendente. Atribuindo um atendente automaticamente para finalizar...");
                DefinirAtendente(idDemanda); 
                demanda = amx.LocalizarDemanda(idDemanda); 
                if (demanda.GetAtendente() == null) 
                {
                     throw new InvalidOperationException("Não foi possível encontrar ou atribuir um atendente para finalizar a demanda.");
                }
            }
            // O método Demanda.FinalizarDemanda agora espera um Associado (atendente)
            demanda.FinalizarDemanda(demanda.GetAtendente()); // Chama com o atendente já definido

            Console.WriteLine($"Demanda {idDemanda} finalizada com sucesso!");
            Console.WriteLine(demanda.ToString());
        }

        static void ListarAssociados()
        {
            Dictionary<string, Associado> entidades = amx.GetAssociados();
            Console.WriteLine($"\n======== Associados Cadastrados ========\n");
            if (entidades.Count == 0)
            {
                Console.WriteLine("Não há associados cadastrados.");
                return;
            }
            foreach (var assoc in entidades.Values)
            {
                Console.WriteLine(assoc.ToString());
                Console.WriteLine("---------------------------------");
            }
        }

        static void ListarHabilidades()
        {
            Dictionary<int, Habilidade> entidades = amx.GetHabilidades();
            Console.WriteLine($"\n======== Habilidades Cadastradas ========\n");
            if (entidades.Count == 0)
            {
                Console.WriteLine("Não há habilidades cadastrados.");
                return;
            }
            foreach (var hab in entidades.Values)
            {
                Console.WriteLine(hab.ToString());
                Console.WriteLine("---------------------------------");
            }
        }

        static void ListarDemandas()
        {
            Dictionary<int, Demanda> entidades = amx.GetDemandas();
            Console.WriteLine($"\n======== Demandas Cadastradas ========\n");
            if (entidades.Count == 0)
            {
                Console.WriteLine("Não há demandas cadastradas.");
                return;
            }
            foreach (var dem in entidades.Values)
            {
                Console.WriteLine(dem.ToString());
                Console.WriteLine("---------------------------------");
            }
        }

        static void ListarProdutos()
        {
            List<Produto> entidades = amx.GetAllProdutos();
            Console.WriteLine($"\n======== Produtos Cadastrados ========\n");
            if (!entidades.Any())
            {
                Console.WriteLine("Não há produtos cadastrados no sistema.");
                return;
            }
            foreach (var prod in entidades)
            {
                Console.WriteLine(prod.ToString());
                Console.WriteLine("---------------------------------");
            }
        }

       
        public static int LerTempoPrevistoValido()
        {
            int tempoPrevistoMinutos = 0;
            while (true)
            {
                Console.Write("Digite o tempo previsto (em minutos — múltiplos de 30): ");
                if (int.TryParse(Console.ReadLine(), out tempoPrevistoMinutos) && tempoPrevistoMinutos > 0 && tempoPrevistoMinutos % 30 == 0)
                {
                    // Retorna o tempo em frações de 30 minutos
                    return tempoPrevistoMinutos / 30;
                }
                Console.WriteLine("Tempo inválido. Digite um número positivo que seja múltiplo de 30 (ex: 30, 60, 90...).");
            }
        }

        private static void CarregarHabilidades()
        {
            // O caminho do arquivo deve ser corrigido para ser mais robusto em diferentes ambientes
            // (por exemplo, ao executar de um diretório diferente).
            // Coloque o arquivo 'habilidadesAMX.txt' na pasta 'bin/Debug' ou 'bin/Release' do seu projeto.
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "habilidadesAMX.txt");

            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"Aviso: O arquivo 'habilidadesAMX.txt' não foi encontrado em '{fullPath}'. Nenhuma habilidade padrão será carregada.");
                Console.WriteLine("Você pode cadastrar habilidades manualmente ou verificar o caminho do arquivo.");
                return; // Não lança exceção, apenas avisa e continua
            }

            string[] leituras = File.ReadAllLines(fullPath);

            foreach (string linha in leituras)
            {
                string[] partes = linha.Split(' ');
                if (partes.Length == 2 && int.TryParse(partes[1], out int pontos))
                {
                    try
                    {
                        amx.CadastrarHabilidade(new Habilidade(partes[0], pontos));
                    }
                    catch (ArgumentException ex) // Captura se os pontos estiverem fora do range (1,2,3)
                    {
                        Console.WriteLine($"Erro ao carregar habilidade '{partes[0]}': {ex.Message}. Verifique o arquivo habilidadesAMX.txt.");
                    }
                    catch (InvalidOperationException ex) // Captura se a habilidade já existe
                    {
                        Console.WriteLine($"Aviso: Habilidade '{partes[0]}' já cadastrada ao tentar carregar do arquivo. {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Formato inválido na linha do arquivo habilidadesAMX.txt: '{linha}'. Esperado 'NomeHabilidade Pontos'.");
                }
            }
            Console.WriteLine($"Carregadas {amx.GetHabilidades().Count} habilidades do arquivo.");
        }
    }
}
