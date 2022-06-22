## Requisitos
- C#
- .NET CORE 3.1
- Node
- MySQL (MariaDB)

## Tecnologias
- Consultas LINQ
- Entity framework v3.1
- MVC
- [Bootstrap v4.6](https://getbootstrap.com/docs/4.6/getting-started/introduction/)
- Razor
- SCSS
- [FusionCharts](https://www.fusioncharts.com/dev/fusioncharts-aspnet-visualization/getting-started/overview-of-fusioncharts-net-viz)

## Recomendações
- Usar do terminal para execução do projeto
- Usar o VS CODE para edição do código

## Comandos
- Para executar o projeto: `dotnet watch run`
- Para gerar o Scaffold: 
`dotnet aspnet-codegenerator controller -name NomeDoController -m NomeDoModel -dc Contexto --relativeFolderPath Controllers -l _NomeDoLayout --referenceScriptLibraries`

## Observações
- O serviço já está podendo ser adicionado como um PWA.
- A estilização é feita em sua maior parte pelo Bootstrap, mas caso queira usar estilos customizados use o SCSS, para mudanças de layout é necessário que o essa linha:
![image](https://user-images.githubusercontent.com/52165006/174906410-32719f13-e22e-4446-a917-1daff9a5b151.png)
no arquivo **Fatec_Facilities.csproj** seja "descomentada", para o serviço em Node que gera o CSS final executar.
Para o <i>deploy</i> no Azure (onde os alunos tem conta gratuita com 200$ de crédito) é necessário a criação de uma instância do MariaDB para o banco de dados e um serviço para hospedar a aplicação.
- Para a conexão com o banco de PROD é necessário passar a string de conexão nesse formato:
`Server=url-do-server;Database=nome-do-banco;Uid=usuario-do-banco;Pwd=senha-do-usuario;`
- A estrutura do banco de dados será enviada a parte, mas caso prefira pode gerar usando as <i>migrations</i> do Entity Framework.
- Para configuração do envio de emails, pode-se usar qualquer serviço de email.
- O preenchimento básico dos dados está no model `InicializaDB.cs` lá também contem a informação do usuário admin para o login.
