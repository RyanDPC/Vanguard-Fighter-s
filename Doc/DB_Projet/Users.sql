-- Il faut faire Attention car il ne faut pas qu'il existe déjà les utilisateurs au préalable pour que cela marche (Mais tout s'execute et ne casse en aucun cas les autres requêtes)!!!

-- Création de le rôle "Joueur" et affectation de droit(Lire les informations des armes, Créer une commande, Lire toutes les commandes)
/*J'ai créer un Utilisateur "Filipe" avec des droits de SELECT et INSERT dans des tables prédéfinis. En l'affectant au role "joueur".*/

CREATE ROLE Joueur;
CREATE USER 'oscar'@'' IDENTIFIED BY 'osc';
GRANT 'Joueur' TO 'oscar'@'';
GRANT SELECT ON db_space_invaders.t_arme TO Joueur;
GRANT SELECT ON db_space_invaders.t_commande TO Joueur;
GRANT INSERT ON db_space_invaders.t_commande TO Joueur;

-- Création de "l'administrateur du jeu" et affectation de droit()
/*J'ai créer un Utilisateur (Jean-Claude) avec tous les droits et "WITH GRAND OPTION" pour avoir un droits sur les autres utilisateurs. En lui affectant le rôle administrateur*/
CREATE ROLE Administrateur;
CREATE USER 'Jean-Claude'@'' IDENTIFIED BY 'jea-cla';
GRANT 'Administrateur' TO 'Jean-Claude'@'';
GRANT ALL PRIVILEGES ON db_space_invaders.* TO Administrateur WITH GRANT OPTION;

-- Création du "rôle de la Gestionnaire de la boutique" + un utilisateur qui detient le rôle et affectation de droit()
/*J'ai créer un rôle "Gestionnaire_Boutique, avec un utilisateur "Gerard" à qui je donne le rôle de Gestionnaire_Boutique et 
je lui donne des Droits specifiques pour chaques tables.*/
CREATE ROLE Gestionnaire_Boutique;
CREATE USER 'Gerard'@'' IDENTIFIED BY 'Gera';
GRANT Gestionnaire_Boutique TO 'Gerard'@'';
GRANT SELECT ON db_space_invaders.t_joueur TO Gestionnaire_Boutique;
GRANT SELECT, UPDATE, DELETE ON db_space_invaders.t_arme TO Gestionnaire_Boutique;
GRANT SELECT ON db_space_invaders.t_commande TO Gestionnaire_Boutique;

SET DEFAULT ROLE Joueur TO 'oscar'@'';
SET DEFAULT ROLE Administrateur TO 'Jean-Claude'@'';
SET DEFAULT ROLE Gestionnaire_Boutique TO 'Gerard'@'';