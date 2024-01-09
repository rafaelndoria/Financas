# Documentação da API
## Endpoints

1. ## Usuario
* #### GET /api/Usuario
O endpoint api/Usuario é responsável por listar todos os usuários registrados no banco de dados do seu sistema, exibindo apenas informações pertinentes.
#### Parâmetros
Sem parâmetros
###### Exemplo de parâmetros:
```
```
#### Resposta
##### 200 OK
Caso essa resposta aconteça, vai ser retornado as informações dos usuarios cadastrado no sistema.
###### Exemplo de resposta:
```
[
    {
        "usuarioId": 1002,
        "nome": "Rafael",
        "email": "rafael@gmail.com",
        "dataNascimento": "2000-12-21",
        "dataCadastro": "2023-12-20T21:58:01.163"
    }
]
```

***

* #### GET /api/Usuario/{id}
O endpoint api/Usuario/{id} é responsável por recuperar e exibir informações de um usuário específico no banco de dados com base no ID fornecido como parâmetro de consulta
#### Parâmetros
`id`: ID do usuário a ser recuperado.
###### Exemplo de parâmetros:
```
/api/Usuario/1
```
#### Resposta
##### 200 OK
Caso essa resposta aconteça, vai ser retornado as informações do usuario cadastrado no sistema.
###### Exemplo de resposta:
```
[
    {
        "usuarioId": 1002,
        "nome": "Rafael",
        "email": "rafael@gmail.com",
        "dataNascimento": "2000-12-21",
        "dataCadastro": "2023-12-20T21:58:01.163"
    }
]
```

***

* #### DELETE /api/Usuario/{id}
O endpoint api/Usuario/{id} é responsável por excluir um usuario especifico no sistema com base no ID fornecido como paramêtro. Ao excluir um usuario, as contas, cartões, e operações serão deletada juntamente com o usuario.
#### Parâmetros
`id`: ID do usuário a ser deletado.
###### Exemplo de parâmetros:
```
/api/Usuario/1
```
#### Resposta
##### 200 OK
Caso essa resposta aconteça, o usuario foi delatado do sistema sem apresentar algum erro durante a operação.
###### Exemplo de resposta:
```
```
##### 400 BAD REQUEST
Caso essa resposta aconteça, ocorreu algum erro durante a operação.
###### Exemplo de resposta:
```
```

***

* #### POST /api/Usuario/Register
O endpoint api/Usuario/Register é responsável por registrar um novo usuário no sistema. Ao enviar uma requisição POST para este endpoint, o sistema processará as informações fornecidas no corpo da requisição para criar e registrar um novo usuário.
#### Parâmetros
`nome`[string]: Nome do usuario.

`email`[string]: Email do usuario.

`senha`[string]: Senha do usuario.

`dataNascimento`[date]: Data de Nascimento do usuario.
###### Exemplo de parâmetros:
```
{
    "Nome": "Rafael",
    "Email": "api@outlook.com",
    "Senha": "123",
    "DataNascimento": "29/03/2001"
}
```
#### Resposta
##### 200 OK
Caso essa resposta aconteça, o usuario foi incluido no sistema, irá ser demonstrado o token para autenticação.
###### Exemplo de resposta:
```
{
  "token": ""
}
```
##### 400 BAD REQUEST
Caso essa resposta aconteça, ocorreu algum erro durante a operação.
###### Exemplo de resposta:
```
{
  "Nao foi possivel incluir o usuario"
}
```

***

* #### POST /api/Usuario/Login
O endpoint api/Usuario/Register é responsável por autenticar o usuário no sistema.
#### Parâmetros
`email`[string]: Email do usuario.

`senha`[string]: Senha do usuario.
###### Exemplo de parâmetros:
```
{
    "Email": "api@outlook.com",
    "Senha": "123"
}
```
#### Resposta
##### 200 OK
Caso essa resposta aconteça, o usuario foi autenticado no sistema, irá ser demonstrado o token para autenticação.
###### Exemplo de resposta:
```
{
  "token": ""
}
```
##### 401 UNAUTHORIZED
Caso essa resposta aconteça, ocorreu algum erro durante a operação e o usuario não foi autenticado no sistema.
###### Exemplo de resposta:
```
{
  "Email não cadastrado no sistema"
}
```
```
{
  "Senha ou email invalidos"
}
```

***

* #### PUT /api/Usuario/{id}
O endpoint api/Usuario/{id} é responsável por atualizar as informações de um usuario especifico atraves do seu ID passado como parametro.
#### Parâmetros
Caso não deseje alterar alguma informação do usuario, deixe o parametro como nullo.
`nome`[string]: Nome do usuario.

`email`[string]: Email do usuario.

`senha`[string]: Senha do usuario.

`dataNascimento`[date]: Data de Nascimento do usuario.
###### Exemplo de parâmetros:
```
{
    "Nome": "Rafael",
    "Email": "",
    "Senha": "123",
    "DataNascimento": ""
}
```
#### Resposta
##### 200 OK
Caso essa resposta aconteça, o usuario foi atualizado com as novas informações no sistema.
###### Exemplo de resposta:
```
{
}
```
##### 400 BAD REQUEST
Caso essa resposta aconteça, ocorreu algum erro durante a operação.
###### Exemplo de resposta:
```
{
  "Nao foi possivel atualizar o usuario"
}
```
