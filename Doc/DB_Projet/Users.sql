--Création de l'utillisateur "Joueur" et affectation de droit(Lire les informations des armes, Créer une commande, Lire toutes les commandes)
CREATE USER 'Joueur'@'localhost' IDENTIFIED BY 'JOUE';
GRAND SELECT ON db_space_invaders.t_arme TO 'Joueur'@'localhost';
GRAND SELECT ON db_space_invaders.t_commande TO 'Joueur'@'localhost';
GRAND INSERT ON db_space_invaders.t_commande TO 'Joueur'@'localhost';

--Création de "l'administrateur du jeu" et affectation de droit()
CREATE USER 'Administrateur'@'localhost' IDENTIFIED BY 'Admin';
GRAND ALL PRIVILEGES ON db_space_invaders TO 'Administrateur'@'localhost' WITH GRAND OPTION;

--Création du "rôle de la Gestionnaire de la boutique" + un utilisateur qui detient le rôle et affectation de droit()
CREATE ROLE 'Gestionnaire_Boutique';
CREATE USER 'Gerard'@'localhost' IDENTIFIED BY 'Gera';
GRAND 'Gestionnaire_Boutique' TO 'Gerard'@'localhost';
GRAND SELECT ON db_space_invaders.t_joueur TO 'Gestionnaire_Boutique';
GRAND SELECT, UPDATE, DELETE ON db_space_invaders.t_arme TO 'Gestionnaire_Boutique';
GRAND SELECT ON db_space_invaders.t_commande TO 'Gestionnaire_Boutique';