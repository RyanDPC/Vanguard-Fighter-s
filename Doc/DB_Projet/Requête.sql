--"À la place de mettre t.[c]ommande.idJoueur je prefere mettre [c].idjoueur pour une meilleure visibilite de la requete et c'est plus simple a retenir pour pour moi" 
--(OUBLI !Ne pas oublier l'ordre quand j'appelle les id et les fk!)

--Requête N°1
--J'ai mis un "ORDER BY" pour dire que l'organisation du resultat doit etre fait avec "jouNombrePoints" avec 5 ligne max.
SELECT 
    jouPseudo AS 'Pseudo',
    jouNombrePoints 
FROM 
    t_joueur 
ORDER BY jouNombrePoints 
DESC LIMIT 5;

--Requête N°2 
--Min = le prix minimum, MAX = le prix maximum, AVG = le prix moyen de toutes les armes.
SELECT 
    MIN(armPRIX),
    MAX(armPRIX),
    AVG(armPRIX) 
FROM 
    t_arme;

--Requête N°3
--Je mets j.Idjoueur pour dire que l'id provient de t_joueur la meme chose pour c.IdCommande, avec un count avant car dans la consigne nous voulons le nbr total de commandes par joueur. Ensuite j'appelle jsute les fk a se relier entres elles. 
SELECT 
    j.IdJoueur,
    COUNT(c.IdCommande) AS 'NombreCommandes' 
FROM
    t_joueur j 
LEFT JOIN
    t_commande c 
ON 
    j.IdJoueur = c.fkjoueur 
GROUP BY j.IdJoueur 
ORDER BY NombreCommandes DESC;

--Requête N°4
--J'ai copie colle la requete N°3 et j'ai juste change la partie "GROUP BY" pour mettre un "HAVING COUNT ... >= " pour regarder ceux qui ont passe plus de 2 commandes.
SELECT 
    j.IdJoueur,
    COUNT(c.IdCommande) AS 'NombreCommandes'
FROM 
    t_joueur j
LEFT JOIN 
    t_commande c 
ON 
    j.IdJoueur = c.fkjoueur
GROUP BY j.IdJoueur
HAVING COUNT(c.IdCommande) >= 2
ORDER BY NombreCommandes DESC;

--Requête N°5
--Dans cette requete j'ai affiche ce que la consigne voulait et j'ai regarder quelle fk etait avec quelle id et dans quelle table pour juste les reliers entres elles.
SELECT 
    j.jouPseudo AS 'pseudo', 
    a.armNom AS 'nom_arme'
FROM 
    t_commande c
LEFT JOIN 
    t_joueur j 
ON 
    c.fkJoueur = j.idJoueur
LEFT JOIN 
    t_arsenal ar 
ON 
    c.idCommande = ar.fkCommande
LEFT JOIN 
    t_arme a 
ON 
    ar.fkArme = a.idArme;

--Requête N°6
--SUM pour avoir le total depense par chaque joueur, ensuite j'ai juste "ORDER BY" pour les ordonner en fonction du montant le plus eleve en premier et j'ai limiter a 10 avec LIMIT.
SELECT
    j.jouPseudo AS 'pseudo', 
    SUM(armPRIX) AS 'TotalDepense'
FROM 
    t_joueur
LEFT JOIN 
    t_joueur j 
ON 
    j.fkJoueur = j.idJoueur
LEFT JOIN 
    t_arsenal ar 
ON 
    ar.idCommande = a.fkCommande
LEFT JOIN 
    t_Commande a 
ON 
    ar.fkArme = a.idArme
GROUP BY t_joueur 
ORDER BY TotalDepense DESC LIMIT 10;


--Requête N°7
SELECT 
    j.jouPseudo AS 'pseudo', 
    c.comNumeroCommande AS 'numero de commande' 
FROM 
    t_joueur j
LEFT JOIN 
    t_commande c 
ON 
    c.fkJoueur = j.idJoueur
LEFT JOIN 
    t_detail_commande dc 
ON 
    dc.fkCommande = c.idCommande
GROUP BY j.jouPseudo, c.comNumeroCommande;


--Requête N°8
SELECT c.comNumeroCommande AS 'numero de commande', j.jouPseudo AS 'pseudo'
FROM t_commande c
LEFT JOIN t_joueur j ON c.fkJoueur = j.idJoueur;
--Requête N°9
SELECT j.jouPseudo AS 'pseudo', COUNT(dc.fkArme) AS 'nombre_armes'
FROM t_joueur j
LEFT JOIN t_commande c ON c.fkJoueur = j.idJoueur
LEFT JOIN t_detail_commande dc ON dc.fkCommande = c.idCommande
GROUP BY j.jouPseudo;

--Requête N°10
SELECT j.jouPseudo AS 'pseudo', COUNT(DISTINCT dc.fkArme) AS 'nombre_armes_diff'
FROM t_joueur j
LEFT JOIN t_commande c ON c.fkJoueur = j.idJoueur
LEFT JOIN t_detail_commande dc ON dc.fkCommande = c.idCommande
GROUP BY j.jouPseudo
HAVING COUNT(DISTINCT dc.fkArme) > 3;
