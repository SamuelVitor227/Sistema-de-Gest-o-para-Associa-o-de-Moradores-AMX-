using System;

namespace AssociacaoAMX
{
    /// <summary>
    /// Representa uma habilidade que um Prestador pode possuir ou que uma Demanda pode requerer.
    /// Cada habilidade tem um nome, uma pontuação de dificuldade e um ID único.
    /// </summary>
    public class Habilidade
    {
        /// <summary>
        /// O nome descritivo da habilidade (armazenado em minúsculas).
        /// </summary>
        private string _habilidade;

        /// <summary>
        /// O identificador numérico único para a habilidade.
        /// </summary>
        private int _idHabilidade;

        /// <summary>
        /// A pontuação que define a dificuldade da habilidade (valores de 1 a 3).
        /// </summary>
        private int _pontos;

        /// <summary>
        /// Contador estático para gerar IDs únicos para cada nova habilidade.
        /// </summary>
        private static int s_id = 1;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Habilidade"/>.
        /// A regra de negócio de pontuação (1, 2 ou 3) é aplicada na criação.
        /// </summary>
        /// <param name="habilidade">O nome da habilidade. Não pode ser nulo ou vazio.</param>
        /// <param name="pontos">A pontuação de dificuldade da habilidade, que deve ser 1, 2 ou 3.</param>
        /// <exception cref="ArgumentNullException">Lançada se o nome da habilidade for nulo ou vazio.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Lançada se a pontuação estiver fora do intervalo permitido [1, 3].</exception>
        public Habilidade(string habilidade, int pontos)
        {
            if (string.IsNullOrWhiteSpace(habilidade))
                throw new ArgumentNullException(nameof(habilidade));
            if (pontos < 1 || pontos > 3)
                throw new ArgumentOutOfRangeException(nameof(pontos), "Os pontos de dificuldade devem ser 1, 2 ou 3.");

            _habilidade = habilidade.ToLower();
            _pontos = pontos;
            _idHabilidade = s_id++;
        }

        /// <summary>
        /// Obtém a pontuação de dificuldade da habilidade.
        /// </summary>
        /// <returns>A pontuação da habilidade (1, 2 ou 3).</returns>
        public int GetPontuacao() => _pontos;

        /// <summary>
        /// Obtém o nome da habilidade.
        /// </summary>
        /// <returns>O nome da habilidade em letras minúsculas.</returns>
        public string GetHabilidade() => _habilidade;

        /// <summary>
        /// Retorna o código de hash para esta habilidade, que corresponde ao seu ID único.
        /// </summary>
        /// <returns>Um inteiro que representa o ID único da habilidade.</returns>
        public override int GetHashCode() => _idHabilidade;

        /// <summary>
        /// Determina se o objeto especificado é igual à habilidade atual.
        /// A comparação é baseada no ID único da habilidade.
        /// </summary>
        /// <param name="obj">O objeto a ser comparado com a habilidade atual.</param>
        /// <returns><c>true</c> se o objeto especificado for uma <see cref="Habilidade"/> com o mesmo ID; caso contrário, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Habilidade && obj.GetHashCode() == _idHabilidade;
        }

        /// <summary>
        /// Retorna uma representação em string do objeto Habilidade.
        /// </summary>
        /// <returns>Uma string que contém o ID, nome e pontuação da habilidade.</returns>
        public override string ToString() => $"ID: {_idHabilidade}, Habilidade: {_habilidade}, Pontos: {_pontos}";
    }
}