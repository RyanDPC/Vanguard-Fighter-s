
USE db_space_invaders;

/*Création de le rôle "Joueur" et affectation de droit(Lire les informations des armes, Créer une commande, Lire toutes les commandes)
J'ai créer un Utilisateur "Filipe" avec des droits de SELECT et INSERT dans des tables prédéfinis. En l'affectant au role "joueur".*/

CREATE ROLE Joueur;
GRANT SELECT ON db_space_invaders.t_arme TO Joueur;
GRANT SELECT ON db_space_invaders.t_commande TO Joueur;
GRANT INSERT ON db_space_invaders.t_commande TO Joueur;

CREATE USER 'oscar'@'localhost' IDENTIFIED BY 'osc';
GRANT Joueur TO 'oscar'@'localhost';
SET DEFAULT ROLE Joueur TO 'oscar'@'localhost';

/*Création de "l'administrateur du jeu" et affectation de droit()
J'ai créer un Utilisateur (Jean-Claude) avec tous les droits et "WITH GRAND OPTION" pour avoir un droits sur les autres utilisateurs. En lui affectant le rôle administrateur*/

CREATE ROLE Administrateur;
GRANT ALL PRIVILEGES ON db_space_invaders TO Administrateur WITH GRANT OPTION;

CREATE USER 'Jean-Claude'@'localhost' IDENTIFIED BY 'jea-cla';
GRANT Administrateur TO 'Jean-Claude'@'localhost';
SET DEFAULT ROLE Administrateur TO 'Jean-Claude'@'localhost';


/*Création du "rôle de la Gestionnaire de la boutique" + un utilisateur qui detient le rôle et affectation de droit()
J'ai créer un rôle "Gestionnaire_Boutique, avec un utilisateur "Gerard" à qui je donne le rôle de Gestionnaire_Boutique et 
je lui donne des Droits specifiques pour chaques tables.*/

CREATE ROLE Gestionnaire_Boutique;
GRANT SELECT ON db_space_invaders.t_joueur TO Gestionnaire_Boutique;
GRANT SELECT, UPDATE, DELETE ON db_space_invaders.t_arme TO Gestionnaire_Boutique;
GRANT SELECT ON db_space_invaders.t_commande TO Gestionnaire_Boutique;

CREATE USER 'Gerard'@'localhost' IDENTIFIED BY 'Gera';
GRANT Gestionnaire_Boutique TO 'Gerard'@'localhost';
SET DEFAULT ROLE Gestionnaire_Boutique TO 'Gerard'@'localhost';
