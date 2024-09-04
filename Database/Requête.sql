--Requête N°1
SELECT jouPseudo AS Pseudo, jouNombrePoints FROM t_joueur ORDER BY jouNombrePoints DESC LIMIT 5;

--Requête N°2
SELECT MIN(armPRIX), MAX(armPRIX), AVG(armPRIX) FROM t_arme;

--Requête N°3
SELECT j.IdJoueur, COUNT(c.IdCommande) AS NombreCommandes 
FROM t_joueur j 
LEFT JOIN t_commande c 
ON j.IdJoueur = c.fkjoueur 
GROUP BY j.IdJoueur 
ORDER BY NombreCommandes DESC;

--Requête N°4
SELECT j.IdJoueur, COUNT(c.IdCommande) AS NombreCommandes
FROM t_joueur j
LEFT JOIN t_commande c ON j.IdJoueur = c.fkjoueur
GROUP BY j.IdJoueur
HAVING COUNT(c.IdCommande) >= 2
ORDER BY NombreCommandes DESC;

--Requête N°5
SELECT
    j.jouPseudo AS pseudo,
    a.armNom AS nom_arme
FROM
    t_commande c
LEFT JOIN t_joueur j ON c.fkJoueur = j.idJoueur
LEFT JOIN t_arsenal ar ON c.idCommande = ar.fkCommande
LEFT JOIN t_arme a ON ar.fkArme = a.idArme;

--Requête N°6
SELECT j.jouPseudo AS pseudo, SUM(armPRIX) AS TotalDepense FROM t_joueur
LEFT JOIN t_joueur j ON j.fkJoueur = j.idJoueur
LEFT JOIN t_arsenal ar ON c.idCommande = ar.fkCommande
LEFT JOIN t_Commande a ON ar.fkArme = a.idArme
GROUP BY t_joueur ORDER BY TotalDepense DESC LIMIT 10;
--Requête N°7
--Requête N°8
--Requête N°9
--Requête N°10
