# 🏘️ AMX - Sistema de Gestão para Associação de Moradores

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![POO](https://img.shields.io/badge/POO-Arquitetura-blue?style=for-the-badge)

Um sistema de back-end em C# desenvolvido para gerenciar uma economia colaborativa baseada em créditos, conectando prestadores de serviços e produtores de bens dentro de uma comunidade.

---

## 🎯 Minhas Contribuições (Destaques Técnicos)

Como desenvolvedor neste projeto de equipe, meu foco principal foi a **arquitetura do sistema, modelagem orientada a objetos e regras de negócio**:

* **Arquitetura Core (`Associacao.cs`):** Desenvolvi o motor principal da aplicação, responsável por orquestrar o estado do sistema, gerenciar as coleções de dados (Associados, Demandas) e garantir a integridade das regras de negócio.
* **Modelagem e UML:** Participei do levantamento de requisitos e diagramei a arquitetura do sistema, garantindo baixo acoplamento através do uso de classes abstratas e interfaces (`ICreditavel`, `IDemandavel`).
* **Lógica de Serviços:** Implementação da classe de serviços e das regras de validação para a atribuição e finalização de demandas.
* **Inteligência de Dados:** Fui o desenvolvedor principal do módulo de relatórios analíticos, criando algoritmos para extração de métricas críticas (como o mapeamento do estado financeiro/créditos da associação).

### 📐 Arquitetura do Sistema (Diagrama de Classes)
```mermaid
classDiagram
    class Associacao {
      -List~Associado~ associados
      -List~Demandas~ demandas
      +gerenciarSistema()
      +extrairRelatorioEstado()
    }

    class Associado {
      <<abstract>>
      +String Nome
      +Decimal SaldoCreditos
    }

    class Prestador {
      +List~Habilidade~ Habilidades
    }

    class Produtor {
      +List~Produto~ Produtos
    }

    class Demanda {
      +String Descricao
      +Int TempoPrevisto
      +DateTime Prazo
    }

    Associado <|-- Prestador
    Associado <|-- Produtor
    Associacao "1" *-- "many" Associado : gerencia
    Associacao "1" *-- "many" Demanda : gerencia
    Demanda "many" --> "1" Prestador : atendida por