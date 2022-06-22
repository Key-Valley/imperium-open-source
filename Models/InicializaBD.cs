using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fatec_Facilities.Models;
using BC = BCrypt.Net.BCrypt;


namespace Fatec_Facilities.Models
{
    public class InicializaBD
    {
        public static void Initialize(Contexto contexto)
        {

            if (contexto.Database.CanConnect())
            {
                contexto.Database.CanConnect();
            }
            else
            {
                contexto.Database.EnsureCreated();
            }

            if (contexto.Usuarios.Any())
            {
                return;
            }

            var bloco = new Bloco[]
            {
                new Bloco{Id = 1, Nome = "Bloco A"},
                new Bloco{Id = 2, Nome = "Bloco B"},
                new Bloco{Id = 3,Nome = "Bloco C"},
            };

            var locais = new Local[]
            {
                new Local{Id = 1, Nome = "Sala A1", BlocoID = 1},
                new Local{Id = 2, Nome = "Sala A2", BlocoID = 1},
                new Local{Id = 3, Nome = "Lab B1", BlocoID = 2},
                new Local{Id = 4, Nome = "Lab B2", BlocoID = 2},
            };

            var categorias = new Categoria[]
            {
                new Categoria{Id = 1, Nome = "Iluminação"},
                new Categoria{Id = 2, Nome = "Ventilação"},
                new Categoria{Id = 3, Nome = "Mobilia"},
                new Categoria{Id = 4, Nome = "Alvenaria"},
            };

            var status = new Status[]
            {
                new Status{Id = 1, Descricao = "Em Espera" },
                new Status{Id = 2, Descricao = "Aprovada" },
                new Status{Id = 3, Descricao = "Negada" },
                new Status{Id = 4, Descricao = "Em Andamento" },
                new Status{Id = 5, Descricao = "Finalizada" },
            };

            var endereco = new Endereco[]{
                new Endereco{
                    Id = 1, 
                    Logradouro = "Av. Prof. João Rodrigues",
                    Numero = "1501",
                    Bairro = "Jardim Esperanca",
                    Cep = "12517-010",
                    Complemento = "Predio",
                    Cidade = "Guaratingueta",
                    Estado = "Sao Paulo",
                    Pais = "Brasil"
                }
            };

            var empresa = new Empresa[]
            {
                new Empresa{
                    Id = 1,
                    Nome = "Centro Estadual De Educacao Tecnologica Paula Souza",
                    NomeFantasia = "Faculdade De Tecnologia Prof. Joao Mod",
                    Cnpj = "62.823.257/0106-78",
                    EnderecoId = 1
                }
            };

            var usuario = new Usuario[]
            {
                new Usuario{Id = 1, Email = "vicente@fatec.sp.gov.br", Nome="Vicente", Rm = "2525", Senha = BC.HashPassword("123"), Gestor = true}
            };

            var usuarioEmpresa = new UsuarioEmpresa[]
            {
                new UsuarioEmpresa{Id = 1, EmpresaId = 1, UsuarioId = 1}
            };

            var conta = new ContaMensal[]
            {

            };

            var fornecedor = new Fornecedor[]
            {
                new Fornecedor{Id = 1, Nome = "Joaquim", CNPJ = "1233321", Contato = "1111111111", Endereco = "Rua Ipanema"}
            };


            var fornecedorCategoria = new FornecedorCategoria[]
            {
                new FornecedorCategoria{Id = 1, FornecedorId = 1, CategoriaId = 1},
                new FornecedorCategoria{Id = 2, FornecedorId = 1, CategoriaId = 2}
            };

            var blocoEmpresa = new BlocoEmpresa[]
            {
                new BlocoEmpresa{Id = 1, BlocoId = 1, EmpresaId = 1},
                new BlocoEmpresa{Id = 2, BlocoId = 2, EmpresaId = 1},
                new BlocoEmpresa{Id = 3, BlocoId = 3, EmpresaId = 1}
            };

            var categoriaEmpresa = new CategoriaEmpresa[]
            {
                new CategoriaEmpresa{Id = 1, CategoriaId = 1, EmpresaId = 1},
                new CategoriaEmpresa{Id = 2, CategoriaId = 2, EmpresaId = 1},
                new CategoriaEmpresa{Id = 3, CategoriaId = 3, EmpresaId = 1},
                new CategoriaEmpresa{Id = 4, CategoriaId = 4, EmpresaId = 1},
            };

            foreach (var item in bloco)
            {
                contexto.Blocos.Add(item);
            }

            foreach (var item in locais)
            {
                contexto.Local.Add(item);
            }

            foreach (var item in categorias)
            {
                contexto.Categoria.Add(item);
            }

            foreach (var item in status)
            {
                contexto.Status.Add(item);
            }

            foreach (var item in endereco)
            {
                contexto.Endereco.Add(item);
            }

            foreach (var item in empresa)
            {
                contexto.Empresa.Add(item);
            }

            foreach (var item in usuario)
            {
                contexto.Usuarios.Add(item);
            }

            foreach (var item in usuarioEmpresa)
            {
                contexto.UsuarioEmpresa.Add(item);
            }

            foreach (var item in blocoEmpresa)
            {
                contexto.BlocoEmpresa.Add(item);
            }

            foreach (var item in categoriaEmpresa)
            {
                contexto.CategoriaEmpresa.Add(item);
            }

            contexto.SaveChanges();
        }
    }
}
