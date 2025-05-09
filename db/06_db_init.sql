-- SQLBook: Code

INSERT INTO dbo.Users (name, username, password, mail, ProfilePicture) VALUES
('Beatriz Costa', 'user1', '0b14d501a594442a01c6859541bcb3e8164d183d32937b', 'user1@example.com', NULL),
('Carlos Santos', 'user2', '6cf615d5bcaac778352a8f1f3360d23f02f34ec182e259', 'user2@example.com', NULL),
('Rui Oliveira', 'user3', '5906ac361a137e2d286465cd6588ebb5ac3f5ae9550011', 'user3@example.com', NULL),
('Maria Ferreira', 'user4', 'b97873a40f73abedd8d685a7cd5e5f85e4a9cfb83eac26', 'user4@example.com', NULL),
('Rui Santos', 'user5', '8b2c86ea9cf2ea4eb517fd1e06b74f399e7fec0fef92e3', 'user5@example.com', NULL);

INSERT INTO dbo.Person (bi, name) VALUES
('123456781', 'João Silva'),
('234567892', 'Maria Santos'),
('345678903', 'Pedro Oliveira'),
('456789014', 'Ana Martins'),
('567890125', 'Carlos Ferreira'),
('678901236', 'Beatriz Costa'),
('789012347', 'Rui Martins'),
('890123458', 'Sofia Oliveira'),
('901234569', 'Marta Ferreira'),
('012345670', 'Luís Costa');

INSERT INTO dbo.Representative (person_bi, contact) VALUES
('123456781', '+351912340001'),
('234567892', '+351912340002'),
('345678903', '+351912340003'),
('456789014', '+351912340004'),
('567890125', '+351912340005');

INSERT INTO dbo.Client (client_bi) VALUES
('123456781'),
('234567892'),
('345678903'),
('456789014');

INSERT INTO dbo.Deceased (person_bi, sex, birth_date, marital_status, residence, nationality, picture) VALUES
('678901236', 'M', '1950-04-12', 'Casado', 'Rua 12', 'Portuguesa', NULL),
('789012347', 'F', '1945-08-30', 'Viúvo', 'Rua 34', 'Portuguesa', NULL),
('890123458', 'M', '1960-12-15', 'Divorciado', 'Rua 56', 'Portuguesa', NULL),
('901234569', 'F', '1955-07-22', 'Casado', 'Rua 78', 'Portuguesa', NULL),
('012345670', 'M', '1948-03-03', 'Solteiro', 'Rua 90', 'Portuguesa', NULL);

INSERT INTO dbo.Process (num_process, start_date, status, budget, description, type_of_payment, user_id, client_id, degree_kinship) VALUES
(1001, '2024-01-10', 'Concluído', 2500.00, 'Processo funeral completo', 'Transferência', 1, '123456781', 'Filho'),
(1002, '2024-02-15', 'Pendente', 1800.00, 'Processo em espera', 'Cartão', 2, '234567892', 'Sobrinha'),
(1003, '2024-03-20', 'Cancelado', 2000.00, 'Cliente desistiu', 'Dinheiro', 3, '345678903', 'Irmão'),
(1004, '2024-04-01', 'Concluído', 3000.00, 'Crematório + Urna', 'Transferência', 4, '456789014', 'Pai'),
(1005, '2024-04-12', 'Pendente', 1500.00, 'Aguarda pagamento', 'Cartão', 5, '123456781', 'Neto');

INSERT INTO dbo.Priest (representative_bi, price, title) VALUES
('123456781', 300.00, 'Priest'),
('234567892', 250.00, 'Deacon'),
('345678903', 275.00, 'Pastor');

INSERT INTO dbo.Church (name, location) VALUES
('Igreja Central', 'Lisboa'),
('Capela Norte', 'Porto'),
('Igreja São João', 'Coimbra');

INSERT INTO dbo.Funeral (num_process, funeral_date, location, deceased_bi, church_id, priest_bi) VALUES
(1001, '2024-01-15', 'Igreja Central', '678901236', 1, '123456781'),
(1002, '2024-02-20', 'Capela Norte', '789012347', 2, '234567892'),
(1003, '2024-03-25', 'Igreja São João', '890123458', 3, '345678903'),
(1004, '2024-04-05', 'Capela Nova', '901234569', 1, '123456781'),
(1005, '2024-04-15', 'Igreja Matriz', '012345670', 2, '234567892');

INSERT INTO dbo.Have (priest_bi, church_id) VALUES
('123456781', 1),
('234567892', 2),
('345678903', 3);

INSERT INTO dbo.Cemetery (location, price, contact) VALUES
('Cemitério da Luz', 1200.00, 969969969),
('Cemitério de Carnide', 1100.00, 949949949);

INSERT INTO dbo.Crematory (location, price, contact) VALUES
('Crematório de Lisboa', 1300.00, 919919919),
('Crematório do Porto', 1250.00, 939939939);

INSERT INTO dbo.Products (id, price, stock) VALUES
(1, 500.00, 10),
(2, 300.00, 15),
(3, 250.00, 20),
(4, 800.00, 5),
(5, 1000.00, 2);

INSERT INTO dbo.Florist (nif, name, address, contact) VALUES
(100001, 'Flores da Cidade', 'Rua Flor 1', '+351912300001'),
(100002, 'Rosa Linda', 'Rua Rosa 2', '+351912300002');

INSERT INTO dbo.Flowers (id, process_num, florist_nif, type, quantity, color) VALUES
(1, 1001, 100001, 'Rosas', 20, 'Vermelho'),
(2, 1002, 100002, 'Lírios', 15, 'Branco');

INSERT INTO dbo.Container (id, supplier, size) VALUES
(3, 'Madeiras Lda', 'Grande'),
(4, 'Urnas Portugal', 'Pequeno'),
(5, 'Madeiras Lda', 'Médio');

INSERT INTO dbo.Coffin (id, color, weight) VALUES
(3, 'Castanho', 45.50),
(5, 'Preto', 50.00);

INSERT INTO dbo.Urn (id) VALUES
(4);

INSERT INTO dbo.Cremation (funeral_id, crematory_id, coffin_id, urn_id) VALUES
(1004, 1, 3, 4);

INSERT INTO dbo.Burial (funeral_id, cemetery_id, coffin_id, num_grave) VALUES
(1001, 1, 3, 101),
(1002, 2, 5, 102),
(1003, 1, 3, 103),
(1005, 2, 5, 104);
