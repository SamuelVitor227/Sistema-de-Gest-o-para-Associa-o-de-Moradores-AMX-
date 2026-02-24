using AssociacaoAMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociacaoAMX
{
    /// <summary>
    /// Fornece métodos auxiliares estáticos para funcionalidades relacionadas a Demandas.
    /// Esta classe não pode ser instanciada.
    /// </summary>
    static class DemandaHelper
    {
        /// <summary>
        /// Determina o nível de dificuldade de uma demanda com base na lista de habilidades necessárias.
        /// </summary>
        /// <remarks>
        /// Uma demanda é considerada "Difícil" se tiver 4 ou mais habilidades, ou se a soma
        /// dos pontos de suas habilidades for 5 ou mais. Caso contrário, é "Simples".
        /// </remarks>
        /// <param name="habilidades">A lista de habilidades associadas à demanda.</param>
        /// <returns>Retorna uma instância de <see cref="DemandaDificil"/> ou <see cref="DemandaSimples"/> 
        /// que implementam a interface <see cref="IDemandavel"/>.</returns>
        public static IDemandavel IdentificarDificuldade(List<Habilidade> habilidades)
        {
            if (habilidades.Count >= 4 || CalcularPontosHabilidade(habilidades) >= 5)
            {
                return new DemandaDificil();
            }

            return new DemandaSimples();
        }

        /// <summary>
        /// Calcula a soma total dos pontos de uma lista de habilidades.
        /// </summary>
        /// <param name="habilidades">A lista de habilidades a ter seus pontos somados.</param>
        /// <returns>A pontuação total como um <see cref="int"/>.</returns>
        private static int CalcularPontosHabilidade(List<Habilidade> habilidades)
        {
            int pontuacaoHabilidades = 0;

            if (habilidades.Count > 0)
            {
                pontuacaoHabilidades = habilidades
                    .Sum(h => h.GetPontuacao());
            }
            return pontuacaoHabilidades;
        }
    }
}