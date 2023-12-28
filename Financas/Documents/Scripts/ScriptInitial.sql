-- Usuario
CREATE TABLE Usuario (
	UsuarioId INT IDENTITY(1,1),
	Nome VARCHAR(50) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Senha VARCHAR(255) NOT NULL,
	DataNascimento DATE NOT NULL,
	DataCadastro DATETIME NOT NULL,

	CONSTRAINT PK_UsuarioId PRIMARY KEY (UsuarioId),
	CONSTRAINT UQ_Email UNIQUE (Email)
);

-- Conta
CREATE TABLE Conta (
	ContaId INT IDENTITY(1,1),
	Nome VARCHAR(50) NOT NULL,
	Principal BIT NOT NULL,
	Balanco DECIMAL(14,2) NOT NULL,
	DataCadastro DATETIME,
	UsuarioId INT NOT NULL,

	CONSTRAINT PK_ContaId PRIMARY KEY (ContaId)
);

-- Cartão
CREATE TABLE Cartao (
	CartaoId INT IDENTITY(1,1),
	Nome VARCHAR(50) NOT NULL,
	LimiteCredito DECIMAL(14,2) NOT NULL,
	LimiteCreditoAtual DECIMAL(14,2) NOT NULL,
	Vencimento DATE,
	Principal BIT NOT NULL,
	ContaId INT NOT NULL,

	CONSTRAINT PK_CartaoId PRIMARY KEY (CartaoId)
);

-- Fatura
CREATE TABLE Fatura (
	FaturaId INT IDENTITY(1,1),
	Valor DECIMAL(14,2) NOT NULL,
	Mes INT NOT NULL,
	Ano INT NOT NULL,
	CartaoId INT NOT NULL

	CONSTRAINT PK_FaturaId PRIMARY KEY (FaturaId)
);

-- Pagamento Fatura
CREATE TABLE PagamentoFatura (
	PagamentoFaturaId INT IDENTITY(1,1),
	ValorPago DECIMAL(14,2) NOT NULL,
	FaturaId INT NOT NULL

	CONSTRAINT PK_PagamentoFaturaId PRIMARY KEY (PagamentoFaturaId)
);

-- Tipo Operação
CREATE TABLE TipoOp (
	TipoOpId INT IDENTITY(1,1),
	Nome VARCHAR(20) NOT NULL,

	CONSTRAINT PK_TipoOpId PRIMARY KEY (TipoOpId)
);

-- Tipo Operação Cartão
CREATE TABLE TipoOpCartao (
	TipoOpCartaoId INT IDENTITY(1,1),
	Nome VARCHAR(20) NOT NULL,

	CONSTRAINT PK_TipoOpCartaoId PRIMARY KEY (TipoOpCartaoId)
);

-- Categoria Operação
CREATE TABLE CategoriaOp (
	CategoriaOpId INT IDENTITY(1,1),
	Nome VARCHAR(20) NOT NULL,
	Descricao VARCHAR(100),

	CONSTRAINT PK_CategoriaOpId PRIMARY KEY (CategoriaOpId)
);

-- Operações Cartão
CREATE TABLE OpCartao (
	OpCartaoId INT IDENTITY(1,1),
	Descricao VARCHAR(100) NOT NULL,
	Valor DECIMAL(14,2) NOT NULL,
	DataOp DATETIME NOT NULL,
	Parcelado BIT NOT NULL,
	QuantidadeParcelas INT NOT NULL,
	ValorPorParcela DECIMAL(14,2),
	CartaoId INT NOT NULL,
	CategoriaOpId INT NOT NULL,
	TipoOpCartaoId INT NOT NULL,

	CONSTRAINT PK_OpCartaoId PRIMARY KEY (OpCartaoId)
);

-- Operações Conta
CREATE TABLE OpConta (
	OpContaId INT IDENTITY(1,1),
	Descricao VARCHAR(100) NOT NULL,
	Valor DECIMAL(14,2) NOT NULL,
	DataOp DATETIME NOT NULL,
	ContaId INT NOT NULL,
	CategoriaOpId INT NOT NULL,
	TipoOpId INT NOT NULL,

	CONSTRAINT PK_OpContaId PRIMARY KEY (OpContaId)
);

-- Balaço
CREATE TABLE Balanco (
	BalancoId INT IDENTITY(1,1),
	Mes INT NOT NULL,
	Ano INT NOT NULL,
	Link VARCHAR(150) NOT NULL,
	Total DECIMAL(14,2),
	Despesa DECIMAL(14,2),
	Receita DECIMAL(14,2),
	UsuarioId INT NOT NULL,

	CONSTRAINT PK_BalancoId PRIMARY KEY (BalancoId),
	CONSTRAINT UQ_Link UNIQUE (Link),

	INDEX idx_Link (Link)
);

--Titulo
CREATE TABLE Titulo (
	TituloId INT IDENTITY(1,1),
	Descricao VARCHAR(255),
	Observacao TEXT,
	Parcela INT NOT NULL,
	NumeroParcelas INT NOT NULL,
	Valor DECIMAL(14,2) NOT NULL,
	TipoTitulo INT NOT NULL,
	StatusTitulo VARCHAR(20),
	DataTitulo DATETIME,
	DataPagamento DATETIME,
	ContaId INT NOT NULL,

	CONSTRAINT PK_TituloId PRIMARY KEY (TituloId)
);



-- FK
--- Tabela Conta
ALTER TABLE Conta
ADD CONSTRAINT FK_Conta_Usuario
FOREIGN KEY (UsuarioId)
REFERENCES Usuario(UsuarioId)
ON DELETE CASCADE;

--- Tabela Cartao
ALTER TABLE Cartao
ADD CONSTRAINT FK_Cartao_Conta
FOREIGN KEY (ContaId)
REFERENCES Conta(ContaId);

--- Tabela Fatura
ALTER TABLE Fatura
ADD CONSTRAINT FK_Fatura_Cartao
FOREIGN KEY (CartaoId)
REFERENCES Cartao(CartaoId);

-- Tabela PagamentoFatura
ALTER TABLE PagamentoFatura
ADD CONSTRAINT FK_PagamentoFatura_Fatura
FOREIGN KEY (FaturaId)
REFERENCES Fatura(FaturaId);

--- Tabela OpCartao
ALTER TABLE OpCartao
ADD CONSTRAINT FK_OpCartao_Cartao
FOREIGN KEY (CartaoId)
REFERENCES Cartao(CartaoId);

ALTER TABLE OpCartao
ADD CONSTRAINT FK_OpCartao_CategoriaOp
FOREIGN KEY (CategoriaOpId)
REFERENCES CategoriaOp(CategoriaOpId);

ALTER TABLE OpCartao
ADD CONSTRAINT FK_OpCartao_TipoOpCartao
FOREIGN KEY (TipoOpCartaoId)
REFERENCES TipoOpCartao(TipoOpCartaoId);

--- Tabela OpConta
ALTER TABLE OpConta
ADD CONSTRAINT FK_OpConta_Conta
FOREIGN KEY (ContaId)
REFERENCES Conta(ContaId);

ALTER TABLE OpConta
ADD CONSTRAINT FK_OpConta_CategoriaOp
FOREIGN KEY (CategoriaOpId)
REFERENCES CategoriaOp(CategoriaOpId);

ALTER TABLE OpConta
ADD CONSTRAINT FK_OpConta_TipoOp
FOREIGN KEY (TipoOpId)
REFERENCES TipoOp(TipoOpId);

---Tabela Balanco
ALTER TABLE Balanco
ADD CONSTRAINT FK_Balanco_Usuario
FOREIGN KEY (UsuarioId)
REFERENCES Usuario(UsuarioId);

---Tabela Titulo
ALTER TABLE Titulo
ADD CONSTRAINT FK_Conta_Titulo
FOREIGN KEY (ContaId) 
REFERENCES Conta(ContaId);



-- Inserts
--- TipoOp
INSERT INTO TipoOp(Nome) VALUES ('Receita');
INSERT INTO TipoOp(Nome) VALUES ('Despesa');

--- TipoOpCartao
INSERT INTO TipoOpCartao(Nome) VALUES ('Credito');
INSERT INTO TipoOpCartao(Nome) VALUES ('Debito');

--- CategoriaOp
INSERT INTO CategoriaOp(Nome) VALUES ('Comida');
INSERT INTO CategoriaOp(Nome) VALUES ('Lazer');
INSERT INTO CategoriaOp(Nome) VALUES ('Educação');
INSERT INTO CategoriaOp(Nome) VALUES ('Internet');
INSERT INTO CategoriaOp(Nome) VALUES ('Salario');
