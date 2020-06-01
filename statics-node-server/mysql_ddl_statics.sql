CREATE SCHEMA `statics`;
USE `statics`;

CREATE TABLE Log (
	id INT AUTO_INCREMENT PRIMARY KEY,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    objectType CHAR(20),
    action CHAR(20),
    details TEXT
);
