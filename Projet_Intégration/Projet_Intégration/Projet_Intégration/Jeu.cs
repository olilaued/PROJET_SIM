﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Jeu : Microsoft.Xna.Framework.Game
    {

        const float INTERVALLE_CALCUL_FPS = 1f;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const float PROFONDEUR_DEFAUT = 0.5f;
        const float TEMPS_FIN_DE_PARTIE = 10;
        const string VAINQUEUR_N = "LES NOIRS L'EMPORTENT!";
        const string VAINQUEUR_B = "LES BLANCS L'EMPORTENT!";
        
        
        
        
        GraphicsDeviceManager PériphériqueGraphique { get; set; }
        SpriteBatch GestionSprites { get; set; }

        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        RessourcesManager<Model> GestionnaireDeModèles { get; set; }
        RessourcesManager<SoundEffect> GestionnaireDeSons { get; set; }

        InputManager GestionInput { get; set; }
        CaméraSubjective CaméraJeu { get; set; }
        AfficheurFps UnAfficheurFPS { get; set; }
        Color[] CouleursÉchiquier { get; set; }
        Partie PartiEnCours { get; set; }
        Afficheur3D unAfficheur3D { get; set; }
        public static List<Bouton> ListeDesBoutons { get; protected set; }
        string NomMap { get; set; }
        string TempsÉcrit { get; set; }
        int TempsDePartie { get; set; }
        float TempsRestantB { get; set; }
        float TempsRestantN { get; set; }
        float TempsÉcouléDepuisFinDePartie { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        Vector3 OrigineÉchiquier { get; set; }
        Vector3 PositionCaméra { get; set; }
        Vector3 CibleCaméra { get; set; }
        Vector3 OVCaméra { get; set; }
        public enum GameState { MenuPrincipal, Options,Musique, ClrsÉchiquier, TempsPartie, EnJeu, EnPause}
        public static GameState CurrentGameState { get; set; }
        ZoneDéroulante ArrièrePlanDéroulant { get; set; }
        ZoneDéroulante Anand { get; set; }
        TexteAffichable Gagnant { get; set; }
        TexteAffichable TempsB { get; set; }
        TexteAffichable TempsN { get; set; }
        SoundEffectInstance Chanson { get; set; }
        
        
        
        // Menu principal
        string text1 = "Jouer 1v1";
        string text2 = "Options";
        Bouton B1;
        Bouton B2;


        // Options
        string text3 = "Couleurs de l'echiquier";
        string text4 = "Temps de la partie";
        string text5 = "Musique d'ambiance";
        Bouton B3;
        Bouton B4;
        Bouton B5;


        //Pause
        string msgPause1 = "Reprendre la partie";
        string msgPause2 = "Quitter la partie";
        Bouton B6;
        Bouton B7;

        // CouleursÉchiquier
        
        string clr1 = "Classique";
        string clr2 = "Vert/Blanc";
        string clr3 = "Rouge/Blanc";
        string clr4 = "Rose/Blanc";
        Bouton B8;
        Bouton B9;
        Bouton B10;
        Bouton B11;

        //ChoixTemps
        string temps1 = "15 minutes";
        string temps2 = "30 minutes";
        string temps3 = "45 minutes";
        string temps4 = "60 minutes";
        Bouton B12;
        Bouton B13;
        Bouton B14;
        Bouton B15;

        //ChoixMusique
        string tune1 = "Irlandaise";
        string tune2 = "Classique";
        string tune3 = "Indien";
        string tune4 = "Rock/comptemporain";
        Bouton B16;
        Bouton B17;
        Bouton B18;
        Bouton B19;


       

       
        
       
       


       

        
        
        



       
        

        public Jeu()
        {
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
           
            PériphériqueGraphique.ApplyChanges();

        }

        
        protected override void Initialize()
        {
            
            CurrentGameState = GameState.MenuPrincipal;
            ListeDesBoutons = new List<Bouton>();
            TempsDePartie = 15*60 ;
            NomMap = "Pub/club_map_2";
            OrigineÉchiquier = new Vector3(163.20f,55.28f,-74.17f);
           
           
           
            Vector3 positionObjet = new Vector3(0, 0, 0);
            Vector3 rotationObjet = new Vector3(0, 0, 0);

            PositionCaméra = new Vector3(140.76f, 65.08f, -68.30f);
            CibleCaméra = new Vector3(157.2f, 55.28f, -68.17f);
            OVCaméra = new Vector3(-0.5933704f, 0.8049271f, -0.00201782f);


            CouleursÉchiquier = new Color[3];
            CouleursÉchiquier[0] = Color.NavajoWhite;
            CouleursÉchiquier[1] = Color.Gray;
            CouleursÉchiquier[2] = Color.Aquamarine;
            
            
           
            

            GestionnaireDeFonts = new RessourcesManager<SpriteFont>(this, "Fonts");
            GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
            GestionnaireDeModèles = new RessourcesManager<Model>(this, "Models");
            GestionnaireDeSons = new RessourcesManager<SoundEffect>(this, "Sons");
           
           
            
           
           
            
            GestionInput = new InputManager(this);
            GestionSprites = new SpriteBatch(GraphicsDevice);
 
           
            
            Components.Add(GestionInput);
            Components.Add(ArrièrePlanDéroulant = new ZoneDéroulante(this, "chess", new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), INTERVALLE_MAJ_STANDARD));
            Components.Add(CaméraJeu = new CaméraSubjective(this,PositionCaméra,CibleCaméra,OVCaméra, INTERVALLE_MAJ_STANDARD));
            
           
           

            Services.AddService(typeof(Random), new Random());
            Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnaireDeModèles);
            Services.AddService(typeof(RessourcesManager<SoundEffect>), GestionnaireDeSons);          
            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(Caméra), CaméraJeu);
            Services.AddService(typeof(SpriteBatch), GestionSprites);


           
            CréerMP();
            CréerOptions();
            CréerChoixPause();
            CréerChoixClrsÉchi();
            CréerChoixTemps();
            CréerChoixMusique();
            
            
            
            //******************* MUSIQUE
            
             Chanson = GestionnaireDeSons.Find("Mozart_Lacrimosa").CreateInstance();

            base.Initialize();
           
            
        }
        

        
        protected override void LoadContent()
        {
           
            base.LoadContent();
            
        }

       
        protected override void Update(GameTime gameTime)
        {

            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ > INTERVALLE_MAJ_STANDARD)
            {
                switch (CurrentGameState)
                {
                    case GameState.MenuPrincipal:

                        if (GestionInput.EstNouvelleTouche(Keys.Escape))
                        {
                            this.Exit();
                        }
                        if (B1.Clicked == true)
                        {
                            DéterminerSettings();
                            CommencerPartie();
                            VoilerBoutons(0, 2);
                            CurrentGameState = GameState.EnJeu;
                        }

                        if (B2.Clicked == true)
                        {
                            CurrentGameState = GameState.Options;
                            VoilerBoutons(0, 2);
                            AfficherBoutons(2, 5);
                        }
                        break;

                    case GameState.Options:

                        if (B3.Clicked == true)
                        {
                            CurrentGameState = GameState.ClrsÉchiquier;
                            VoilerBoutons(2, 5);
                            AfficherBoutons(7, 11);
                        }
                        if (B4.Clicked == true)
                        {
                            CurrentGameState = GameState.TempsPartie;
                            VoilerBoutons(2, 5);
                            AfficherBoutons(11, 15);
                        }
                        if (B5.Clicked == true)
                        {
                            CurrentGameState = GameState.Musique;
                            VoilerBoutons(2, 5);
                            AfficherBoutons(15, 19);
                        }
                        if (GestionInput.EstNouvelleTouche(Keys.Escape))
                        {
                            CurrentGameState = GameState.MenuPrincipal;
                            VoilerBoutons(2, 5);
                            AfficherBoutons(0, 2);
                        }
                        break;

                    case GameState.ClrsÉchiquier:
                        if (B8.Clicked || B9.Clicked || B10.Clicked == true || B11.Clicked == true)
                        {
                            CurrentGameState = GameState.Options;
                            VoilerBoutons(7, 11);
                            AfficherBoutons(2, 5);
                        }
                        break;

                    case GameState.TempsPartie:
                        if (B12.Clicked || B13.Clicked || B14.Clicked || B15.Clicked == true)
                        {
                            CurrentGameState = GameState.Options;
                            VoilerBoutons(11, 15);
                            AfficherBoutons(2, 5);
                        }
                        break;
                    case GameState.Musique:
                        if (B16.Clicked || B17.Clicked || B18.Clicked || B19.Clicked == true)
                        {
                            CurrentGameState = GameState.Options;
                            VoilerBoutons(15, 19);
                            AfficherBoutons(2, 5);
                        }
                        break;

                    case GameState.EnPause:

                        if (GestionInput.EstNouvelleTouche(Keys.Escape) || B6.Clicked == true)
                        {
                            CurrentGameState = GameState.EnJeu;
                            VoilerBoutons(5, 7);
                            Components.Remove(Anand);


                        }
                        if (B7.Clicked == true)
                        {

                            VoilerBoutons(5, 7);
                            QuitterPartie();
                        }
                        break;
                    case GameState.EnJeu:


                        if (PartiEnCours.PartieTerminée)
                        {

                            TempsÉcouléDepuisFinDePartie += tempsÉcoulé;
                            if (!Components.Contains(Gagnant))
                            {
                                switch (PartiEnCours.TourActuel.AutreCouleur)
                                {
                                    case "White":
                                        Components.Add(Gagnant = new TexteAffichable(this, "Arial", VAINQUEUR_N, Color.LightGreen, 0, 3.0f, PROFONDEUR_DEFAUT -0.5f));
                                        break;
                                    case "Black":
                                        Components.Add(Gagnant = new TexteAffichable(this, "Arial", VAINQUEUR_B, Color.LightGreen, 0, 3.0f, PROFONDEUR_DEFAUT -0.5f));
                                        break;
                                }
                            }
                            
                            Afficheur3D afficheur3DTemporaire;
                            Components.Add(afficheur3DTemporaire = new Afficheur3D(this));
                            if (TempsÉcouléDepuisFinDePartie >TEMPS_FIN_DE_PARTIE)
                            {
                                QuitterPartie();
                                Components.Remove(Gagnant);
                                Components.Remove(afficheur3DTemporaire);
                                TempsÉcouléDepuisFinDePartie = 0;
                            }


                        }
                        else
                        {
                            if (GestionInput.EstNouvelleTouche(Keys.P))
                            {
                                switch (Chanson.State)
                                {
                                    case SoundState.Playing:
                                        Chanson.Pause();
                                        break;
                                    case SoundState.Paused:
                                        Chanson.Resume();
                                        break;
                                }
                            }
                            if (GestionInput.EstNouvelleTouche(Keys.Escape))
                            {
                                CurrentGameState = GameState.EnPause;
                                Components.Add(Anand = new ZoneDéroulante(this, "Anand", new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), INTERVALLE_MAJ_STANDARD));
                                Components.Remove(B6);
                                Components.Remove(B7);
                                Components.Add(B6);
                                Components.Add(B7);
                                AfficherBoutons(5, 7);
                            }
                            
                            if (PartiEnCours.TourActuel.Couleur == "White" && CaméraJeu.aFiniTourner())
                            {
                               
                                TempsRestantB = TempsRestantB - tempsÉcoulé;
                                ChangerTemps("W");
                                string temps = ((int)(TempsRestantB / 60)).ToString() + ":" + ((int)(TempsRestantB % 60)).ToString();
                                TempsB.ModifierTexte(temps);

                            }
                            else
                            {
                                if (CaméraJeu.aFiniTourner())
                                {
                                    TempsRestantN = TempsRestantN - tempsÉcoulé;
                                    ChangerTemps("N");
                                    string temps = ((int)(TempsRestantN / 60)).ToString() + ":" + ((int)(TempsRestantN % 60)).ToString();
                                    TempsN.ModifierTexte(temps);

                                }
                            }
                            
                            
                        }
                        break;

                }
                 TempsÉcouléDepuisMAJ = 0;
            }
         
            
            base.Update(gameTime);
        }
        void CommencerPartie()
        {
            Chanson.Play();
            CaméraJeu.ResetCaméra(PositionCaméra, CibleCaméra, OVCaméra);
            TempsRestantB = TempsDePartie;
            TempsRestantN = TempsDePartie;
            TempsÉcrit = ((int)(TempsDePartie / 60)).ToString() + ":" + ((int)(TempsDePartie % 60)).ToString();

            ArrièrePlanDéroulant.ModifierActivation();
            Components.Add(PartiEnCours = new Partie(this, TempsDePartie, NomMap, CouleursÉchiquier, OrigineÉchiquier));
            Components.Add(UnAfficheurFPS = new AfficheurFps(this, "Arial", Color.Blue, INTERVALLE_CALCUL_FPS));
            Components.Add(TempsB = new TexteAffichable(this, "Arial",TempsÉcrit, new Vector2((GraphicsDevice.Viewport.Width - UnAfficheurFPS.PositionDroiteBas.X), UnAfficheurFPS.PositionDroiteBas.Y), Color.White, 1f));
            Components.Add(TempsN = new TexteAffichable(this, "Arial", TempsÉcrit, new Vector2((GraphicsDevice.Viewport.Width - UnAfficheurFPS.PositionDroiteBas.X), UnAfficheurFPS.PositionDroiteBas.Y), Color.Black, 1f));
            Components.Add(unAfficheur3D  = new Afficheur3D(this));
        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }

        void CréerMP()
        {
            int indice = 2;
            float valeur = GraphicsDevice.Viewport.Height / indice/3;
            float y = GraphicsDevice.Viewport.Height / 1.8f;
            float x = GraphicsDevice.Viewport.Width / 4;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(B1 = new Bouton(this, "button", "Arial", text1, new Vector2(x, y), new Vector2(2 * longueur, hauteur/2)));
            y += valeur;
            Components.Add(B2 = new Bouton(this, "button", "Arial", text2, new Vector2(x, y), new Vector2(longueur, hauteur/2)));
            ListeDesBoutons.Add(B1); 
            ListeDesBoutons.Add(B2); 
        }

        void CréerOptions()
        {
            int indice = 3;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(B3 = new Bouton(this, "button", "Arial", text3, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B4 = new Bouton(this, "button", "Arial", text4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B5 = new Bouton(this, "button", "Arial", text5, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(B3); 
            ListeDesBoutons.Add(B4); 
            ListeDesBoutons.Add(B5); 
            VoilerBoutons(2, 5);

        }




        void CréerChoixClrsÉchi()
        {
            int indice = 4;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(B8 = new Bouton(this, "button", "Arial", clr1, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B9 = new Bouton(this, "button", "Arial", clr2, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B10 = new Bouton(this, "button", "Arial", clr3, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B11 = new Bouton(this, "button", "Arial", clr4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(B8); 
            ListeDesBoutons.Add(B9); 
            ListeDesBoutons.Add(B10);  
            ListeDesBoutons.Add(B11); 
            VoilerBoutons(7, 11);

        }

        void CréerChoixPause()
        {
            int indice = 2;
            float valeur = GraphicsDevice.Viewport.Height / indice/3;
            float y = GraphicsDevice.Viewport.Height/1.6f;
            float x = GraphicsDevice.Viewport.Width / 4;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(B6 = new Bouton(this, "button", "Arial", msgPause1, new Vector2(x, y), new Vector2(2 * longueur, hauteur/3)));
            y += valeur;
            Components.Add(B7 = new Bouton(this, "button", "Arial", msgPause2, new Vector2(x, y), new Vector2(2 * longueur, hauteur/3)));
           
            ListeDesBoutons.Add(B6); 
            ListeDesBoutons.Add(B7); 
            VoilerBoutons(5,7);

        }

        void CréerChoixTemps()
        {
            int indice = 4;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(B12 = new Bouton(this, "button", "Arial", temps1, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B13 = new Bouton(this, "button", "Arial", temps2, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B14 = new Bouton(this, "button", "Arial", temps3, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B15 = new Bouton(this, "button", "Arial", temps4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(B12);
            ListeDesBoutons.Add(B13); 
            ListeDesBoutons.Add(B14); 
            ListeDesBoutons.Add(B15);
            VoilerBoutons(11, 15);

        }
        void CréerChoixMusique()
        {
            int indice = 4;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(B16 = new Bouton(this, "button", "Arial", tune1, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B17= new Bouton(this, "button", "Arial", tune2, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B18 = new Bouton(this, "button", "Arial", tune3, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B19 = new Bouton(this, "button", "Arial", tune4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(B16); 
            ListeDesBoutons.Add(B17); 
            ListeDesBoutons.Add(B18); 
            ListeDesBoutons.Add(B19); 
            VoilerBoutons(15, 19);

        }
        void DéterminerSettings()
        {
            
            int indexClrÉchi = ListeDesBoutons.FindIndex(7, 4, x => (x.Clicked == true));
            int indexTempsPartie = ListeDesBoutons.FindIndex(11,4, x => (x.Clicked == true));
            int indexMusique = ListeDesBoutons.FindIndex(15, 4, x => (x.Clicked == true));
            

            
            switch (indexClrÉchi)
            {
                case 7: 
                    CouleursÉchiquier[0] = Color.White; 
                    CouleursÉchiquier[1] = Color.Gray; 
                    CouleursÉchiquier[2] = Color.Black; 
                    break;
                case 8: 
                    CouleursÉchiquier[0] = Color.White; 
                    CouleursÉchiquier[1] = Color.Green; 
                    CouleursÉchiquier[2] = Color.Black; 
                    break;
                case 9: 
                    CouleursÉchiquier[0] = Color.White; 
                    CouleursÉchiquier[1] = Color.Red; 
                    CouleursÉchiquier[2] = Color.Black;
                    break;
                case 10: 
                    CouleursÉchiquier[0] = Color.White; 
                    CouleursÉchiquier[1] = Color.Pink; 
                    CouleursÉchiquier[2] = Color.Black; 
                    break;
            }
            switch (indexTempsPartie)
            {
                case 11: 
                    TempsDePartie = 15 * 60; 
                    break;
                case 12: 
                    TempsDePartie = 30 * 60; 
                    break;
                case 13: 
                    TempsDePartie = 45 * 60; 
                    break;
                case 14: 
                    TempsDePartie = 60 * 60; 
                    break;
            }
            switch (indexMusique)
            {
                case 15: 
                    Chanson = GestionnaireDeSons.Find("the_gael").CreateInstance(); 
                    Chanson.IsLooped = true; 
                    break;
                case 16: 
                    Chanson = GestionnaireDeSons.Find("Mozart_Lacrimosa").CreateInstance();
                    Chanson.IsLooped = true; 
                    break;
                case 17: 
                    Chanson = GestionnaireDeSons.Find("Tunak_Tunak").CreateInstance();
                    Chanson.IsLooped = true; 
                    break;
                case 18: 
                    Chanson = GestionnaireDeSons.Find("Muse_Resistance").CreateInstance();
                    Chanson.IsLooped = true; 
                    break;

            }
            

        }
        
        void AfficherBoutons(int indiceA, int indiceB)
        {
            for (int i = indiceA; i < indiceB; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = true;
                ListeDesBoutons.ElementAt(i).Enabled = true;
            }
        }
        
        void VoilerBoutons(int indiceA, int indiceB)
        {
            for (int i = indiceA; i < indiceB; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = false;
                ListeDesBoutons.ElementAt(i).Enabled = false;
            }
        }
        void QuitterPartie()
        {
            CurrentGameState = GameState.MenuPrincipal;
            AfficherBoutons(0, 2);
            PartiEnCours.Retirer();
            Components.Remove(UnAfficheurFPS);
            Components.Remove(TempsB);
            Components.Remove(TempsN);
            Components.Remove(unAfficheur3D);
            Components.Remove(Anand);
            Components.Remove(PartiEnCours.TourActuel);
            ArrièrePlanDéroulant.ModifierActivation();
            Chanson.Stop();
        }
        void ChangerTemps(string couleur)
        {
            switch(couleur)
            {
                case "W": 
                    TempsB.Visible = true;
                    TempsB.Enabled = true;
                    
                    TempsN.Visible = false;
                    TempsN.Enabled = false;
                    break;
                case "N":
                    TempsN.Visible = true;
                    TempsN.Enabled = true;

                    TempsB.Visible = false;
                    TempsB.Enabled = false;
                    break;
            }
        }

       
    }
}
