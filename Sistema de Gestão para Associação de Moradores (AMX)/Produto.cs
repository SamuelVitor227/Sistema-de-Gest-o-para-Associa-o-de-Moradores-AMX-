using System;
using System.Collections.Generic;

namespace AssociacaoAMX
{
    /// <summary>
    /// Representa um produto que pode ser registrado por um Associado do tipo <see cref="Produtor"/>.
    /// Cada produto possui um nome e uma pontuação associada que se converte em créditos.
    /// </summary>
    public class Produto
    {
        /// <summary>
        /// O nome do produto.
        /// </summary>
        private string _produto;

        /// <summary>
        /// A pontuação do produto (valores permitidos: 1, 2, 3 ou 5).
        /// </summary>
        private int _pontos;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Produto"/>.
        /// </summary>
        /// <param name="produto">O nome do produto. Não pode ser nulo ou vazio.</param>
        /// <param name="pontos">A pontuação do produto, que deve ser 1, 2, 3 ou 5.</param>
        /// <exception cref="ArgumentException">Lançada se o nome do produto for vazio ou se a pontuação estiver fora dos valores permitidos.</exception>
        public Produto(string produto, int pontos)
        {
            if (string.IsNullOrWhiteSpace(produto))
                throw new ArgumentException("O nome do produto não pode estar vazio.");

            if (pontos != 1 && pontos != 2 && pontos != 3 && pontos != 5)
                throw new ArgumentException("Pontos do produto devem ser 1, 2, 3 ou 5.");

            _produto = produto;
            _pontos = pontos;
        }

        /// <summary>
        /// Obtém a pontuação do produto.
        /// </summary>
        /// <returns>
        /// A pontuação atribuída ao produto como um <see cref="int"/>.
        /// </returns>
        public int GetPontos() => _pontos;

        /// <summary>
        /// Determina se o objeto especificado é igual ao produto atual.
        /// A comparação é baseada no nome do produto.
        /// </summary>
        /// <param name="obj">O objeto a ser comparado com o produto atual.</param>
        /// <returns><c>true</c> se o objeto especificado for um <see cref="Produto"/> com o mesmo nome; caso contrário, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Produto p && _produto == p._produto;
        }

        /// <summary>
        /// Retorna o código de hash para este produto, que é baseado no nome do produto.
        /// </summary>
        /// <returns>Um inteiro que representa o código de hash do nome do produto.</returns>
        public override int GetHashCode()
        {
            return _produto.GetHashCode();
        }

        /// <summary>
        /// Retorna uma representação em string do objeto Produto.
        /// </summary>
        /// <returns>Uma string que contém o nome e a pontuação do produto.</returns>
        public override string ToString()
        {
            return $"{_produto} ({_pontos} pontos)";
        }
    }
}