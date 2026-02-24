using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociacaoAMX
{
    /// <summary>
    /// Representa um Associado do tipo Produtor, que pode registrar produtos na Associação.
    /// Herda da classe <see cref="Associado"/> e implementa a interface <see cref="IProdutor"/>.
    /// </summary>
    public class Produtor : Associado, IProdutor
    {
        /// <summary>
        /// Lista privada que armazena todos os produtos criados e registrados por este produtor.
        /// </summary>
        private List<Produto> _produtosCriados;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Produtor"/>.
        /// </summary>
        /// <param name="nome">O nome do produtor.</param>
        /// <param name="cpf">O CPF do produtor.</param>
        public Produtor(string nome, string cpf) : base(nome, cpf)
        {
            _produtosCriados = new List<Produto>();
        }

        /// <summary>
        /// Adiciona um novo produto à lista de produtos criados pelo produtor.
        /// </summary>
        /// <param name="produto">O produto a ser registrado.</param>
        /// <exception cref="ArgumentNullException">Lançada se o produto fornecido for nulo.</exception>
        public void RegistrarProduto(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto), "Produto não pode ser nulo.");

            _produtosCriados.Add(produto);
        }

        /// <summary>
        /// Calcula o saldo final de créditos do produtor.
        /// O cálculo é baseado nos créditos ganhos por produtos cedidos (1 crédito a cada 10 pontos)
        /// menos os débitos de demandas criadas pelo próprio produtor.
        /// </summary>
        /// <returns>O total de créditos do produtor.</returns>
        public override int CalcularCreditos()
        {
            int creditosProdutos = CalcularPontosProduto() / 10;
            int debitosDemandasCriadas = _demandasCriadas.Values.Sum(d => (d.GetTempoPrevisto() / 2) * 1);

            return creditosProdutos - debitosDemandasCriadas;
        }

        /// <summary>
        /// Calcula a soma total dos pontos de todos os produtos registrados pelo produtor.
        /// </summary>
        /// <returns>A pontuação total dos produtos.</returns>
        public int CalcularPontosProduto()
        {
            return _produtosCriados.Sum(p => p.GetPontos());
        }

        /// <summary>
        /// Retorna a lista de produtos criados por este produtor.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="Produto"/>.</returns>
        public List<Produto> GetProdutos()
        {
            return _produtosCriados;
        }

        /// <summary>
        /// Retorna uma representação em string do objeto Produtor.
        /// </summary>
        /// <returns>Uma string que representa o produtor, incluindo informações da classe base, seu tipo e o total de pontos de seus produtos.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, Tipo: Produtor, Total de Pontos de Produto: {CalcularPontosProduto()}";
        }
    }
}