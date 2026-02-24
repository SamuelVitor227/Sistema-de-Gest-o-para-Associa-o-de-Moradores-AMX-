using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociacaoAMX
{
    /// <summary>
    /// Classe estática utilitária para gerar uma carga de dados de teste (associados e habilidades)
    /// para uma instância da <see cref="Associacao"/>.
    /// </summary>
    public static class GerarAssociados
    {
        /// <summary>
        /// Instância estática de Random para garantir a aleatoriedade na geração de dados.
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Contador global estático para garantir a unicidade de nomes e CPFs gerados.
        /// </summary>
        private static int contadorGlobal = 1;

        /// <summary>
        /// Ponto de entrada principal para executar a geração de dados de associados.
        /// Popula a associação com 250 associados (20% produtores e 80% prestadores) e suas respectivas informações.
        /// </summary>
        /// <param name="associacao">A instância da <see cref="Associacao"/> a ser populada.</param>
        /// <exception cref="ArgumentNullException">Lançada se a associação fornecida for nula.</exception>
        /// <exception cref="InvalidOperationException">Lançada se não houver pelo menos 7 habilidades cadastradas, o que é um pré-requisito.</exception>
        public static void Executar(Associacao associacao)
        {
            if (associacao == null)
                throw new ArgumentNullException(nameof(associacao), "A associação não pode ser nula.");

            // Garante que as habilidades padrão estejam cadastradas antes de criar associados
            GerarHabilidadesPadrao(associacao);

            List<Habilidade> todasHabilidades = associacao.GetHabilidades().Values.ToList();
            if (todasHabilidades.Count < 7)
            {
                throw new InvalidOperationException("É necessário ter pelo menos 7 habilidades cadastradas para a geração de carga de associados.");
            }

            int totalAssociados = 250;
            int totalProdutores = (int)(totalAssociados * 0.20); 
            int totalPrestadores = totalAssociados - totalProdutores;

            Console.WriteLine($"Gerando {totalProdutores} Produtores...");
            CriarProdutores(associacao, totalProdutores);

            Console.WriteLine($"Gerando {totalPrestadores} Prestadores com diferentes quantidades de habilidades...");
            int com7 = (int)(totalPrestadores * 0.10);
            int com5 = (int)(totalPrestadores * 0.20);
            int com4 = (int)(totalPrestadores * 0.20);
            int com3 = (int)(totalPrestadores * 0.20);
            int com2 = totalPrestadores - (com7 + com5 + com4 + com3);

            CriarPrestadores(associacao, com7, 7, todasHabilidades);
            CriarPrestadores(associacao, com5, 5, todasHabilidades);
            CriarPrestadores(associacao, com4, 4, todasHabilidades);
            CriarPrestadores(associacao, com3, 3, todasHabilidades);
            CriarPrestadores(associacao, com2, 2, todasHabilidades);

            Console.WriteLine($"Geração de {associacao.GetAssociados().Count} associados concluída.");
        }

        /// <summary>
        /// Garante que um conjunto mínimo de habilidades padrão exista na associação.
        /// </summary>
        /// <param name="associacao">A instância da associação a ser verificada e, se necessário, populada com habilidades.</param>
        private static void GerarHabilidadesPadrao(Associacao associacao)
        {
            if (associacao.GetHabilidades().Count >= 7) return;

            string[] nomesHabilidades = {
                "Eletricista", "Encanador", "Jardineiro", "Pintor", "Marceneiro",
                "Pedreiro", "Desenvolvedor Web", "Designer Gráfico", "Consultor Financeiro", "Contador",
                "Cozinheiro", "Motorista", "Costureira", "Professor", "Terapeuta"
            };

            foreach (string nome in nomesHabilidades)
            {
                // Verifica se a habilidade já existe antes de tentar cadastrar
                if (!associacao.GetHabilidades().Values.Any(h => h.GetHabilidade().Equals(nome, StringComparison.OrdinalIgnoreCase)))
                {
                    try
                    {
                        associacao.CadastrarHabilidade(new Habilidade(nome, random.Next(1, 4)));
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Erro ao gerar habilidade padrão '{nome}': {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Cria e cadastra uma quantidade específica de associados do tipo <see cref="Produtor"/>.
        /// </summary>
        /// <param name="associacao">A instância da associação onde os produtores serão cadastrados.</param>
        /// <param name="quantidade">O número de produtores a serem criados.</param>
        private static void CriarProdutores(Associacao associacao, int quantidade)
        {
            int[] pontosProduto = { 1, 2, 3, 5 };
            for (int i = 0; i < quantidade; i++)
            {
                string nome = $"Produtor {contadorGlobal}";
                string cpf = contadorGlobal.ToString().PadLeft(11, '0'); // Gera um CPF simples

                Produtor novoProdutor = new Produtor(nome, cpf);

                int numProdutos = random.Next(1, 4); // Entre 1 e 3 produtos por produtor
                for (int j = 0; j < numProdutos; j++)
                {
                    string nomeProduto = $"Produto {(char)('A' + j)} de {nome}";
                    int pontos = pontosProduto[random.Next(pontosProduto.Length)];
                    novoProdutor.RegistrarProduto(new Produto(nomeProduto, pontos));
                }

                associacao.CadastrarAssociado(novoProdutor);
                contadorGlobal++;
            }
        }

        /// <summary>
        /// Cria e cadastra uma quantidade específica de associados do tipo <see cref="Prestador"/>, 
        /// atribuindo a cada um um número definido de habilidades aleatórias.
        /// </summary>
        /// <param name="associacao">A instância da associação onde os prestadores serão cadastrados.</param>
        /// <param name="quantidade">O número de prestadores a serem criados neste lote.</param>
        /// <param name="qtdHabilidades">O número exato de habilidades a serem atribuídas a cada prestador.</param>
        /// <param name="todasHabilidades">A lista completa de habilidades disponíveis para seleção.</param>
        private static void CriarPrestadores(Associacao associacao, int quantidade, int qtdHabilidades, List<Habilidade> todasHabilidades)
        {
            for (int i = 0; i < quantidade; i++)
            {
                string nome = $"Prestador {contadorGlobal}";
                string cpf = contadorGlobal.ToString().PadLeft(11, '0'); 

                Prestador novoPrestador = new Prestador(nome, cpf);

                List<Habilidade> habilidadesDoAssociado = todasHabilidades
                    .OrderBy(x => random.Next())
                    .Take(qtdHabilidades)       
                    .ToList();

                foreach (Habilidade habilidade in habilidadesDoAssociado)
                {
                    novoPrestador.AtribuirHabilidade(habilidade);
                }

                associacao.CadastrarAssociado(novoPrestador);
                contadorGlobal++;
            }
        }
    }
}