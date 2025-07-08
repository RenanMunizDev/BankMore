CREATE TABLE IF NOT EXISTS ContaCorrente (
    Id TEXT PRIMARY KEY,
    Agencia TEXT NOT NULL,
    NumeroConta TEXT NOT NULL,
    NomeTitular TEXT NOT NULL,
    Cpf TEXT NOT NULL,
    Ativo BOOLEAN NOT NULL,
    SenhaHash TEXT NOT NULL,
    Saldo REAL NOT NULL
);
