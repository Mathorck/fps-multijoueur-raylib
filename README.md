# Dead Ops 3D

## Description
Il s'agit d'un **FPS multijoueur** en mode **tous contre tous** (Deathmatch), développé en **C#** en utilisant la bibliothèque graphique **Raylib**. Ce projet a été conçu et réalisé pour les **portes ouvertes du CPNE-TI 2025**.

## Prérequis
- **.NET Framework** (nécessaire pour la compilation du jeu).

## Comment installer le jeu

1. **Téléchargez le fichier ZIP** :
   - [Télécharger le ZIP](https://git.s2.rpn.ch/MonnierM/fps-multijoueur-raylib/-/archive/main/fps-multijoueur-raylib-main.zip)
   
2. **Décompressez le fichier ZIP** dans le répertoire de votre choix.

3. **Compiler le jeu** :
   - Ouvrez un terminal dans le dossier où vous avez décompressé les fichiers.
   - Exécutez la commande suivante pour compiler le jeu :
     ```bash
     cd chemin/vers/mon/dossier
     dotnet build
     ```

4. **Si vous êtes le premier joueur** :
   - Téléchargez et décompressez le fichier **Laragon** pour démarrer un serveur **MySQL**.
   - Ouvrez Laragon et démarrez le serveur MySQL.
   - **Assurez-vous que le serveur MySQL fonctionne correctement avant de lancer le jeu.**

5. **Pour les autres joueurs** :
   - Modifiez le fichier `config` et remplacez l'adresse IP par l'**IP locale** du premier joueur (serveur MySQL).
   - Une fois cela fait, vous pouvez rejoindre la partie du serveur.

## Comment jouer
### Si vous êtes le premier joueur (création du serveur) :
1. Créer le serveur donnez lui un nom

### Si vous voulez rejoindre la partie d'un autre joueur :
1. Lancez l'application.
2. Appuyez simplement sur un serveur dans la **liste des serveurs** pour rejoindre la partie.

### Configuration pour les autres joueurs :
- Ouvrez le fichier `config` et entrez l'**adresse IP locale** du serveur MySQL de l'hôte (premier joueur).

## Images
Les images seront ajoutées prochainement pour illustrer l'interface utilisateur et le gameplay.

## Auteurs
Ce projet a été développé par l'équipe suivante :
- **Fahme Elias**
- **Mazyad Hussein**
- **Mazyad Mehdy**
- **Monnier Mathéo**
- **Calame Quentin**
- **Margotin Virgile**

---

## Remarques importantes

- Ce projet utilise un **serveur MySQL** pour stocker certaines données des joueurs.
- **Premier joueur** : Téléchargez et décompressez Laragon pour activer le serveur MySQL. Assurez-vous qu'il fonctionne avant de démarrer le jeu.
- **Autres joueurs** : Modifiez le fichier `config` pour indiquer l'**adresse IP locale** du serveur MySQL (c'est-à-dire du joueur qui héberge le serveur).
- Si vous avez des soucis de connexion, vérifiez que les paramètres réseau de votre machine autorisent les connexions entrantes.

Amusez-vous bien et n'hésitez pas à partager vos retours !

