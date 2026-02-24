using AssociacaoAMX;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociacaoAMX
{
    /// <summary>
    /// Representa o nível de dificuldade "Simples" para uma demanda.
    /// Esta classe implementa a interface <see cref="IDemandavel"/> e define o multiplicador de créditos base (padrão).
    /// </summary>
    internal class DemandaSimples : IDemandavel
    {
        /// <summary>
        /// O valor constante do multiplicador de créditos para demandas simples.
        /// </summary>
        private const int MULTIPLICADOR = 1;

        /// <summary>
        /// Retorna o multiplicador de créditos para uma demanda de dificuldade simples.
        /// </summary>
        /// <returns>O valor inteiro do multiplicador, que é 1.</returns>
        public int MultiplicadorCreditos()
        {
            return MULTIPLICADOR;
        }
    }
}