CREATE TABLE IF NOT EXISTS users (
    email TEXT PRIMARY KEY,
    verified INT,
    subscribeCode TEXT,
    unsubscribeCode TEXT
)