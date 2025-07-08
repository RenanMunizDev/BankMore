CREATE TABLE IF NOT EXISTS Transferencias (
    Id TEXT PRIMARY KEY,
    ContaOrigemNumero TEXT NOT NULL,
    ContaDestinoNumero TEXT NOT NULL,
    Valor REAL NOT NULL,
    Data TEXT NOT NULL,
    IdempotentKey TEXT NOT NULL
);
