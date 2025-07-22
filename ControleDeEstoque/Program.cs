using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Quantidade { get; set; }

    public override string ToString()
    {
        return $"{Id} - {Nome} ({Quantidade} unidades)";
    }
}

class Program
{
    static string caminhoCsv = "estoque.csv";

    static List<Produto> produtos = new List<Produto>();

    static void Main()
    {
        // Cria o CSV se ele não existir
        if (!File.Exists(caminhoCsv))
        {
            File.WriteAllText(caminhoCsv, "Id;Produto;Quantidade\n");
            Console.WriteLine("Arquivo 'estoque.csv' criado.");
        }

        //  Carrega o conteúdo do arquivo para a lista
        produtos = CarregarEstoque(caminhoCsv);

        bool sair = false;

        while (!sair)
        {
            Console.Clear();
            Console.WriteLine("=== MENU DE ESTOQUE ===");
            Console.WriteLine("1 - Listar produtos");
            Console.WriteLine("2 - Adicionar produto");
            Console.WriteLine("3 - Remover produto");
            Console.WriteLine("4 - Alterar quantidade");
            Console.WriteLine("5 - Sair");
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    Listar();
                    break;
                case "2":
                    Adicionar();
                    SalvarEstoque(produtos, caminhoCsv);
                    break;
                case "3":
                    Remover();
                    SalvarEstoque(produtos, caminhoCsv);
                    break;
                case "4":
                    AlterarQuantidade();
                    SalvarEstoque(produtos, caminhoCsv);
                    break;
                case "5":
                    sair = true;
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            if (!sair)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    // Lê os dados do CSV e monta a lista de produtos
    static List<Produto> CarregarEstoque(string caminho)
    {
        var lista = new List<Produto>();

        var linhas = File.ReadAllLines(caminho).Skip(1); // Pula o cabeçalho

        foreach (var linha in linhas)
        {
            var partes = linha.Split(';');
            if (partes.Length == 3)
            {
                lista.Add(new Produto
                {
                    Id = int.Parse(partes[0]),
                    Nome = partes[1],
                    Quantidade = int.Parse(partes[2])
                });
            }
        }

        return lista;
    }

    // Salva os dados da lista no arquivo CSV
    static void SalvarEstoque(List<Produto> produtos, string caminho)
    {
        var linhas = new List<string> { "Id;Produto;Quantidade" };
        linhas.AddRange(produtos.Select(p => $"{p.Id};{p.Nome};{p.Quantidade}"));
        File.WriteAllLines(caminho, linhas);
    }

    // Lista os produtos no console
    static void Listar()
    {
        Console.WriteLine("\n=== PRODUTOS CADASTRADOS ===");
        if (produtos.Count == 0)
        {
            Console.WriteLine("Nenhum produto cadastrado.");
        }
        else
        {
            foreach (var p in produtos)
            {
                Console.WriteLine(p);
            }
        }
    }

    // Adiciona um novo produto com nome e quantidade
    static void Adicionar()
    {
        Console.Write("\nNome do novo produto: ");
        string nome = Console.ReadLine();

        Console.Write("Quantidade inicial: ");
        if (!int.TryParse(Console.ReadLine(), out int qtd) || qtd < 0)
        {
            Console.WriteLine("Quantidade inválida.");
            return;
        }

        int novoId = produtos.Any() ? produtos.Max(p => p.Id) + 1 : 1;

        produtos.Add(new Produto
        {
            Id = novoId,
            Nome = nome,
            Quantidade = qtd
        });

        Console.WriteLine("Produto adicionado com sucesso!");
    }

    // Remove um produto com base no ID
    static void Remover()
    {
        Console.Write("\nID do produto a remover: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var produto = produtos.FirstOrDefault(p => p.Id == id);
        if (produto != null)
        {
            produtos.Remove(produto);
            Console.WriteLine("Produto removido.");
        }
        else
        {
            Console.WriteLine("Produto não encontrado.");
        }
    }

    // Altera a quantidade de um produto pelo ID
    static void AlterarQuantidade()
    {
        Console.Write("\nID do produto: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var produto = produtos.FirstOrDefault(p => p.Id == id);
        if (produto != null)
        {
            Console.Write($"Nova quantidade para {produto.Nome}: ");
            if (int.TryParse(Console.ReadLine(), out int novaQtd) && novaQtd >= 0)
            {
                produto.Quantidade = novaQtd;
                Console.WriteLine("Quantidade atualizada.");
            }
            else
            {
                Console.WriteLine("Quantidade inválida.");
            }
        }
        else
        {
            Console.WriteLine("Produto não encontrado.");
        }
    }
}
