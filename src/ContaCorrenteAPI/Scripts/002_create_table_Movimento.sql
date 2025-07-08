CREATE TABLE IF NOT EXISTS Movimento (
    Id TEXT PRIMARY KEY,
    ContaId TEXT NOT NULL,
    DataHora TEXT NOT NULL,
    Valor REAL NOT NULL,
    Tipo TEXT NOT NULL,
    IdempotentKey TEXT NOT NULL
);
