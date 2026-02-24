using AssociacaoAMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociacaoAMX
{
    /// <summary>
    /// Representa o nível de dificuldade "Difícil" para uma demanda.
    /// Esta classe implementa a interface <see cref="IDemandavel"/> e define um multiplicador de créditos específico.
    /// </summary>
    internal class DemandaDificil : IDemandavel
    {
        /// <summary>
        /// O valor constante do multiplicador de créditos para demandas difíceis.
        /// </summary>
        private const int MULTIPLICADOR = 2;

        /// <summary>
        /// Retorna o multiplicador de créditos para uma demanda de dificuldade difícil.
        /// </summary>
        /// <returns>O valor inteiro do multiplicador, que é 2.</returns>
        public int MultiplicadorCreditos()
        {
            return MULTIPLICADOR;
        }
    }
}